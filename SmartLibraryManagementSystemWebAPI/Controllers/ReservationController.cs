using SmartLibraryManagementSystemClassLibrary.Model;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StudentLibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly DatabaseContext _dbCtx;

        public ReservationController(DatabaseContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        [HttpGet]
        public IActionResult GetReservations()
        {
            var reservations = _dbCtx.Reservation.ToList();
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public IActionResult GetReservation(int id)
        {
            var reservation = _dbCtx.Reservation.Find(id);
            if (reservation == null) return NotFound();
            return Ok(reservation);
        }

        [HttpPost]
        public IActionResult NewReservation([FromBody] ReservationCreationDto reservation)
        {
            _dbCtx.Reservation.Add(new Reservation { BookId = reservation.BookId, CatalogId = reservation.CatalogId, ReservationTime = reservation.ReservationTime, UserId = reservation.UserId });
            _dbCtx.SaveChanges();
            var insertedReservation = _dbCtx.Reservation.Find(reservation);
            return CreatedAtAction("", new { reservationId = insertedReservation.ReservationId });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReservation(int id, [FromBody] Reservation reservation)
        {
            if (id != reservation.ReservationId) return BadRequest();
            _dbCtx.Entry(reservation).State = EntityState.Modified;
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            var reservation = _dbCtx.Reservation.Find(id);
            if (reservation == null) return NotFound();
            _dbCtx.Reservation.Remove(reservation);
            _dbCtx.SaveChanges();
            return NoContent();
        }
    }
}
