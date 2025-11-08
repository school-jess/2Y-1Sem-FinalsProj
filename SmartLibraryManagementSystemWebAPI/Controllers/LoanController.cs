using SmartLibraryManagementSystemClassLibrary.Model;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StudentLibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly DatabaseContext _dbCtx;

        public LoanController(DatabaseContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        [HttpGet]
        public IActionResult GetLoans()
        {
            var loans = _dbCtx.Loan.Select(l => new LoanUpdateDto
            {
                AmtLoanedSinceLastLoaned = l.AmtLoanedSinceLastLoaned,
                LoanAmount = l.LoanAmount,
                LoanId = l.LoanId,
                UserId = l.UserId
            }).ToList();
            return Ok(loans);
        }

        [HttpGet("{id}")]
        public IActionResult GetLoan(int id)
        {
            var loan = _dbCtx.Loan.Find(id);
            if (loan == null) return NotFound();
            return Ok(new LoanGet1Dto
            {
                AmtLoanedSinceLastLoaned = loan.AmtLoanedSinceLastLoaned,
                LoanAmount = loan.LoanAmount,
                LoanId = loan.LoanId,
                Reservation = new ReservationUpdateDto
                {
                    BookId = loan.Reservaton.BookId,
                    CatalogId = loan.Reservaton.CatalogId,
                    ReservationId = loan.Reservaton.ReservationId,
                    ReservationTime = loan.Reservaton.ReservationTime,
                    UserId = loan.Reservaton.UserId
                }
            });
        }

        [HttpPost]
        public IActionResult NewLoan([FromBody] LoanCreationDto loan)
        {
            _dbCtx.Loan.Add(new Loan
            {
                AmtLoanedSinceLastLoaned = loan.AmtLoanedSinceLastLoaned,
                LoanAmount = loan.LoanAmount,
                UserId = loan.UserId
            });
            _dbCtx.SaveChanges();
            var insertedLoan = (from l in _dbCtx.Loan
                               where l.UserId == loan.UserId
                               select l).First();
            return CreatedAtAction("", new { loanId = insertedLoan.LoanId });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateLoan(int id, [FromBody] LoanUpdateDto loan)
        {
            if (id != loan.LoanId) return BadRequest();
            Loan updateLoan = new Loan
            {
                AmtLoanedSinceLastLoaned = loan.AmtLoanedSinceLastLoaned,
                LoanAmount = loan.LoanAmount,
                LoanId = loan.LoanId,
                UserId = loan.UserId
            };
            _dbCtx.Entry(updateLoan).State = EntityState.Modified;
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletLoan(int id)
        {
            var loan = _dbCtx.Loan.Find(id);
            if (loan == null) return NotFound();
            _dbCtx.Loan.Remove(loan);
            _dbCtx.SaveChanges();
            return NoContent();
        }
    }
}
