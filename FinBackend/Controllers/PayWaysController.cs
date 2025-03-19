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
    public class PayWaysController : ControllerBase
    {
        private readonly FinContext _db;

        public PayWaysController(FinContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            int userId = GetUserId();
            var list = await _db.PayWays
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.PayLabel)
                .ToListAsync();

            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PayWayDto dto)
        {
            int userId = GetUserId();

            var pay = new PayWay
            {
                PayLabel = dto.PayLabel,
                PayInfo = dto.PayInfo,
                UserId = userId!
            };

            _db.PayWays.Add(pay);
            await _db.SaveChangesAsync();
            return Ok(pay);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PayWayDto dto)
        {
            int uid = GetUserId();
            var pay = await _db.PayWays.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
            if (pay == null) return NotFound();

            pay.PayLabel = dto.PayLabel;
            pay.PayInfo = dto.PayInfo;
            await _db.SaveChangesAsync();
            return Ok(pay);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            int uid = GetUserId();
            var pay = await _db.PayWays.FirstOrDefaultAsync(x => x.Id == id && x.UserId == uid);
            if (pay == null) return NotFound();

            _db.PayWays.Remove(pay);
            await _db.SaveChangesAsync();
            return Ok("Deleted");
        }

        private int GetUserId()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            return int.Parse(claim.Value);
        }
    }

    public class PayWayDto
    {
        public string PayLabel { get; set; }
        public string PayInfo { get; set; }
    }
}
