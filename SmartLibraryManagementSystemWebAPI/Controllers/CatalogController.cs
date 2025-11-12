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
                Copies = c.Copies,
                Genre = c.Genre,
                ClassificationId = c.ClassificationId,
                CopiesBorrowed = c.CopiesBorrowed
            }).ToList();
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
                Genre = catalog.Genre,
                ClassificationId = catalog.ClassificationId,
                CopiesBorrowed = catalog.CopiesBorrowed,
                Reservations = catalog.Reservations.Select(r => new ReservationUpdateDto
                {
                    BookId = r.BookId,
                    CatalogId = r.CatalogId,
                    ReservationId = r.ReservationId,
                    ReservationDateTime = r.ReservationDateTime,
                    UserId = r.UserId,
                    ReservationReturnDateTime = r.ReservationReturnDateTime
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
                Genre = catalog.Genre,
                CopiesBorrowed = catalog.CopiesBorrowed
            });
            _dbCtx.SaveChanges();
            var insertedCatalog = _dbCtx.Catalog.First(c => c.BookId == catalog.BookId);
            return CreatedAtAction(nameof(GetCatalog), new { id = insertedCatalog.CatalogId }, new CatalogUpdateDto
            {
                BookId = insertedCatalog.BookId,
                CatalogId = insertedCatalog.CatalogId,
                ClassificationId = insertedCatalog.ClassificationId,
                Copies = insertedCatalog.Copies,
                Genre = insertedCatalog.Genre,
            });
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateCatalog(int id, [FromBody] CatalogUpdateDto catalog)
        {
            if (id != catalog.CatalogId) return BadRequest();
            Catalog updatedCatalog = new Catalog
            {
                BookId = catalog.BookId,
                CatalogId = catalog.CatalogId,
                Copies = catalog.Copies,
                ClassificationId = catalog.ClassificationId,
                Genre = catalog.Genre,
                CopiesBorrowed = catalog.CopiesBorrowed
            };
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
