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
            var loanes = _dbCtx.Loan.ToList();
            return Ok(loanes);
        }

        [HttpGet("{id}")]
        public IActionResult GetLoan(int id)
        {
            var loan = _dbCtx.Loan.Find(id);
            if (loan == null) return NotFound();
            return Ok(loan);
        }

        [HttpPost]
        public IActionResult NewLoan([FromBody] LoanCreationDto loan)
        {
            _dbCtx.Loan.Add(new Loan { AmtLoanedSinceLastLoaned = loan.AmtLoanedSinceLastLoaned, LoanAmount = loan.LoanAmount, UserId = loan.UserId });
            _dbCtx.SaveChanges();
            var insertedLoan = _dbCtx.Loan.Find(loan);
            return CreatedAtAction("", new { loanId = insertedLoan.LoanId });
        }

        [HttpPut("{id}")]
        public IActionResult UpdatLoan(int id, [FromBody] Loan loan)
        {
            if (id != loan.LoanId) return BadRequest();
            _dbCtx.Entry(loan).State = EntityState.Modified;
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
