using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class Reservation
{
    [Key]
    public int ReservationId { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public int BookId { get; set; }
    [ForeignKey("BookId")]
    public Book Book { get; set; }
    [DataType(DataType.Time)]
    public DateTime ReservationTime { get; set; }
    public int CatalogId { get; set; }
    [ForeignKey("CatalogId")]
    public Catalog Catalog { get; set; }
}
