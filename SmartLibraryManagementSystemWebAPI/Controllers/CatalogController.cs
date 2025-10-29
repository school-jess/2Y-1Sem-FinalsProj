using SmartLibraryManagementSystemClassLibrary.Model;
using SmartLibraryManagementSystemClassLibrary.Dtos;
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
        public IActionResult NewCatalog([FromBody] CatalogCreationDto catalog)
        {
            _dbCtx.Catalog.Add(new Catalog { BookId = catalog.BookId, Copies = catalog.Copies });
            _dbCtx.SaveChanges();
            var insertedCatalog = _dbCtx.Catalog.Find(catalog);
            return CreatedAtAction("", new { catalogId = insertedCatalog.CatalogId });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCatalog(int id, [FromBody] CatalogUpdateDto catalog)
        {
            if (id != catalog.CatalogId) return BadRequest();
            Catalog updatedCatalog = new Catalog { BookId = catalog.BookId, CatalogId = catalog.CatalogId, Copies = catalog.Copies };
            _dbCtx.Entry(updatedCatalog).State = EntityState.Modified;
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
