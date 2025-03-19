using FinBackend.Api.Models;
using FinBackend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BudgetsController : ControllerBase
    {
        private readonly FinContext _db;

        public BudgetsController(FinContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            int uid = GetUserId();
            var list = await _db.PlanBuds
                .Include(x => x.TheCat)
                .Where(x => x.UserId == uid)
                .ToListAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BudgetDto dto)
        {
            int uid = GetUserId();
            var bud = new PlanBud
            {
                UserId = uid,
                CatId = dto.CatId,
                LimitAmt = dto.LimitAmt,
                PeriodTxt = dto.PeriodTxt
            };
            _db.PlanBuds.Add(bud);
            await _db.SaveChangesAsync();
            return Ok(bud);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BudgetDto dto)
        {
            int uid = GetUserId();
            var bud = await _db.PlanBuds.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
            if (bud == null) return NotFound();

            bud.CatId = dto.CatId;
            bud.LimitAmt = dto.LimitAmt;
            bud.PeriodTxt = dto.PeriodTxt;
            await _db.SaveChangesAsync();
            return Ok(bud);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int uid = GetUserId();
            var bud = await _db.PlanBuds.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
            if (bud == null) return NotFound();

            _db.PlanBuds.Remove(bud);
            await _db.SaveChangesAsync();
            return Ok("Deleted");
        }

        private int GetUserId()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            return int.Parse(claim.Value);
        }
    }

    public class BudgetDto
    {
        public int CatId { get; set; }
        public decimal LimitAmt { get; set; }
        public string PeriodTxt { get; set; }
    }
}
