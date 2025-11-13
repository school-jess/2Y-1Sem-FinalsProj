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
            var faculties = _dbCtx.Faculty.Select(f => new FacultyUpdateDto(f.FacultyId, f.FacultyName, f.Department,
                f.Subject, f.Course, f.Email, f.Password, f.IsLoggedIn, f.UserId)).ToList();
            return Ok(faculties);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetFaculty(int id)
        {
            var faculty = _dbCtx.Faculty.Include(f => f.User).FirstOrDefault(f => f.FacultyId == id);
            if (faculty == null) return NotFound();
            return Ok(new FacultyGet1Dto(faculty.FacultyId, faculty.FacultyName, faculty.Department, faculty.Subject,
                faculty.Course, new UserUpdateDto(faculty.User.UserId, faculty.User.UserName, faculty.User.HasFine,
                    faculty.User.HasLoan, faculty.User.IsAdmin), faculty.Email, faculty.Password, faculty.IsLoggedIn));
        }

        [HttpGet("{email:regex(.*@.*)}")]
        public IActionResult GetFacultyEmail(string email)
        {
            var faculty = _dbCtx.Faculty
                .Include(f => f.User)
                .FirstOrDefault(f => f.Email == email);
            if (faculty == null) return NotFound();
            return Ok(new FacultyGet1Dto(faculty.FacultyId, faculty.FacultyName, faculty.Department, faculty.Subject,
                faculty.Course,
                new UserUpdateDto(faculty.User.UserId, faculty.User.UserName, faculty.User.HasFine,
                    faculty.User.HasLoan, faculty.User.IsAdmin), faculty.Email, faculty.Password,
                faculty.IsLoggedIn));
        }

        [HttpPost]
        public IActionResult NewFaculty([FromBody] FacultyCreationDto faculty)
        {
            _dbCtx.Faculty.Add(new Faculty(faculty.FacultyName, faculty.Department, faculty.Subject, faculty.Course,
                faculty.Email, faculty.IsLoggedIn, faculty.Password, faculty.UserId));
            _dbCtx.SaveChanges();
            var insertedFaculty = _dbCtx.Faculty.First(f => f.Email == faculty.Email);
            return CreatedAtAction(nameof(GetFaculty), new { id = insertedFaculty.FacultyId },
                new FacultyUpdateDto(insertedFaculty.FacultyId, insertedFaculty.FacultyName, insertedFaculty.Department,
                    insertedFaculty.Subject, insertedFaculty.Course, insertedFaculty.Email, insertedFaculty.Password,
                    insertedFaculty.IsLoggedIn, insertedFaculty.UserId));
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
