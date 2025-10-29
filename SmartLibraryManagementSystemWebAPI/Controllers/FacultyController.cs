using SmartLibraryManagementSystemClassLibrary.Model;
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
            var faculties = _dbCtx.Faculty.ToList();
            return Ok(faculties);
        }

        [HttpGet("{id}")]
        public IActionResult GetFaculty(int id)
        {
            var faculty = _dbCtx.Faculty.Find(id);
            if (faculty == null) return NotFound();
            return Ok(faculty);
        }

        [HttpPost]
        public IActionResult NewFaculty([FromBody] Faculty faculty)
        {
            _dbCtx.Faculty.Add(faculty);
            _dbCtx.SaveChanges();
            var insertedFaculty = _dbCtx.Faculty.Find(faculty);
            return CreatedAtAction("", new { facultyId = insertedFaculty.FacultyId });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFaculty(int id, [FromBody] Faculty faculty)
        {
            if (id != faculty.FacultyId) return BadRequest();
            _dbCtx.Entry(faculty).State = EntityState.Modified;
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
