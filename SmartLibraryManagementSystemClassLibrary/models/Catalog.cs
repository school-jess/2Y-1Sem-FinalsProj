using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SmartLibraryManagementSystemClassLibrary.Model;

[Index(nameof(ClassificationId), IsUnique = true)]
public class Catalog
{
    [Key]
    public int CatalogId { get; set; }
    public int BookId { get; set; }
    [ForeignKey("BookId")]
    public Book Book { get; set; }
    public int Copies { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
    [StringLength(10)]
    public string Genre { get; set; }
    [StringLength(10)]
    public string ClassificationId { get; set; }
}
