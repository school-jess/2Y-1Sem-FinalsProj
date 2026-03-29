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
            var loans = _dbCtx.Loan
                .Select(l => new LoanUpdateDto(
                    l.LoanId,
                    l.LoanAmount,
                    l.UserId,
                    l.ReservatonId,
                    l.HasPayed)).ToList();
            return Ok(loans);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetLoan(int id)
        {
            var loan = _dbCtx.Loan.Include(l => l.Reservaton).FirstOrDefault(l => l.LoanId == id);
            if (loan == null) return NotFound();
            return Ok(new LoanGet1Dto(
                loan.LoanId,
                loan.LoanAmount,
                new UserUpdateDto(
                    loan.User.UserId,
                    loan.User.UserName,
                    loan.User.HasFine,
                    loan.User.HasLoan,
                    loan.User.IsAdmin),
                new ReservationUpdateDto(
                    loan.Reservaton.ReservationId,
                    loan.Reservaton.UserId,
                    loan.Reservaton.BookId,
                    loan.Reservaton.ReservationDateTime,
                    loan.Reservaton.CatalogId,
                    loan.Reservaton.ReservationReturnDateTime,
                    loan.Reservaton.HasFine,
                    loan.Reservaton.HasReturned,
                    loan.Reservaton.HasLoan),
                loan.HasPayed));
        }

        [HttpPost]
        public IActionResult NewLoan([FromBody] LoanCreationDto loan)
        {
            _dbCtx.Loan.Add(new Loan(
                loan.LoanAmount,
                loan.UserId,
                loan.ReservationId,
                loan.HasPayed));
            _dbCtx.SaveChanges();
            var insertedLoan = _dbCtx.Loan.First(l => l.UserId == loan.UserId);
            return CreatedAtAction(nameof(GetLoan), new { id = insertedLoan.LoanId },
                new LoanUpdateDto(
                    insertedLoan.LoanId,
                    insertedLoan.LoanAmount,
                    insertedLoan.UserId,
                    insertedLoan.ReservatonId,
                    insertedLoan.HasPayed));
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateLoan(int id, [FromBody] LoanUpdateDto loan)
        {
            if (id != loan.LoanId) return BadRequest();
            Loan? loanToUpdate = _dbCtx.Loan.Find(id);
            if (loanToUpdate == null) return NotFound();
            loanToUpdate.UpdateLoan(
                loan.LoanId,
                loan.LoanAmount,
                loan.UserId,
                loan.ReservationId,
                loan.HasPayed);
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
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
