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
    [StringLength(128)]
    public string BookImgPath { get; set; }

    public Book(int bookId, string bookName, string author, DateTime releaseDate, string synopsis, string bookImgPath)
    {
        BookId = bookId;
        BookName = bookName;
        Author = author;
        ReleaseDate =  releaseDate;
        Synopsis = synopsis;
        BookImgPath = bookImgPath;
    }

    public Book(string bookName, string author, DateTime releaseDate, string synopsis, string bookImgPath)
    {
        BookName = bookName;
        Author = author;
        ReleaseDate =  releaseDate;
        Synopsis = synopsis;
        BookImgPath = bookImgPath;
    }

    public void UpdateBook(int bookId, string bookName, string author, string synopsis, DateTime releaseDate, string bookImgPath)
    {
        BookId = bookId;
        BookName = bookName;
        Author = author;
        Synopsis = synopsis;
        ReleaseDate = releaseDate;
        BookImgPath = bookImgPath;
    }
}
