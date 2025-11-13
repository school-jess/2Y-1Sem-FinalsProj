using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class CatalogCreationDto
{
    public int BookId { get; set; }
    public int Copies { get; set; }
    [StringLength(10)]
    public string Genre { get; set; }
    [StringLength(10)]
    public string ClassificationId { get; set; }
    public int CopiesBorrowed { get; set; }

    public CatalogCreationDto(int bookId, int copies, string genre, string classificationId, int copiesBorrowed)
    {
        BookId = bookId;
        Copies = copies;
        Genre = genre;
        ClassificationId = classificationId;
        CopiesBorrowed = copiesBorrowed;
    }
}

public class CatalogUpdateDto
{
    public int CatalogId { get; set; }
    public int BookId { get; set; }
    public int Copies { get; set; }
    [StringLength(10)]
    public string Genre { get; set; }
    [StringLength(10)]
    public string ClassificationId { get; set; }
    public int CopiesBorrowed { get; set; }

    public CatalogUpdateDto(int catalogId, int bookId, int copies, string genre, string classificationId, int copiesBorrowed)
    {
        CatalogId = catalogId;
        BookId = bookId;
        Copies = copies;
        Genre = genre;
        ClassificationId = classificationId;
        CopiesBorrowed = copiesBorrowed;
    }
}

public class CatalogGet1Dto
{
    public int CatalogId { get; set; }
    public BookUpdateDto Book { get; set; }
    public int Copies { get; set; }
    public ICollection<ReservationUpdateDto> Reservations { get; set; }
    [StringLength(10)]
    public string Genre { get; set; }
    [StringLength(10)]
    public string ClassificationId { get; set; }
    public int CopiesBorrowed { get; set; }

    public CatalogGet1Dto(int catalogId, BookUpdateDto book, int copies, List<ReservationUpdateDto> reservations, string genre, string classificationId, int copiesBorrowed)
    {
        CatalogId = catalogId;
        Book = book;
        Copies = copies;
        Reservations = reservations;
        Genre = genre;
        ClassificationId = classificationId;
        CopiesBorrowed = copiesBorrowed;
    }
}
