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
            var users = _dbCtx.User.Select(u => new UserUpdateDto
            {
                HasFine = u.HasFine,
                HasLoan = u.HasLoan,
                UserId = u.UserId,
                UserName = u.UserName,
                IsAdmin = u.IsAdmin,
            }).ToList();
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
                UserWithReservationsDto userWithReservationsToRet = new UserWithReservationsDto
                {
                    HasFine = user.HasFine,
                    HasLoan = user.HasLoan,
                    Reservations = user.Reservations.Select(r => new ReservationUserDto
                    {
                        Book = new BookUpdateDto
                        {
                            Author = r.Book.Author,
                            BookId = r.Book.BookId,
                            BookName = r.Book.BookName,
                            ReleaseDate = r.Book.ReleaseDate,
                            Synopsis = r.Book.Synopsis
                        },
                        Catalog = new CatalogUpdateDto
                        {
                            BookId = r.Catalog.BookId,
                            CatalogId = r.Catalog.CatalogId,
                            ClassificationId = r.Catalog.ClassificationId,
                            Copies = r.Catalog.Copies,
                            Genre = r.Catalog.Genre
                        },
                        Fine = new FineUpdateDto
                        {
                            AmtPayedSinceLastFine = r.Fine.AmtPayedSinceLastFine,
                            FineAmount = r.Fine.FineAmount,
                            FineId = r.Fine.FineId,
                            UserId = r.Fine.UserId,
                        },
                        Loan = new LoanUpdateDto
                        {
                            AmtLoanedSinceLastLoaned = r.Loan.AmtLoanedSinceLastLoaned,
                            LoanAmount = r.Loan.LoanAmount,
                            LoanId = r.Loan.LoanId,
                            UserId = r.Loan.UserId
                        },
                        ReservationId = r.ReservationId,
                        ReservationDateTime = r.ReservationDateTime,
                        UserId = r.UserId,
                    }).ToList(),
                    UserId = user.UserId,
                    UserName = user.UserName,
                    IsAdmin = user.IsAdmin,
                    Fines = user.PrevFine.Select(f => new FineUpdateDto
                    {
                        AmtPayedSinceLastFine = f.AmtPayedSinceLastFine,
                        FineAmount = f.FineAmount,
                        FineId = f.FineId,
                        UserId = f.UserId
                    }).ToList(),
                    Loans = user.PrevLoan.Select(l => new LoanUpdateDto
                    {
                        AmtLoanedSinceLastLoaned = l.AmtLoanedSinceLastLoaned,
                        LoanAmount = l.LoanAmount,
                        LoanId = l.LoanId,
                        UserId = l.UserId
                    }).ToList()
                };
                if (user.Faculty == null)
                {
                    userWithReservationsToRet.Student = new StudentUpdateDto
                    {
                        Course = user.Student.Course,
                        Department = user.Student.Department,
                        Grade = user.Student.Grade,
                        StudentId = user.Student.StudentId,
                        StudentName = user.Student.StudentName,
                        Email = user.Student.Email,
                        IsLoggedIn = user.Student.IsLoggedIn,
                        Password = user.Student.Password
                    };
                }
                else
                {
                    userWithReservationsToRet.Faculty = new FacultyUpdateDto
                    {
                        Course = user.Faculty.Course,
                        Department = user.Faculty.Department,
                        FacultyId = user.Faculty.FacultyId,
                        FacultyName = user.Faculty.FacultyName,
                        Subject = user.Faculty.Subject,
                        Email = user.Faculty.Email,
                        IsLoggedIn = user.Faculty.IsLoggedIn,
                        Password = user.Faculty.Password
                    };
                }

                return Ok(userWithReservationsToRet);
            }
            else
            {
                UserGet1Dto userToRet = new UserGet1Dto
                {
                    HasFine = user.HasFine,
                    HasLoan = user.HasLoan,
                    Reservations = user.Reservations.Select(r => new ReservationUpdateDto
                    {
                        BookId = r.BookId,
                        CatalogId = r.CatalogId,
                        ReservationId = r.ReservationId,
                        ReservationDateTime = r.ReservationDateTime,
                        UserId = r.UserId,
                    }).ToList(),
                    UserId = user.UserId,
                    UserName = user.UserName,
                    IsAdmin = user.IsAdmin,
                    Fines = user.PrevFine.Select(f => new FineUpdateDto
                    {
                        AmtPayedSinceLastFine = f.AmtPayedSinceLastFine,
                        FineAmount = f.FineAmount,
                        FineId = f.FineId,
                        UserId = f.UserId
                    }).ToList(),
                    Loans = user.PrevLoan.Select(l => new LoanUpdateDto
                    {
                        AmtLoanedSinceLastLoaned = l.AmtLoanedSinceLastLoaned,
                        LoanAmount = l.LoanAmount,
                        LoanId = l.LoanId,
                        UserId = l.UserId
                    }).ToList()
                };
                if (user.Faculty == null)
                {
                    userToRet.Student = new StudentUpdateDto
                    {
                        Course = user.Student.Course,
                        Department = user.Student.Department,
                        Grade = user.Student.Grade,
                        StudentId = user.Student.StudentId,
                        StudentName = user.Student.StudentName,
                        Email = user.Student.Email,
                        IsLoggedIn = user.Student.IsLoggedIn,
                        Password = user.Student.Password
                    };
                }
                else
                {
                    userToRet.Faculty = new FacultyUpdateDto
                    {
                        Course = user.Faculty.Course,
                        Department = user.Faculty.Department,
                        FacultyId = user.Faculty.FacultyId,
                        FacultyName = user.Faculty.FacultyName,
                        Subject = user.Faculty.Subject,
                        Email = user.Faculty.Email,
                        IsLoggedIn = user.Faculty.IsLoggedIn,
                        Password = user.Faculty.Password
                    };
                }

                return Ok(userToRet);
            }
        }

        [HttpPost]
        public IActionResult NewUser([FromBody] UserCreationDto user)
        {
            _dbCtx.User.Add(new User
            {
                HasFine = user.HasFine,
                HasLoan = user.HasLoan,
                UserName = user.UserName,
                IsAdmin = user.IsAdmin,
            });
            _dbCtx.SaveChanges();
            var insertedUser = _dbCtx.User.First(u => u.UserName == user.UserName);
            return CreatedAtAction(nameof(GetUser), new { id = insertedUser.UserId, withReservation = false },
                new UserUpdateDto
                {
                    UserId = insertedUser.UserId,
                    HasFine = insertedUser.HasFine,
                    HasLoan = insertedUser.HasLoan,
                    IsAdmin = insertedUser.IsAdmin,
                    UserName = insertedUser.UserName
                });
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdateDto user)
        {
            if (id != user.UserId) return BadRequest();
            User updatedUser = new User
            {
                HasFine = user.HasFine,
                HasLoan = user.HasLoan,
                UserId = user.UserId,
                UserName = user.UserName,
                IsAdmin = user.IsAdmin
            };
            _dbCtx.Entry(updatedUser).State = EntityState.Modified;
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
