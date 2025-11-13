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
            var catalogs = _dbCtx.Catalog.Select(c =>
                    new CatalogUpdateDto(
                        c.CatalogId,
                        c.BookId,
                        c.Copies,
                        c.Genre,
                        c.ClassificationId,
                        c.CopiesBorrowed))
                .ToList();
            return Ok(catalogs);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetCatalog(int id)
        {
            var catalog = _dbCtx.Catalog
                .Include(c => c.Book)
                .Include(c => c.Reservations)
                .FirstOrDefault(c => c.CatalogId == id);
            if (catalog == null) return NotFound();
            return Ok(new CatalogGet1Dto(
                catalog.CatalogId,
                new BookUpdateDto(catalog.Book.BookId, catalog.Book.BookName, catalog.Book.Author,
                    catalog.Book.ReleaseDate, catalog.Book.Synopsis),
                catalog.Copies,
                catalog.Reservations.Select(r => new ReservationUpdateDto(
                    r.ReservationId,
                    r.UserId,
                    r.BookId,
                    r.ReservationDateTime,
                    r.CatalogId,
                    r.ReservationReturnDateTime,
                    r.HasFine)).ToList(),
                catalog.Genre,
                catalog.ClassificationId,
                catalog.CopiesBorrowed));
        }

        [HttpPost]
        public IActionResult NewCatalog([FromBody] CatalogCreationDto catalog)
        {
            _dbCtx.Catalog.Add(new Catalog(
                catalog.BookId,
                catalog.Copies,
                catalog.Genre,
                catalog.ClassificationId,
                catalog.CopiesBorrowed));
            _dbCtx.SaveChanges();
            var insertedCatalog = _dbCtx.Catalog.First(c => c.BookId == catalog.BookId);
            return CreatedAtAction(nameof(GetCatalog), new { id = insertedCatalog.CatalogId },
                new CatalogUpdateDto(
                    insertedCatalog.CatalogId,
                    insertedCatalog.BookId,
                    insertedCatalog.Copies,
                    insertedCatalog.Genre,
                    insertedCatalog.ClassificationId,
                    insertedCatalog.CopiesBorrowed));
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateCatalog(int id, [FromBody] CatalogUpdateDto catalog)
        {
            if (id != catalog.CatalogId) return BadRequest();
            Catalog updatedCatalog = new Catalog(
                catalog.CatalogId,
                catalog.BookId,
                catalog.Copies,
                catalog.Genre,
                catalog.ClassificationId,
                catalog.CopiesBorrowed);
            _dbCtx.Entry(updatedCatalog).State = EntityState.Modified;
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
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
