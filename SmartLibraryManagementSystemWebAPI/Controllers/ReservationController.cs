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
            var reservations = (
                from reservation in _dbCtx.Reservation
                select new ReservationUpdateDto(
                    reservation.ReservationId,
                    reservation.UserId,
                    reservation.BookId,
                    reservation.ReservationDateTime,
                    reservation.CatalogId,
                    reservation.ReservationReturnDateTime,
                    reservation.HasFine,
                    reservation.HasReturned)).ToList();
            return Ok(reservations);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetReservation(int id)
        {
            var reservation = _dbCtx.Reservation
                .Include(r => r.Book)
                .Include(r => r.Catalog)
                .Include(r => r.Fine)
                .Include(r => r.Loan)
                .Include(r => r.User)
                .FirstOrDefault(r => r.ReservationId == id);
            if (reservation == null) return NotFound();
            return Ok(new ReservationGet1Dto(
                reservation.ReservationId,
                new UserUpdateDto(
                    reservation.User.UserId,
                    reservation.User.UserName,
                    reservation.User.HasFine,
                    reservation.User.HasLoan,
                    reservation.User.IsAdmin),
                new BookUpdateDto(
                    reservation.Book.BookId,
                    reservation.Book.BookName,
                    reservation.Book.Author,
                    reservation.Book.ReleaseDate,
                    reservation.Book.Synopsis),
                reservation.ReservationDateTime,
                new CatalogUpdateDto(
                    reservation.Catalog.CatalogId,
                    reservation.Catalog.BookId,
                    reservation.Catalog.Copies,
                    reservation.Catalog.ClassificationId,
                    reservation.Catalog.Genre,
                    reservation.Catalog.CopiesBorrowed),
                new FineUpdateDto(
                    reservation.Fine.FineId,
                    reservation.Fine.FineAmount,
                    reservation.Fine.UserId),
                new LoanUpdateDto(
                    reservation.Loan.LoanId,
                    reservation.Loan.LoanAmount,
                    reservation.Loan.UserId,
                    reservation.Loan.ReservatonId),
                reservation.ReservationReturnDateTime,
                reservation.HasFine,
                reservation.HasReturned));
        }

        [HttpPost]
        public IActionResult NewReservation([FromBody] ReservationCreationDto reservation)
        {
            _dbCtx.Reservation.Add(new Reservation(
                reservation.UserId,
                reservation.BookId,
                reservation.ReservationDateTime,
                reservation.CatalogId,
                reservation.ReservationReturnDateTime,
                reservation.HasFine,
                reservation.HasReturned));
            _dbCtx.SaveChanges();
            var insertedReservation = _dbCtx.Reservation.First(r => r.BookId == reservation.BookId);
            return CreatedAtAction(nameof(GetReservation), new { id = insertedReservation.ReservationId },
                new ReservationUpdateDto(
                    insertedReservation.ReservationId,
                    insertedReservation.UserId,
                    insertedReservation.BookId,
                    insertedReservation.ReservationDateTime,
                    insertedReservation.CatalogId,
                    insertedReservation.ReservationReturnDateTime,
                    insertedReservation.HasFine,
                    insertedReservation.HasReturned));
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateReservation(int id, [FromBody] ReservationUpdateDto reservation)
        {
            if (id != reservation.ReservationId) return BadRequest();
            Reservation? reservationToUpdate = _dbCtx.Reservation.Find(id);
            if (reservationToUpdate == null) return NotFound();
            reservationToUpdate.ReservationId = reservation.ReservationId;
            reservationToUpdate.UserId = reservation.UserId;
            reservationToUpdate.BookId = reservation.BookId;
            reservationToUpdate.ReservationDateTime = reservation.ReservationDateTime;
            reservationToUpdate.CatalogId = reservation.CatalogId;
            reservationToUpdate.ReservationReturnDateTime = reservation.ReservationReturnDateTime;
            reservationToUpdate.HasFine = reservation.HasFine;
            reservationToUpdate.HasReturned = reservation.HasReturned;
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
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
