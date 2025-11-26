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
            var students = _dbCtx.Student
                .Select(s => new StudentUpdateDto(
                    s.StudentId,
                    s.StudentName,
                    s.Department,
                    s.Course,
                    s.Grade,
                    s.Email,
                    s.Password,
                    s.IsLoggedIn,
                    s.UserId,
                    s.ProfileImgPath)).ToList();
            return Ok(students);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetStudent(int id)
        {
            var student = _dbCtx.Student
                .Include(s => s.User)
                .FirstOrDefault(s => s.StudentId == id);
            if (student == null) return NotFound();
            return Ok(new StudentGet1Dto(
                student.StudentId,
                student.StudentName,
                student.Department,
                student.Course,
                student.Grade,
                new UserUpdateDto(
                    student.User.UserId,
                    student.User.UserName,
                    student.User.HasFine,
                    student.User.HasLoan,
                    student.User.IsAdmin),
                student.Email,
                student.Password,
                student.IsLoggedIn,
                student.ProfileImgPath));
        }

        [HttpGet("{email:regex(.*@.*)}")]
        public IActionResult GetStudentEmail(string email)
        {
            var student = _dbCtx.Student
                .Include(s => s.User)
                .FirstOrDefault(s => s.Email == email);
            if (student == null) return NotFound();
            return Ok(new StudentGet1Dto(
                student.StudentId,
                student.StudentName,
                student.Department,
                student.Course,
                student.Grade,
                new UserUpdateDto(
                    student.User.UserId,
                    student.User.UserName,
                    student.User.HasFine,
                    student.User.HasLoan,
                    student.User.IsAdmin),
                student.Email,
                student.Password,
                student.IsLoggedIn,
                student.ProfileImgPath));
        }

        [HttpPost]
        public IActionResult NewStudent([FromBody] StudentCreationDto student)
        {
            _dbCtx.Student.Add(new Student(
                student.StudentName,
                student.Department,
                student.Course,
                student.Grade,
                student.Email,
                student.IsLoggedIn,
                student.Password,
                student.UserId,
                student.ProfileImgPath
                ));
            _dbCtx.SaveChanges();
            var insertedStudent = _dbCtx.Student.First(s => s.Email == student.Email);
            return CreatedAtAction(nameof(GetStudent), new { id = insertedStudent.StudentId },
                new StudentUpdateDto(
                    insertedStudent.StudentId,
                    insertedStudent.StudentName,
                    insertedStudent.Department,
                    insertedStudent.Course,
                    insertedStudent.Grade,
                    insertedStudent.Email,
                    insertedStudent.Password,
                    insertedStudent.IsLoggedIn,
                    insertedStudent.UserId,
                    insertedStudent.ProfileImgPath));
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateStudent(int id, [FromBody] StudentUpdateDto student)
        {
            if (id != student.StudentId) return BadRequest();
            Student? studentToUpdate = _dbCtx.Student.Find(id);
            if (studentToUpdate == null) return NotFound();
            studentToUpdate.UpdateStudent(
                student.StudentId,
                student.StudentName,
                student.Department,
                student.Course,
                student.Grade,
                student.Email,
                student.IsLoggedIn,
                student.Password,
                student.UserId,
                student.ProfileImgPath);
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
