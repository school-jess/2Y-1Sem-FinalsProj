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
                select new ReservationUpdateDto
                {
                    BookId = reservation.BookId,
                    CatalogId = reservation.CatalogId,
                    ReservationId = reservation.ReservationId,
                    ReservationTime = reservation.ReservationTime,
                    UserId = reservation.UserId,
                    ReservationDate = reservation.ReservationDate,
                }).ToList();
            return Ok(reservations);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetReservation(int id)
        {
            var reservation = _dbCtx.Reservation.Find(id);
            if (reservation == null) return NotFound();
            return Ok(new ReservationGet1Dto
            {
                Book = new BookUpdateDto
                {
                    Author = reservation.Book.Author,
                    BookId = reservation.Book.BookId,
                    BookName = reservation.Book.BookName,
                },
                Catalog = new CatalogUpdateDto
                {
                    BookId = reservation.Catalog.BookId,
                    CatalogId = reservation.Catalog.CatalogId,
                    Copies = reservation.Catalog.Copies,
                    ClassificationId = reservation.Catalog.ClassificationId,
                    Genre = reservation.Catalog.Genre,
                },
                Fine = new FineUpdateDto
                {
                    AmtPayedSinceLastFine = reservation.Fine.AmtPayedSinceLastFine,
                    FineAmount = reservation.Fine.AmtPayedSinceLastFine,
                    FineId = reservation.Fine.FineId,
                    UserId = reservation.Fine.UserId
                },
                Loan = new LoanUpdateDto
                {
                    AmtLoanedSinceLastLoaned = reservation.Loan.AmtLoanedSinceLastLoaned,
                    LoanAmount = reservation.Loan.LoanAmount,
                    LoanId = reservation.Loan.LoanId,
                    UserId = reservation.Loan.UserId
                },
                ReservationId = reservation.ReservationId,
                ReservationTime = reservation.ReservationTime,
                User = new UserUpdateDto
                {
                    FacultyId = reservation.User.FacultyId,
                    HasFine = reservation.User.HasFine,
                    HasLoan = reservation.User.HasLoan,
                    IsFaculty = reservation.User.IsFaculty,
                    StudentId = reservation.User.StudentId,
                    UserId = reservation.User.UserId,
                    UserName = reservation.User.UserName
                }
            });
        }

        [HttpPost]
        public IActionResult NewReservation([FromBody] ReservationCreationDto reservation)
        {
            _dbCtx.Reservation.Add(new Reservation
            {
                BookId = reservation.BookId,
                CatalogId = reservation.CatalogId,
                ReservationTime = reservation.ReservationTime,
                UserId = reservation.UserId
            });
            _dbCtx.SaveChanges();
            var insertedReservation = (from r in _dbCtx.Reservation
                                      where r.BookId == reservation.BookId
                                      select r).First();
            return CreatedAtAction(nameof(GetReservation), new { id = insertedReservation.ReservationId }, insertedReservation);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateReservation(int id, [FromBody] ReservationUpdateDto reservation)
        {
            if (id != reservation.ReservationId) return BadRequest();
            Reservation updatedReservation = new Reservation
            {
                BookId = reservation.BookId,
                CatalogId = reservation.CatalogId,
                ReservationId = reservation.ReservationId,
                ReservationTime = reservation.ReservationTime,
                UserId = reservation.UserId
            };
            _dbCtx.Entry(updatedReservation).State = EntityState.Modified;
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
