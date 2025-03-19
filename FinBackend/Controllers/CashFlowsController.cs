using FinBackend.Data;
using FinBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CashFlowsController : ControllerBase
    {
        private readonly FinContext _db;

        public CashFlowsController(FinContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            int userId = GetUserId();

            var list = await _db.CashFlows
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.FlowDate)
                .Include(f => f.Category)
                .Include(f => f.PayWay)
                .Select(f => new
                {
                    f.Id,
                    f.FlowDesc,
                    f.FlowAmount,
                    f.FlowDate,
                    f.FlowType,
                    CategoryName = f.Category != null ? f.Category.CatName : "None",
                    PayLabel = f.PayWay != null ? f.PayWay.PayLabel : "None"
                })
                .ToListAsync();

            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] FlowDto dto)
        {
            int userId = GetUserId();

            var flow = new CashFlow
            {
                FlowDesc = dto.FlowDesc,
                FlowAmount = dto.FlowAmount,
                FlowDate = dto.FlowDate,
                FlowType = dto.FlowType,
                CatId = dto.CatId,
                PayId = dto.PayId,
                UserId = userId
            };

            _db.CashFlows.Add(flow);
            await _db.SaveChangesAsync();
            return Ok(flow);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DelFlow(int id)
        {
            int uid = GetUserId();
            var flow = await _db.CashFlows.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);

            if (flow == null)
            {
                return NotFound("No cashflow");
            }

            _db.CashFlows.Remove(flow);
            await _db.SaveChangesAsync();

            return Ok("Deleted");
        }

        private int GetUserId()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            return int.Parse(claim.Value);
        }
    }

    public class FlowDto
    {
        public string FlowDesc { get; set; }
        public decimal FlowAmount { get; set; }
        public DateTime FlowDate { get; set; }
        public int? CatId { get; set; }
        public int? PayId { get; set; }
        public string FlowType { get; set; }
    }
}
