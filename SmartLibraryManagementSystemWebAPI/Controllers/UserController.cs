using System.Collections.Immutable;
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
                .ThenInclude(r => r.Book)
                .Include(u => u.Reservations)
                .ThenInclude(r => r.Catalog)
                .Include(u => u.Reservations)
                .ThenInclude(r => r.Fine)
                .Include(u => u.Reservations)
                .ThenInclude(r => r.Loan)
                .Include(u => u.PrevLoan)
                .Include(u => u.Student)
                .Include(u => u.Faculty)
                .Include(u => u.PrevFine)
                .FirstOrDefault(u => u.UserId == id);
            if (withReservation)
            {
                List<ReservationUserDto> reservations = [];
                foreach (var reservation in user.Reservations)
                {
                    ReservationUserDto userReservation = new ReservationUserDto(
                            reservation.ReservationId,
                            reservation.UserId,
                            new BookUpdateDto(
                                reservation.Book.BookId,
                                reservation.Book.BookName,
                                reservation.Book.Author,
                                reservation.Book.ReleaseDate,
                                reservation.Book.Synopsis,
                                reservation.Book.BookImgPath),
                            reservation.ReservationDateTime,
                            new CatalogUpdateDto(
                                reservation.Catalog.CatalogId,
                                reservation.Catalog.BookId,
                                reservation.Catalog.Copies,
                                reservation.Catalog.Genre,
                                reservation.Catalog.ClassificationId,
                                reservation.Catalog.CopiesBorrowed),
                            null,
                            null,
                            reservation.ReservationReturnDateTime,
                            reservation.HasFine,
                            reservation.HasReturned,
                            reservation.HasLoan);
                    if (reservation.Fine != null)
                    {
                        userReservation.Fine = new FineUpdateDto(
                            reservation.Fine.FineId,
                            reservation.Fine.FineAmount,
                            reservation.Fine.UserId,
                            reservation.Fine.ReservatonId,
                            reservation.Fine.HasPayed);
                    }
                    if (reservation.Loan != null)
                    {
                        userReservation.Loan = new LoanUpdateDto(
                            reservation.Loan.LoanId,
                            reservation.Loan.LoanAmount,
                            reservation.Loan.UserId,
                            reservation.Loan.ReservatonId,
                            reservation.Loan.HasPayed);
                    }
                    reservations.Add(userReservation);
                }
                UserWithReservationsDto userWithReservationsToRet = new UserWithReservationsDto(
                    user.UserId,
                    user.UserName,
                    null,
                    null,
                    user.HasFine,
                    user.HasLoan,
                    reservations,
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
                        user.Student.UserId,
                        user.Student.ProfileImgPath);
                }
                else
                {
                    userWithReservationsToRet.Faculty = new FacultyUpdateDto(
                        user.Faculty.FacultyId,
                        user.Faculty.FacultyName,
                        user.Faculty.Department,
                        user.Faculty.Subject,
                        user.Faculty.Course,
                        user.Faculty.Email,
                        user.Faculty.Password,
                        user.Faculty.IsLoggedIn,
                        user.Faculty.UserId,
                        user.Faculty.ProfileImgPath);
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
                    user.Student.UserId,
                    user.Student.ProfileImgPath);
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
                    user.Faculty.IsLoggedIn,
                    user.Faculty.UserId,
                    user.Faculty.ProfileImgPath);
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
            userToUpdate.UpdateUser(
                user.UserId,
                user.UserName,
                user.HasFine,
                user.HasLoan,
                user.IsAdmin
            );
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
