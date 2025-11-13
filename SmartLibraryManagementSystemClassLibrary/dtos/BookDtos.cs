using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class BookCreationDto
{
    [StringLength(30)]
    public string BookName { get; set; }
    [StringLength(50)]
    public string Author { get; set; }
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; }

    public BookCreationDto(string bookName, string author, DateTime releaseDate, string synopsis)
    {
        BookName = bookName;
        Author = author;
        ReleaseDate = releaseDate;
        Synopsis = synopsis;
    }
}

public class BookUpdateDto
{
    public int BookId { get; set; }
    [StringLength(30)]
    public string BookName { get; set; }
    [StringLength(50)]
    public string Author { get; set; }
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; }

    public BookUpdateDto(int bookId, string bookName, string author, DateTime releaseDate, string synopsis)
    {
        BookId = bookId;
        BookName = bookName;
        Author = author;
        ReleaseDate = releaseDate;
        Synopsis = synopsis;
    }
}

public class BookGet1Dto
{
    public int BookId { get; set; }
    [StringLength(30)]
    public string BookName { get; set; }
    [StringLength(50)]
    public string Author { get; set; }
    public CatalogUpdateDto Catalog { get; set; }
    public List<ReservationUpdateDto> Reservations { get; set; }
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; }

    public BookGet1Dto(int bookId, string bookName, string author, CatalogUpdateDto catalog, List<ReservationUpdateDto> reservations, DateTime releaseDate, string synopsis)
    {
        BookId = bookId;
        BookName = bookName;
        Author = author;
        Catalog = catalog;
        Reservations = reservations;
        ReleaseDate = releaseDate;
        Synopsis = synopsis;
    }
}