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
            var fines = _dbCtx.Fine
                .Select(f => new FineUpdateDto(
                    f.FineId,
                    f.FineAmount,
                    f.UserId,
                    f.ReservatonId,
                    f.HasPayed)).ToList();
            return Ok(fines);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetFine(int id)
        {
            var fine = _dbCtx.Fine
                .Include(f => f.User)
                .Include(f => f.Reservaton)
                .FirstOrDefault(f => f.FineId == id);
            if (fine == null) return NotFound();
            return Ok(new FineGet1Dto(
                fine.FineId,
                fine.FineAmount,
                new UserUpdateDto(
                    fine.User.UserId,
                    fine.User.UserName,
                    fine.User.HasFine,
                    fine.User.HasLoan,
                    fine.User.IsAdmin),
                new ReservationUpdateDto(
                    fine.Reservaton.ReservationId,
                    fine.Reservaton.UserId,
                    fine.Reservaton.BookId,
                    fine.Reservaton.ReservationDateTime,
                    fine.Reservaton.CatalogId,
                    fine.Reservaton.ReservationReturnDateTime,
                    fine.Reservaton.HasFine,
                    fine.Reservaton.HasReturned,
                    fine.Reservaton.HasLoan),
                fine.HasPayed));
        }

        [HttpPost]
        public IActionResult NewFine([FromBody] FineCreationDto fine)
        {
            _dbCtx.Fine.Add(new Fine(
                fine.FineAmount,
                fine.UserId,
                fine.ReservatonId,
                fine.HasPayed));
            _dbCtx.SaveChanges();
            var insertedFine = _dbCtx.Fine.First(f => f.UserId == fine.UserId);
            return CreatedAtAction(nameof(GetFine), new { id = insertedFine.FineId },
                new FineUpdateDto(
                    insertedFine.FineId,
                    insertedFine.FineAmount,
                    insertedFine.UserId,
                    insertedFine.ReservatonId,
                    insertedFine.HasPayed));
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateFine(int id, [FromBody] FineUpdateDto fine)
        {
            if (id != fine.FineId) return BadRequest();
            Fine? fineToUpdate = _dbCtx.Fine.Find(id);
            if (fineToUpdate == null) return NotFound();
            fineToUpdate.UpdateFine(
                fine.FineId,
                fine.FineAmount,
                fine.ReservatonId,
                fine.UserId,
                fine.HasPayed);
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
