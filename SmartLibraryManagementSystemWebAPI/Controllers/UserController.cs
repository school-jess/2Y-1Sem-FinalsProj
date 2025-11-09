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
                FacultyId = u.FacultyId,
                HasFine = u.HasFine,
                HasLoan = u.HasLoan,
                IsFaculty = u.IsFaculty,
                StudentId = u.StudentId,
                UserId = u.UserId,
                UserName = u.UserName,
                IsAdmin = u.IsAdmin,
            }).ToList();
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetUser(int id)
        {
            var user = (from u in _dbCtx.User.Include(u => u.Reservations).Include(u => u.Student).Include(u => u.Faculty)
                       where u.UserId == id
                       select u).First();
            if (user == null) return NotFound();
            UserGet1Dto userToRet = new UserGet1Dto
            {
                HasFine = user.HasFine,
                HasLoan = user.HasLoan,
                IsFaculty = user.IsFaculty,
                Reservations = user.Reservations.Select(r => new ReservationUpdateDto
                {
                    BookId = r.BookId,
                    CatalogId = r.CatalogId,
                    ReservationId = r.ReservationId,
                    ReservationTime = r.ReservationTime,
                    UserId = r.UserId,
                }).ToList(),
                UserId = user.UserId,
                UserName = user.UserName,
                IsAdmin = user.IsAdmin
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
            } else
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

        [HttpPost]
        public IActionResult NewUser([FromBody] UserCreationDto user)
        {
            _dbCtx.User.Add(new User
            {
                FacultyId = user.FacultyId,
                HasFine = user.HasFine,
                HasLoan = user.HasLoan,
                IsFaculty = user.IsFaculty,
                StudentId = user.StudentId,
                UserName = user.UserName,
                IsAdmin = user.IsAdmin,
            });
            _dbCtx.SaveChanges();
            var insertedUser = (from u in _dbCtx.User
                               where u.UserName == user.UserName
                               select u).First();
            return CreatedAtAction(nameof(GetUser), new { id = insertedUser.UserId }, insertedUser);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateUser(int id, [FromBody] UserUpdateDto user)
        {
            if (id != user.StudentId) return BadRequest();
            User updatedUser = new User
            {
                FacultyId = user.FacultyId,
                HasFine = user.HasFine,
                HasLoan = user.HasLoan,
                IsFaculty = user.IsFaculty,
                StudentId = user.StudentId,
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
