using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class Catalog
{
    [Key]
    public int CatalogId { get; set; }
    public int BookId { get; set; }
    [ForeignKey("BookId")]
    public Book Book { get; set; }
    public int Copies { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
}
