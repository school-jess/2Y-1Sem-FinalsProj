using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class BookCreationDto
{
    [StringLength(30)] public string BookName { get; set; }
    [StringLength(50)] public string Author { get; set; }
    [DataType(DataType.Date)] public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; }
    [StringLength(128)] public string BookImgPath { get; set; }

    public BookCreationDto(string bookName, string author, DateTime releaseDate, string synopsis, string bookImgPath)
    {
        BookName = bookName;
        Author = author;
        ReleaseDate = releaseDate;
        Synopsis = synopsis;
        BookImgPath = bookImgPath;
    }
}

public class BookUpdateDto
{
    public int BookId { get; set; }
    [StringLength(30)] public string BookName { get; set; }
    [StringLength(50)] public string Author { get; set; }
    [DataType(DataType.Date)] public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; }
    [StringLength(128)] public string BookImgPath { get; set; }

    public BookUpdateDto(int bookId, string bookName, string author, DateTime releaseDate, string synopsis,
        string bookImgPath)
    {
        BookId = bookId;
        BookName = bookName;
        Author = author;
        ReleaseDate = releaseDate;
        Synopsis = synopsis;
        BookImgPath = bookImgPath;
    }
}

public class BookGet1Dto
{
    public int BookId { get; set; }
    [StringLength(30)] public string BookName { get; set; }
    [StringLength(50)] public string Author { get; set; }
    public CatalogUpdateDto Catalog { get; set; }
    public List<ReservationUpdateDto> Reservations { get; set; }
    [DataType(DataType.Date)] public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; }
    [StringLength(128)] public string BookImgPath { get; set; }

    public BookGet1Dto(int bookId, string bookName, string author, CatalogUpdateDto catalog,
        List<ReservationUpdateDto> reservations, DateTime releaseDate, string synopsis, string bookImgPath)
    {
        BookId = bookId;
        BookName = bookName;
        Author = author;
        Catalog = catalog;
        Reservations = reservations;
        ReleaseDate = releaseDate;
        Synopsis = synopsis;
        BookImgPath = bookImgPath;
    }
}