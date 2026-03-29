using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SmartLibraryManagementSystemClassLibrary.Model;

[Index(nameof(Email), IsUnique = true)]
public class Faculty
{
    [Key] public int FacultyId { get; set; }
    [StringLength(50)] public string FacultyName { get; set; }
    [StringLength(10)] public string Department { get; set; }
    [StringLength(20)] public string Subject { get; set; }
    [StringLength(2)] public string Course { get; set; }
    [DataType(DataType.EmailAddress)] public string Email { get; set; }
    public bool IsLoggedIn { get; set; }
    [DataType(DataType.Password)] public string Password { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")] public User User { get; set; }
    [StringLength(255)] public string ProfileImgPath { get; set; }

    public Faculty(string facultyName, string department, string subject, string course, string email, bool isLoggedIn,
        string password, int userId, string profileImgPath)
    {
        FacultyName = facultyName;
        Department = department;
        Subject = subject;
        Course = course;
        Email = email;
        IsLoggedIn = isLoggedIn;
        Password = password;
        UserId = userId;
        ProfileImgPath = profileImgPath;
    }

    public void UpdateFaculty(int facultyId, string facultyName, string department, string subject, string course,
        string email, bool isLoggedIn, string password, int userId, string profileImg)
    {
        FacultyId = facultyId;
        FacultyName = facultyName;
        Department = department;
        Subject = subject;
        Course = course;
        Email = email;
        IsLoggedIn = isLoggedIn;
        Password = password;
        UserId = userId;
        ProfileImgPath = profileImg;
    }
}
