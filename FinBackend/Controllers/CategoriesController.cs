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
    public class CategoriesController : ControllerBase
    {
        private readonly FinContext _db;

        public CategoriesController(FinContext db)
        {
            _db = db;
        }

        private int GetUserId()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            return claim != null ? int.Parse(claim.Value) : 0;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            int userId = GetUserId();
            var list = await _db.Categ
                .Where(x => x.UserId == userId)  //data from logged user
                .OrderBy(x => x.CatName)
                .ToListAsync();

            return Ok(list);
        }



        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategDto dto)
        {
            int userId = GetUserId();

            var cat = new Categ
            {
                CatName = dto.CatName,
                CatInfo = dto.CatInfo,
                UserId = userId
            };

            _db.Categ.Add(cat);
            await _db.SaveChangesAsync();
            return Ok(cat);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategDto dto)
        {
            var cat = await _db.Categ.FirstOrDefaultAsync(x => x.Id == id);
            if (cat == null) return NotFound();

            cat.CatName = dto.CatName;
            cat.CatInfo = dto.CatInfo;
            await _db.SaveChangesAsync();
            return Ok(cat);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _db.Categ.FirstOrDefaultAsync(x => x.Id == id);
            if (cat == null) return NotFound();

            _db.Categ.Remove(cat);
            await _db.SaveChangesAsync();
            return Ok("Deleted");
        }
    }

    public class CategDto
    {
        public string CatName { get; set; }
        public string CatInfo { get; set; }
    }
}
