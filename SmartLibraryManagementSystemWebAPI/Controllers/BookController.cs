using SmartLibraryManagementSystemClassLibrary.Model;
using SmartLibraryManagementSystemClassLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StudentLibraryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly DatabaseContext _dbCtx;

        public BookController(DatabaseContext dbCtx)
        {
            _dbCtx = dbCtx;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var books = _dbCtx.Book.Select(b => new BookUpdateDto
            {
                Author = b.Author,
                BookId = b.BookId,
                BookName = b.BookName,
                Synopsis = b.Synopsis,
                ReleaseDate = b.ReleaseDate,
            }).ToList();
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBook(int id)
        {
            var book = _dbCtx.Book
                .Include(b => b.Catalog)
                .Include(b => b.Reservation)
                .FirstOrDefault(b => b.BookId == id);
            if (book == null) return NotFound();
            return Ok(new BookGet1Dto
            {
                Author = book.Author,
                BookId = book.BookId,
                BookName = book.BookName,
                Synopsis = book.Synopsis,
                ReleaseDate = book.ReleaseDate,
                Catalog = new CatalogUpdateDto
                {
                    BookId = book.Catalog.BookId,
                    CatalogId = book.Catalog.CatalogId,
                    Copies = book.Catalog.Copies,
                    ClassificationId = book.Catalog.ClassificationId,
                    Genre = book.Catalog.Genre
                },
                Reservations = book.Reservation.Select(r => new ReservationUpdateDto
                    {
                        BookId = r.ReservationId,
                        CatalogId = r.CatalogId,
                        ReservationId = r.ReservationId,
                        ReservationDateTime = r.ReservationDateTime,
                        UserId = r.UserId,
                        ReservationReturnDateTime = r.ReservationReturnDateTime
                    }
                ).ToList()
            });
        }

        [HttpPost]
        public IActionResult NewBook([FromBody] BookCreationDto book)
        {
            _dbCtx.Book.Add(new Book
            {
                Author = book.Author,
                BookName = book.BookName,
                Synopsis = book.Synopsis,
                ReleaseDate = book.ReleaseDate,
            });
            _dbCtx.SaveChanges();
            var insertedBook = _dbCtx.Book.First(b => b.BookName == book.BookName);
            return CreatedAtAction(nameof(GetBook), new { id = insertedBook.BookId }, new BookUpdateDto
            {
                Author = insertedBook.Author,
                BookId = insertedBook.BookId,
                BookName = insertedBook.BookName,
                ReleaseDate = insertedBook.ReleaseDate,
                Synopsis = insertedBook.Synopsis
            });
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook(int id, [FromBody] BookUpdateDto book)
        {
            if (id != book.BookId) return BadRequest();
            Book updatedBook = new Book
            {
                Author = book.Author,
                BookId = book.BookId,
                BookName = book.BookName,
            };
            _dbCtx.Entry(updatedBook).State = EntityState.Modified;
            _dbCtx.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _dbCtx.Book.Find(id);
            if (book == null) return NotFound();
            _dbCtx.Book.Remove(book);
            _dbCtx.SaveChanges();
            return NoContent();
        }
    }
}
