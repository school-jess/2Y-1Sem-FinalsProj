using SmartLibraryManagementSystemClassLibrary.Model;
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
            var students = _dbCtx.Student.ToList();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public IActionResult GetReservation(int id)
        {
            var reservation = _dbCtx.Reservation.Find(id);
            if (reservation == null) return NotFound();
            return Ok(reservation);
        }

        [HttpPost]
        public IActionResult NewStudent([FromBody] Student student)
        {
            _dbCtx.Student.Add(student);
            _dbCtx.SaveChanges();
            var insertedStudent = _dbCtx.Student.Find(student);
            return CreatedAtAction("", new { studentId = insertedStudent.StudentId });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] Student student)
        {
            if (id != student.StudentId) return BadRequest();
            _dbCtx.Entry(student).State = EntityState.Modified;
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
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
