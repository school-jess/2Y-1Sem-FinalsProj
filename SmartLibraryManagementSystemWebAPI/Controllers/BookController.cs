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
            var books = _dbCtx.Book
                .Select(b => new BookUpdateDto(
                    b.BookId,
                    b.BookName,
                    b.Author,
                    b.ReleaseDate,
                    b.Synopsis)).ToList();
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
            return Ok(new BookGet1Dto(
                book.BookId,
                book.BookName,
                book.Author,
                new CatalogUpdateDto(
                    book.Catalog.CatalogId,
                    book.Catalog.BookId,
                    book.Catalog.Copies,
                    book.Catalog.Genre,
                    book.Catalog.ClassificationId,
                    book.Catalog.CopiesBorrowed),
                book.Reservation
                    .Select(r => new ReservationUpdateDto(
                        r.ReservationId,
                        r.UserId,
                        r.BookId,
                        r.ReservationDateTime,
                        r.CatalogId,
                        r.ReservationReturnDateTime,
                        r.HasFine)).ToList(),
                book.ReleaseDate,
                book.Synopsis));
        }

        [HttpPost]
        public IActionResult NewBook([FromBody] BookCreationDto book)
        {
            _dbCtx.Book.Add(new Book(
                book.BookName,
                book.Author,
                book.ReleaseDate,
                book.Synopsis));
            _dbCtx.SaveChanges();
            var insertedBook = _dbCtx.Book.First(b => b.BookName == book.BookName);
            return CreatedAtAction(nameof(GetBook), new { id = insertedBook.BookId },
                new BookUpdateDto(
                    insertedBook.BookId,
                    insertedBook.BookName,
                    insertedBook.Author,
                    insertedBook.ReleaseDate,
                    insertedBook.Synopsis));
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook(int id, [FromBody] BookUpdateDto book)
        {
            if (id != book.BookId) return BadRequest();
            Book updatedBook = new Book(
                book.BookId,
                book.BookName,
                book.Author,
                book.ReleaseDate,
                book.Synopsis);
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
