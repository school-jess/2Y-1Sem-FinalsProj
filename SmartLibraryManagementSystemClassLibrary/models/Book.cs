using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class Book
{
    [Key]
    public int BookId { get; set; }
    [StringLength(30)]
    public string BookName { get; set; }
    [StringLength(50)]
    public string Author { get; set; }
    public Catalog Catalog { get; set; }
    public ICollection<Reservation> Reservation { get; set; }
    public string Synopsis { get; set; }
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    public Book(int bookId, string bookName, string author, DateTime releaseDate, string synopsis)
    {
        BookId = bookId;
        BookName = bookName;
        Author = author;
        ReleaseDate =  releaseDate;
        Synopsis = synopsis;
    }

    public Book(string bookName, string author, DateTime releaseDate, string synopsis)
    {
        BookName = bookName;
        Author = author;
        ReleaseDate =  releaseDate;
        Synopsis = synopsis;
    }

    public void UpdateBook(int bookId, string bookName, string author, string synopsis, DateTime releaseDate)
    {
        BookId = bookId;
        BookName = bookName;
        Author = author;
        Synopsis = synopsis;
        ReleaseDate = releaseDate;
    }
}
