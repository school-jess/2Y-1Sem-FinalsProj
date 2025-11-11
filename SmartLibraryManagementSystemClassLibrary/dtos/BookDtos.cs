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
}