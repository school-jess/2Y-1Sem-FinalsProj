using SmartLibraryManagementSystemClassLibrary.Model;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StudentLibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        private readonly DatabaseContext _dbCtx;

        public FacultyController(DatabaseContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        [HttpGet]
        public IActionResult GetFaculties()
        {
            var faculties = _dbCtx.Faculty.Select(f => new FacultyUpdateDto
            {
                Course = f.Course,
                Department = f.Department,
                FacultyId = f.FacultyId,
                FacultyName = f.FacultyName,
                Subject = f.Subject,
                Email = f.Email,
                Password = f.Password,
                IsLoggedIn = f.IsLoggedIn,
                UserId = f.UserId
            }).ToList();
            return Ok(faculties);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetFaculty(int id)
        {
            var faculty = _dbCtx.Faculty.Include(f => f.User).FirstOrDefault(f => f.FacultyId == id);
            if (faculty == null) return NotFound();
            return Ok(new FacultyGet1Dto
            {
                Course = faculty.Course,
                Department = faculty.Department,
                FacultyId = faculty.FacultyId,
                FacultyName = faculty.FacultyName,
                Subject = faculty.Subject,
                Email = faculty.Email,
                Password = faculty.Password,
                User = new UserUpdateDto
                {
                    HasFine = faculty.User.HasFine,
                    HasLoan = faculty.User.HasLoan,
                    UserId = faculty.User.UserId,
                    UserName = faculty.User.UserName,
                    IsAdmin = faculty.User.IsAdmin
                },
                IsLoggedIn = faculty.IsLoggedIn
            });
        }

        [HttpGet("{email:regex(.*@.*)}")]
        public IActionResult GetFacultyEmail(string email)
        {
            var faculty = _dbCtx.Faculty
                .Include(f => f.User)
                .FirstOrDefault(f => f.Email == email);
            if (faculty == null) return NotFound();
            return Ok(new FacultyGet1Dto
            {
                Course = faculty.Course,
                Department = faculty.Department,
                FacultyId = faculty.FacultyId,
                FacultyName = faculty.FacultyName,
                Subject = faculty.Subject,
                Email = faculty.Email,
                Password = faculty.Password,
                User = new UserUpdateDto
                {
                    HasFine = faculty.User.HasFine,
                    HasLoan = faculty.User.HasLoan,
                    UserId = faculty.User.UserId,
                    UserName = faculty.User.UserName,
                    IsAdmin = faculty.User.IsAdmin
                }
            });
        }

        [HttpPost]
        public IActionResult NewFaculty([FromBody] FacultyCreationDto faculty)
        {
            _dbCtx.Faculty.Add(new Faculty
            {
                Course = faculty.Course,
                Department = faculty.Department,
                FacultyName = faculty.FacultyName,
                Subject = faculty.Subject,
                Email = faculty.Email,
                IsLoggedIn = faculty.IsLoggedIn,
                Password = faculty.Password,
                UserId = faculty.UserId
            });
            _dbCtx.SaveChanges();
            var insertedFaculty = _dbCtx.Faculty.First(f => f.Email == faculty.Email);
            return CreatedAtAction(nameof(GetFaculty), new { id = insertedFaculty.FacultyId }, new FacultyUpdateDto
            {
                Course = insertedFaculty.Course,
                Department = insertedFaculty.Department,
                Email = insertedFaculty.Email,
                FacultyId = insertedFaculty.FacultyId,
                FacultyName = insertedFaculty.FacultyName,
                IsLoggedIn = insertedFaculty.IsLoggedIn,
                Password = insertedFaculty.Password,
                Subject = insertedFaculty.Subject,
                UserId = insertedFaculty.UserId,
            });
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateFaculty(int id, [FromBody] FacultyUpdateDto faculty)
        {
            if (id != faculty.FacultyId) return BadRequest();
            Faculty? facultyToUpdate = _dbCtx.Faculty.Find(id);
            if (facultyToUpdate == null) return NotFound();
            facultyToUpdate.Course = faculty.Course;
            facultyToUpdate.Department = faculty.Department;
            facultyToUpdate.FacultyId = faculty.FacultyId;
            facultyToUpdate.Subject = faculty.Subject;
            facultyToUpdate.Email = faculty.Email;
            facultyToUpdate.IsLoggedIn = faculty.IsLoggedIn;
            facultyToUpdate.Password = faculty.Password;
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteFaculty(int id)
        {
            var faculty = _dbCtx.Faculty.Find(id);
            if (faculty == null) return NotFound();
            _dbCtx.Faculty.Remove(faculty);
            _dbCtx.SaveChanges();
            return NoContent();
        }
    }
}
