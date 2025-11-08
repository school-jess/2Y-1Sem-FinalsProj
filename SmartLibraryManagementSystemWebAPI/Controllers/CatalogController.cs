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
            var catalogs = _dbCtx.Catalog.Select(c => new CatalogUpdateDto
            {
                BookId = c.BookId,
                CatalogId = c.CatalogId,
                Copies = c.Copies
            }).ToList();
            return Ok(catalogs);
        }

        [HttpGet("{id}")]
        public IActionResult GetCatalog(int id)
        {
            var catalog = _dbCtx.Catalog.Find(id);
            if (catalog == null) return NotFound();
            return Ok(new CatalogGet1Dto
            {
                Book = new BookUpdateDto
                {
                    Author = catalog.Book.Author,
                    BookId = catalog.Book.BookId,
                    BookName = catalog.Book.BookName,
                    ReleaseDate = catalog.Book.ReleaseDate,
                    Synopsis = catalog.Book.Synopsis
                },
                CatalogId = catalog.CatalogId,
                Copies = catalog.Copies,
                Reservations = catalog.Reservations.Select(r => new ReservationUpdateDto
                {
                    BookId = r.BookId,
                    CatalogId = r.CatalogId,
                    ReservationId = r.ReservationId,
                    ReservationTime = r.ReservationTime,
                    UserId = r.UserId
                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult NewCatalog([FromBody] CatalogCreationDto catalog)
        {
            _dbCtx.Catalog.Add(new Catalog
            {
                BookId = catalog.BookId,
                Copies = catalog.Copies,
                ClassificationId = catalog.ClassificationId,
                Genre = catalog.Genre
            });
            _dbCtx.SaveChanges();
            var insertedCatalog = (from c in _dbCtx.Catalog
                                  where c.BookId == catalog.BookId
                                  select c).First();
            return CreatedAtAction("", new { catalogId = insertedCatalog.CatalogId });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCatalog(int id, [FromBody] CatalogUpdateDto catalog)
        {
            if (id != catalog.CatalogId) return BadRequest();
            Catalog updatedCatalog = new Catalog
            {
                BookId = catalog.BookId,
                CatalogId = catalog.CatalogId,
                Copies = catalog.Copies,
                ClassificationId = catalog.ClassificationId,
                Genre = catalog.Genre
            };
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
