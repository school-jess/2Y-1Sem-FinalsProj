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
                UserName = u.UserName
            }).ToList();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _dbCtx.User.Find(id);
            if (user == null) return NotFound();
            return Ok(new UserGet1Dto
            {
                Faculty = new FacultyUpdateDto
                {
                    Course = user.Faculty.Course,
                    Department = user.Faculty.Department,
                    FacultyId = user.Faculty.FacultyId,
                    FacultyName = user.Faculty.FacultyName,
                    Subject = user.Faculty.Subject,
                },
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
                Student = new StudentUpdateDto
                {
                    Course = user.Student.Course,
                    Department = user.Student.Department,
                    Grade = user.Student.Grade,
                    StudentId = user.Student.StudentId,
                    StudentName = user.Student.StudentName
                },
                UserId = user.UserId,
                UserName = user.UserName
            });
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
                UserName = user.UserName
            });
            _dbCtx.SaveChanges();
            var insertedUser = _dbCtx.User.Find(user);
            return CreatedAtAction("", new { userId = insertedUser.UserId });
        }

        [HttpPut("{id}")]
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
                UserName = user.UserName
            };
            _dbCtx.Entry(updatedUser).State = EntityState.Modified;
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
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
