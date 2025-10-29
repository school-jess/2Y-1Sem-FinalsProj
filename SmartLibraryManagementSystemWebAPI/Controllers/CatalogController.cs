using SmartLibraryManagementSystemClassLibrary.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StudentLibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly DatabaseContext _dbCtx;

        public CatalogController(DatabaseContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        [HttpGet]
        public IActionResult GetCatalogs()
        {
            var catalogs = _dbCtx.Catalog.ToList();
            return Ok(catalogs);
        }

        [HttpGet("{id}")]
        public IActionResult GetCatalog(int id)
        {
            var catalog = _dbCtx.Catalog.Find(id);
            if (catalog == null) return NotFound();
            return Ok(catalog);
        }

        [HttpPost]
        public IActionResult NewCatalog([FromBody] Catalog catalog)
        {
            _dbCtx.Catalog.Add(catalog);
            _dbCtx.SaveChanges();
            var insertedCatalog = _dbCtx.Catalog.Find(catalog);
            return CreatedAtAction("", new { catalogId = insertedCatalog.CatalogId });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCatalog(int id, [FromBody] Catalog catalog)
        {
            if (id != catalog.CatalogId) return BadRequest();
            _dbCtx.Entry(catalog).State = EntityState.Modified;
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCatalog(int id)
        {
            var catalog = _dbCtx.Catalog.Find(id);
            if (catalog == null) return NotFound();
            _dbCtx.Catalog.Remove(catalog);
            _dbCtx.SaveChanges();
            return NoContent();
        }
    }
}
