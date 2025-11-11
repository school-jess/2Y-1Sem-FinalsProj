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
}
