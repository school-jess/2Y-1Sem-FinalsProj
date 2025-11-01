using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class Student
{
    [Key]
    public int StudentId { get; set; }
    [StringLength(50)]
    public string StudentName { get; set; }
    [StringLength(5)]
    public string Department { get; set; }
    [StringLength(2)]
    public string Course { get; set; }
    public int Grade { get; set; }
    public User User { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    public bool IsLoggedIn { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
