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
                Subject = f.Subject
            }).ToList();
            return Ok(faculties);
        }

        [HttpGet("{id}")]
        public IActionResult GetFaculty(int id)
        {
            var faculty = _dbCtx.Faculty.Find(id);
            if (faculty == null) return NotFound();
            return Ok(new FacultyGet1Dto
            {
                Course = faculty.Course,
                Department = faculty.Department,
                FacultyId = faculty.FacultyId,
                FacultyName = faculty.FacultyName,
                Subject = faculty.Subject,
                User = new UserUpdateDto
                {
                    FacultyId = faculty.User.FacultyId,
                    HasFine = faculty.User.HasFine,
                    HasLoan = faculty.User.HasLoan,
                    IsFaculty = faculty.User.IsFaculty,
                    StudentId = faculty.User.StudentId,
                    UserId = faculty.User.UserId,
                    UserName = faculty.User.UserName
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
                Subject = faculty.Subject
            });
            _dbCtx.SaveChanges();
            var insertedFaculty = _dbCtx.Faculty.Find(faculty);
            return CreatedAtAction("", new { facultyId = insertedFaculty.FacultyId });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFaculty(int id, [FromBody] FacultyUpdateDto faculty)
        {
            if (id != faculty.FacultyId) return BadRequest();
            Faculty updatedFaculty = new Faculty
            {
                Course = faculty.Course,
                Department = faculty.Department,
                FacultyId = faculty.FacultyId,
                FacultyName = faculty.FacultyName,
                Subject = faculty.Subject
            };
            _dbCtx.Entry(updatedFaculty).State = EntityState.Modified;
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
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
