using SmartLibraryManagementSystemClassLibrary.Model;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StudentLibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly DatabaseContext _dbCtx;

        public StudentController(DatabaseContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        [HttpGet]
        public IActionResult GetStudents()
        {
            var students = _dbCtx.Student.Select(s => new StudentUpdateDto
            {
                Course = s.Course,
                Department = s.Department,
                Grade = s.Grade,
                StudentId = s.StudentId,
                StudentName = s.StudentName
            }).ToList();
            return Ok(students);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetStudent(int id)
        {
            var student = (from s in _dbCtx.Student.Include(s => s.User)
                where s.StudentId == id
                select s).First();
            if (student == null) return NotFound();
            return Ok(new StudentGet1Dto
            {
                Course = student.Course,
                Department = student.Department,
                Grade = student.Grade,
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                Email = student.Email,
                IsLoggedIn = student.IsLoggedIn,
                Password = student.Password,
                User = new UserUpdateDto
                {
                    FacultyId = student.User.FacultyId,
                    HasFine = student.User.HasFine,
                    HasLoan = student.User.HasLoan,
                    IsFaculty = student.User.IsFaculty,
                    StudentId = student.User.StudentId,
                    UserId = student.User.UserId,
                    UserName = student.User.UserName,
                    IsAdmin = student.User.IsAdmin,
                }
            });
        }

        [HttpGet("{email:regex(.*@.*)}")]
        public IActionResult GetStudentEmail(string email)
        {
            var student = (from s in _dbCtx.Student.Include(s => s.User)
                where s.Email == email
                select s).First();
            if (student == null) return NotFound();
            return Ok(new StudentGet1Dto
            {
                Course = student.Course,
                Department = student.Department,
                Grade = student.Grade,
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                Email = student.Email,
                IsLoggedIn = student.IsLoggedIn,
                Password = student.Password,
                User = new UserUpdateDto
                {
                    FacultyId = student.User.FacultyId,
                    HasFine = student.User.HasFine,
                    HasLoan = student.User.HasLoan,
                    IsFaculty = student.User.IsFaculty,
                    StudentId = student.User.StudentId,
                    UserId = student.User.UserId,
                    UserName = student.User.UserName,
                    IsAdmin = student.User.IsAdmin,
                }
            });
        }

        [HttpPost]
        public IActionResult NewStudent([FromBody] StudentCreationDto student)
        {
            _dbCtx.Student.Add(new Student
            {
                Course = student.Course,
                Department = student.Department,
                Grade = student.Grade,
                StudentName = student.StudentName,
                Email = student.Email,
                IsLoggedIn = student.IsLoggedIn,
                Password = student.Password
            });
            _dbCtx.SaveChanges();
            var insertedStudent = (from s in _dbCtx.Student
                where s.StudentName == student.StudentName
                select s).First();
            return CreatedAtAction(nameof(GetStudent), new { id = insertedStudent.StudentId }, insertedStudent);
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateStudent(int id, [FromBody] StudentUpdateDto student)
        {
            if (id != student.StudentId) return BadRequest();
            Student updatedStudent = new Student
            {
                Course = student.Course,
                Department = student.Department,
                Grade = student.Grade,
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                Email = student.Email,
                IsLoggedIn = student.IsLoggedIn,
                Password = student.Password,
            };
            _dbCtx.Entry(updatedStudent).State = EntityState.Modified;
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _dbCtx.Student.Find(id);
            if (student == null) return NotFound();
            _dbCtx.Student.Remove(student);
            _dbCtx.SaveChanges();
            return NoContent();
        }
    }
}
