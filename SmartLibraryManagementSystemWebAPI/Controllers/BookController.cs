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
            var books = (
                from book in _dbCtx.Book
                select new BookUpdateDto
                {
                    Author = book.Author,
                    BookId = book.BookId,
                    BookName = book.BookName,
                    Synopsis = book.Synopsis,
                    ReleaseDate = book.ReleaseDate,
                }).ToList();
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetBook(int id)
        {
            var book = (from b in _dbCtx.Book.Include(b => b.Catalog).Include(b => b.Reservation)
                where b.BookId == id
                select b).First();
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
                        ReservationTime = r.ReservationTime,
                        UserId = r.UserId
                    }
                ).ToList()
            });
        }

        [HttpGet("{name}")]
        public IActionResult GetBookName(string name)
        {
            var book = (from b in _dbCtx.Book.Include(b => b.Catalog).Include(b => b.Reservation)
                where b.BookName == name
                select b).First();
            if (book == null) return NotFound();
            return Ok(new BookUpdateDto
            {
                Author = book.Author,
                BookId = book.BookId,
                BookName = book.BookName,
                Synopsis = book.Synopsis,
                ReleaseDate = book.ReleaseDate,
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
            var insertedBook = (from b in _dbCtx.Book
                where b.BookName == book.BookName
                select b).First();
            return CreatedAtAction(nameof(GetBook), new { id = insertedBook.BookId }, insertedBook);
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
