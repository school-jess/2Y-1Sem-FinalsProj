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
            var fines = _dbCtx.Fine.Select(f => new FineUpdateDto
            {
                AmtPayedSinceLastFine = f.AmtPayedSinceLastFine,
                FineAmount = f.FineAmount,
                FineId = f.FineId,
                UserId = f.UserId
            }).ToList();
            return Ok(fines);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetFine(int id)
        {
            var fine = _dbCtx.Fine.Include(f => f.User).Include(f => f.Reservaton).FirstOrDefault(f => f.FineId == id);
            if (fine == null) return NotFound();
            return Ok(new FineGet1Dto
            {
                AmtPayedSinceLastFine = fine.AmtPayedSinceLastFine,
                FineAmount = fine.FineAmount,
                FineId = fine.FineId,
                User = new UserUpdateDto
                {
                    HasFine = fine.User.HasFine,
                    HasLoan = fine.User.HasLoan,
                    UserId = fine.User.UserId,
                    UserName = fine.User.UserName
                },
                Reservation = new ReservationUpdateDto
                {
                    BookId = fine.Reservaton.BookId,
                    CatalogId = fine.Reservaton.CatalogId,
                    ReservationId = fine.Reservaton.ReservationId,
                    ReservationDateTime = fine.Reservaton.ReservationDateTime,
                    UserId = fine.Reservaton.UserId,
                    ReservationReturnDateTime = fine.Reservaton.ReservationReturnDateTime
                }
            });
        }

        [HttpPost]
        public IActionResult NewFine([FromBody] FineCreationDto fine)
        {
            _dbCtx.Fine.Add(new Fine
            {
                AmtPayedSinceLastFine = fine.AmtPayedSinceLastFine,
                FineAmount = fine.FineAmount,
                UserId = fine.UserId
            });
            _dbCtx.SaveChanges();
            var insertedFine = _dbCtx.Fine.First(f => f.UserId == fine.UserId);
            return CreatedAtAction(nameof(GetFine), new { id = insertedFine.FineId }, new FineUpdateDto
            {
                AmtPayedSinceLastFine = insertedFine.AmtPayedSinceLastFine,
                FineAmount = insertedFine.FineAmount,
                FineId = insertedFine.FineId,
                UserId = insertedFine.UserId
            });
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateFine(int id, [FromBody] FineUpdateDto fine)
        {
            if (id != fine.FineId) return BadRequest();
            Fine updatedFine = new Fine
            {
                AmtPayedSinceLastFine = fine.AmtPayedSinceLastFine,
                FineAmount = fine.FineAmount,
                FineId = fine.FineId,
                UserId = fine.UserId
            };
            _dbCtx.Entry(updatedFine).State = EntityState.Modified;
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
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
