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
    public ICollection<Fine> PrevFine { get; set; }
    public ICollection<Loan> PrevLoan { get; set; }
    public bool HasFine { get; set; }
    public bool HasLoan { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
    public bool IsAdmin { get; set; }
    public Student? Student { get; set; }
    public Faculty? Faculty { get; set; }

    public User(int userId, string userName, bool hasFine, bool hasLoan, bool isAdmin)
    {
        UserId = userId;
        UserName = userName;
        HasFine = hasFine;
        HasLoan = hasLoan;
        IsAdmin = isAdmin;
    }

    public User(string userName, bool hasFine, bool hasLoan, bool isAdmin)
    {
        UserName = userName;
        HasFine = hasFine;
        HasLoan = hasLoan;
        IsAdmin = isAdmin;
    }

    public void UpdateUser(int userId, string userName, bool hasFine, bool hasLoan, bool isAdmin)
    {
        UserId = userId;
        UserName = userName;
        HasFine = hasFine;
        HasLoan = hasLoan;
        IsAdmin = isAdmin;
    }
}
