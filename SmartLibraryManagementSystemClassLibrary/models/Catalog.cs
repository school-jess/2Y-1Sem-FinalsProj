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
    public int CopiesBorrowed { get; set; }

    public Catalog(int catalogId, int bookId, int copies, string genre, string classificationId, int copiesBorrowed)
    {
        CatalogId = catalogId;
        BookId = bookId;
        Copies = copies;
        Genre = genre;
        ClassificationId = classificationId;
        CopiesBorrowed = copiesBorrowed;
    }

    public Catalog(int bookId, int copies, string genre, string classificationId, int copiesBorrowed)
    {
        BookId = bookId;
        Copies = copies;
        Genre = genre;
        ClassificationId = classificationId;
        CopiesBorrowed = copiesBorrowed;
    }
}
