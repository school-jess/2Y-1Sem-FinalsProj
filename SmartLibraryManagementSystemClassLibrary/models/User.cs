using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SmartLibraryManagementSystemClassLibrary.Model;

[Index(nameof(UserName), IsUnique = true)]
public class User
{
    [Key]
    public int UserId { get; set; }
    [StringLength(50)]
    public string UserName { get; set; }
    public int? StudentId { get; set; }
    [ForeignKey("StudentId")]
    public Student? Student { get; set; }
    public int? FacultyId { get; set; }
    [ForeignKey("FacultyId")]
    public Faculty? Faculty { get; set; }
    public ICollection<Fine> PrevFine { get; set; }
    public ICollection<Loan> PrevLoan { get; set; }
    public bool HasFine { get; set; }
    public bool HasLoan { get; set; }
    public bool IsFaculty { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
    public bool IsAdmin { get; set; }
}
