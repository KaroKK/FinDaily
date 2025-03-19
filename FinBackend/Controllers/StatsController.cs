using System.Text.Json;
using FinBackend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("api/stats")]
public class StatsController(FinContext db) : ControllerBase
{
    private readonly FinContext _db = db;

    [HttpGet]
    public async Task<IActionResult> GetStats([FromQuery] string from, [FromQuery] string to)
    {
        try
        {
            int uid = GetUserId();

            if (!DateTime.TryParse(from, out DateTime fromDate) || !DateTime.TryParse(to, out DateTime toDate))
            {
                return BadRequest("Invalid datetime format.");
            }

            fromDate = DateTime.SpecifyKind(fromDate, DateTimeKind.Utc);
            toDate = DateTime.SpecifyKind(toDate, DateTimeKind.Utc);

            if (fromDate > toDate)
            {
                return BadRequest("From date must be earlier than to date.");
            }


            var query = _db.CashFlows
                .Where(f => f.UserId == uid && f.FlowDate >= fromDate && f.FlowDate <= toDate);

            var totalIncome = await query.Where(f => f.FlowType == "income").SumAsync(f => (decimal?)f.FlowAmount) ?? 0;
            var totalExpense = await query.Where(f => f.FlowType == "expense").SumAsync(f => (decimal?)f.FlowAmount) ?? 0;
            totalExpense = Math.Abs(totalExpense);
            var balance = totalIncome - totalExpense;

            var monthlyFlow = await query
                .GroupBy(f => new { f.FlowDate.Year, f.FlowDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Sum(f => f.FlowAmount)
                })
                .OrderBy(g => g.Year).ThenBy(g => g.Month)
                .ToListAsync();

            var dailyTrend = await _db.CashFlows
      .Where(f => f.UserId == uid && f.FlowDate >= fromDate && f.FlowDate <= toDate)
      .GroupBy(f => f.FlowDate.AddHours(1).Date) // UTC+1
      .Select(g => new
      {
          Date = g.Key,
          Total = g.Sum(f => f.FlowAmount)
      })
      .OrderBy(g => g.Date)
      .ToListAsync();


            var categoryStats = await query
                .Where(f => f.FlowType == "expense" && f.CatId != null)
                .GroupBy(f => f.CatId)
                .Select(g => new
                {
                    CategoryId = g.Key ?? 0,
                    TotalAmount = g.Sum(f => f.FlowAmount)
                })
                .ToListAsync();

            var categoryNames = await _db.Categ
                .Where(c => categoryStats.Select(cs => cs.CategoryId).Contains(c.Id))
                .ToDictionaryAsync(c => c.Id, c => c.CatName);

            var formattedCategoryStats = categoryStats
                .Select(cs => new
                {
                    CategoryName = categoryNames.ContainsKey(cs.CategoryId) ? categoryNames[cs.CategoryId] : "Unknown",
                    TotalAmount = cs.TotalAmount
                })
                .ToList();

            var monthlyComparison = await query
                .GroupBy(f => new { f.FlowDate.Year, f.FlowDate.Month, f.FlowType })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    FlowType = g.Key.FlowType,
                    TotalAmount = g.Sum(f => f.FlowAmount)
                })
                .OrderBy(g => g.Year).ThenBy(g => g.Month)
                .ToListAsync();

            var formattedMonthlyComparison = monthlyComparison
                .GroupBy(mc => new { mc.Year, mc.Month })
                .Select(g => new
                {
                    Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Income = g.FirstOrDefault(f => f.FlowType == "income")?.TotalAmount ?? 0,
                    Expense = Math.Abs(g.FirstOrDefault(f => f.FlowType == "expense")?.TotalAmount ?? 0)
                })
                .ToList();


            return Ok(new
            {
                summary = new { totalIncome, totalExpense, balance },
                monthlyFlow = new
                {
                    labels = monthlyFlow.Select(m => $"{m.Year}-{m.Month:D2}").ToList(),
                    values = monthlyFlow.Select(m => m.Total).ToList()
                },
                dailyTrend = new
                {
                    labels = dailyTrend.Select(d => d.Date.ToString("yyyy-MM-dd")).ToList(),
                    values = dailyTrend.Select(d => d.Total).ToList()
                },
                categoryStats = formattedCategoryStats,
                monthlyComparison = formattedMonthlyComparison
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine("API Error: " + ex.Message);
            return StatusCode(500, "Error");
        }
    }

    private int GetUserId()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == "userId");

        if (claim == null)
        {
            throw new UnauthorizedAccessException("No Id in Token");
        }

        return int.Parse(claim.Value);
    }
}
