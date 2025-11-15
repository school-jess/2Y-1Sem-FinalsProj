using SmartLibraryManagementSystemClassLibrary.Model;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StudentLibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _dbCtx;

        public UserController(DatabaseContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _dbCtx.User
                .Select(u => new UserUpdateDto(
                    u.UserId,
                    u.UserName,
                    u.HasFine,
                    u.HasLoan,
                    u.IsAdmin)).ToList();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetUser(int id, [FromQuery] bool withReservation)
        {
            var user = _dbCtx.User
                .Include(u => u.Reservations)
                .Include(u => u.Student)
                .Include(u => u.Faculty)
                .Include(u => u.PrevFine)
                .Include(u => u.PrevLoan)
                .FirstOrDefault(u => u.UserId == id);
            if (user == null) return NotFound();
            if (withReservation)
            {
                UserWithReservationsDto userWithReservationsToRet = new UserWithReservationsDto(
                    user.UserId,
                    user.UserName,
                    null, null,
                    user.HasFine,
                    user.HasLoan,
                    user.Reservations
                        .Select(r => new ReservationUserDto(
                            r.ReservationId,
                            r.UserId,
                            new BookUpdateDto(r.Book.BookId, r.Book.BookName, r.Book.Author, r.Book.ReleaseDate,
                                r.Book.Synopsis),
                            r.ReservationDateTime,
                            new CatalogUpdateDto(
                                r.Catalog.CatalogId,
                                r.Catalog.BookId,
                                r.Catalog.Copies,
                                r.Catalog.Genre,
                                r.Catalog.ClassificationId,
                                r.Catalog.CopiesBorrowed),
                            new FineUpdateDto(
                                r.Fine.FineId,
                                r.Fine.FineAmount,
                                r.Fine.UserId,
                                r.Fine.ReservatonId,
                                r.Fine.HasPayed),
                            new LoanUpdateDto(
                                r.Loan.LoanId,
                                r.Loan.LoanAmount,
                                r.Loan.UserId,
                                r.Loan.ReservatonId,
                                r.Loan.HasPayed),
                            r.ReservationReturnDateTime,
                            r.HasFine,
                            r.HasReturned,
                            r.HasLoan)).ToList(),
                    user.PrevFine
                        .Select(f => new FineUpdateDto(
                            f.FineId,
                            f.FineAmount,
                            f.UserId,
                            f.HasPayed)).ToList(),
                    user.PrevLoan.Select(l => new LoanUpdateDto(
                        l.LoanId,
                        l.LoanAmount,
                        l.UserId,
                        l.ReservatonId,
                        l.HasPayed)).ToList(),
                    user.IsAdmin);
                if (user.Faculty == null)
                {
                    userWithReservationsToRet.Student = new StudentUpdateDto(
                        user.Student.StudentId,
                        user.Student.StudentName,
                        user.Student.Department,
                        user.Student.Course,
                        user.Student.Grade,
                        user.Student.Email,
                        user.Student.Password,
                        user.Student.IsLoggedIn,
                        user.Student.UserId);
                }
                else
                {
                    userWithReservationsToRet.Faculty = new FacultyUpdateDto(user.Faculty.FacultyId,
                        user.Faculty.FacultyName, user.Faculty.Department, user.Faculty.Subject, user.Faculty.Course,
                        user.Faculty.Email, user.Faculty.Password, user.Faculty.IsLoggedIn);
                }

                return Ok(userWithReservationsToRet);
            }

            UserGet1Dto userToRet = new UserGet1Dto(
                user.UserId,
                user.UserName,
                null,
                null,
                user.HasFine,
                user.HasLoan,
                user.Reservations.Select(r => new ReservationUpdateDto(
                    r.ReservationId,
                    r.UserId,
                    r.BookId,
                    r.ReservationDateTime,
                    r.CatalogId,
                    r.ReservationReturnDateTime,
                    r.HasFine,
                    r.HasReturned,
                    r.HasLoan)).ToList(),
                user.PrevFine.Select(f => new FineUpdateDto(
                    f.FineId,
                    f.FineAmount,
                    f.UserId,
                    f.HasPayed)).ToList(),
                user.PrevLoan.Select(l =>
                    new LoanUpdateDto(
                        l.LoanId,
                        l.LoanAmount,
                        l.UserId,
                        l.ReservatonId,
                        l.HasPayed)).ToList(),
                user.IsAdmin);
            if (user.Faculty == null)
            {
                userToRet.Student = new StudentUpdateDto(
                    user.Student.StudentId,
                    user.Student.StudentName,
                    user.Student.Department,
                    user.Student.Course,
                    user.Student.Grade,
                    user.Student.Email,
                    user.Student.Password,
                    user.Student.IsLoggedIn,
                    user.Student.UserId);
            }
            else
            {
                userToRet.Faculty = new FacultyUpdateDto(
                    user.Faculty.FacultyId,
                    user.Faculty.FacultyName,
                    user.Faculty.Department,
                    user.Faculty.Subject,
                    user.Faculty.Course,
                    user.Faculty.Email,
                    user.Faculty.Password,
                    user.Faculty.IsLoggedIn);
            }

            return Ok(userToRet);
        }

        [HttpPost]
        public IActionResult NewUser([FromBody] UserCreationDto user)
        {
            _dbCtx.User.Add(new User(
                user.UserName,
                user.HasFine,
                user.HasLoan,
                user.IsAdmin));
            _dbCtx.SaveChanges();
            var insertedUser = _dbCtx.User.First(u => u.UserName == user.UserName);
            return CreatedAtAction(nameof(GetUser), new { id = insertedUser.UserId, withReservation = false },
                new UserUpdateDto(
                    insertedUser.UserId,
                    insertedUser.UserName,
                    insertedUser.HasFine,
                    insertedUser.HasLoan,
                    insertedUser.IsAdmin));
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdateDto user)
        {
            if (id != user.UserId) return BadRequest();
            User? userToUpdate = _dbCtx.User.Find(id);
            if (userToUpdate == null) return NotFound();
            userToUpdate.UserId = user.UserId;
            userToUpdate.UserName = user.UserName;
            userToUpdate.HasFine = user.HasFine;
            userToUpdate.HasLoan = user.HasLoan;
            userToUpdate.IsAdmin = user.IsAdmin;
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _dbCtx.User.Find(id);
            if (user == null) return NotFound();
            _dbCtx.User.Remove(user);
            _dbCtx.SaveChanges();
            return NoContent();
        }
    }
}
