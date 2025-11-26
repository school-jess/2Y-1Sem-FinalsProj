using SmartLibraryManagementSystemClassLibrary.Model;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StudentLibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionDataController : ControllerBase
    {
        private readonly DatabaseContext _dbCtx;

        public SessionDataController(DatabaseContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        [HttpGet]
        public IActionResult GetSessionDatas()
        {
            var sessionDatas = _dbCtx.SessionData
                .Select(s => new SessionDataUpdateDto(
                    s.Key,
                    s.Value,
                    s.Expiry)).ToList();
            return Ok(sessionDatas);
        }

        [HttpGet("{key}")]
        public IActionResult GetSessionData(string key)
        {
            var sessionData = _dbCtx.SessionData.FirstOrDefault(s => s.Key == key);
            if (sessionData == null) return NotFound();
            return Ok(new SessionDataGet1Dto(
                sessionData.Key,
                sessionData.Value,
                sessionData.Expiry));
        }

        [HttpPost]
        public IActionResult NewSessionData([FromBody] SessionDataCreationDto sessionData)
        {
            _dbCtx.SessionData.Add(new SessionData(
                sessionData.Key,
                sessionData.Value,
                sessionData.Expiry));
            _dbCtx.SaveChanges();
            var insertedSessionData = _dbCtx.SessionData.First(s => s.Key == sessionData.Key);
            return CreatedAtAction(nameof(GetSessionData), new { id = insertedSessionData.Key },
                new SessionDataUpdateDto(
                    insertedSessionData.Key,
                    insertedSessionData.Value,
                    insertedSessionData.Expiry));
        }

        [HttpPut("{key}")]
        public IActionResult UpdateSessionData(string key, [FromBody] SessionDataUpdateDto sessionData)
        {
            if (key != sessionData.Key) return BadRequest();
            SessionData? sessionDataToUpdate = _dbCtx.SessionData.Find(key);
            if (sessionDataToUpdate == null) return NotFound();
            sessionDataToUpdate.UpdateSessionData(
                sessionData.Key,
                sessionData.Value,
                sessionData.Expiry);
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteSessionData(int id)
        {
            var sessionData = _dbCtx.SessionData.Find(id);
            if (sessionData == null) return NotFound();
            _dbCtx.SessionData.Remove(sessionData);
            _dbCtx.SaveChanges();
            return NoContent();
        }
    }
}
