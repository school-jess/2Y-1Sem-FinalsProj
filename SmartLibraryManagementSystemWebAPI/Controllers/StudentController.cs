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
                StudentName = s.StudentName,
                Email = s.Email,
                IsLoggedIn = s.IsLoggedIn,
                Password = s.Password,
                UserId = s.UserId
            }).ToList();
            return Ok(students);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetStudent(int id)
        {
            var student = _dbCtx.Student.Include(s => s.User).FirstOrDefault(s => s.StudentId == id);
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
                    HasFine = student.User.HasFine,
                    HasLoan = student.User.HasLoan,
                    UserId = student.User.UserId,
                    UserName = student.User.UserName,
                    IsAdmin = student.User.IsAdmin,
                }
            });
        }

        [HttpGet("{email:regex(.*@.*)}")]
        public IActionResult GetStudentEmail(string email)
        {
            var student = _dbCtx.Student
                .Include(s => s.User)
                .FirstOrDefault(s => s.Email == email);
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
                    HasFine = student.User.HasFine,
                    HasLoan = student.User.HasLoan,
                    IsAdmin = student.User.IsAdmin,
                    UserId = student.User.UserId,
                    UserName = student.User.UserName,
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
                Password = student.Password,
                UserId = student.UserId
            });
            Console.WriteLine(student.UserId);
            _dbCtx.SaveChanges();
            var insertedStudent = _dbCtx.Student.First(s => s.Email == student.Email);
            return CreatedAtAction(nameof(GetStudent), new { id = insertedStudent.StudentId }, new StudentUpdateDto
            {
                Course = insertedStudent.Course,
                Department = insertedStudent.Department,
                Email = insertedStudent.Email,
                Grade = insertedStudent.Grade,
                IsLoggedIn = insertedStudent.IsLoggedIn,
                Password = insertedStudent.Password,
                StudentId = insertedStudent.StudentId,
                StudentName = insertedStudent.StudentName,
                UserId = insertedStudent.UserId,
            });
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateStudent(int id, [FromBody] StudentUpdateDto student)
        {
            if (id != student.StudentId) return BadRequest();
            Student? studentToUpdate = _dbCtx.Student.Find(id);
            if (studentToUpdate == null) return NotFound();
            studentToUpdate.Course = student.Course;
            studentToUpdate.Department = student.Department;
            studentToUpdate.Grade = student.Grade;
            studentToUpdate.StudentId = student.StudentId;
            studentToUpdate.StudentName = student.StudentName;
            studentToUpdate.Email = student.Email;
            studentToUpdate.IsLoggedIn = student.IsLoggedIn;
            studentToUpdate.Password = student.Password;
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
