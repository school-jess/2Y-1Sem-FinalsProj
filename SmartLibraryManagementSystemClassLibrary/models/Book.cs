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
    [StringLength(15)]
    public string Genre { get; set; }
}
