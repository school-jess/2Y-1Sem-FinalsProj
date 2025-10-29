using SmartLibraryManagementSystemClassLibrary.Model;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StudentLibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FineController : ControllerBase
    {
        private readonly DatabaseContext _dbCtx;

        public FineController(DatabaseContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        [HttpGet]
        public IActionResult GetFines()
        {
            var fines = _dbCtx.Fine.ToList();
            return Ok(fines);
        }

        [HttpGet("{id}")]
        public IActionResult GetFine(int id)
        {
            var fine = _dbCtx.Fine.Find(id);
            if (fine == null) return NotFound();
            return Ok(fine);
        }

        [HttpPost]
        public IActionResult NewFine([FromBody] FineCreationDto fine)
        {
            _dbCtx.Fine.Add(new Fine { AmtPayedSinceLastFine = fine.AmtPayedSinceLastFine, FineAmount = fine.FineAmount, FineId = fine.UserId });
            _dbCtx.SaveChanges();
            var insertedFine = _dbCtx.Fine.Find(fine);
            return CreatedAtAction("", new { fineId = insertedFine.FineId });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFine(int id, [FromBody] FineUpdateDto fine)
        {
            if (id != fine.FineId) return BadRequest();
            Fine updatedFine = new Fine { AmtPayedSinceLastFine = fine.AmtPayedSinceLastFine, FineAmount = fine.FineAmount, FineId = fine.FineId, UserId = fine.UserId };
            _dbCtx.Entry(updatedFine).State = EntityState.Modified;
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFine(int id)
        {
            var fine = _dbCtx.Fine.Find(id);
            if (fine == null) return NotFound();
            _dbCtx.Fine.Remove(fine);
            _dbCtx.SaveChanges();
            return NoContent();
        }
    }
}
