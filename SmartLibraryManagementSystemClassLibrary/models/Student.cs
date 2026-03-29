using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SmartLibraryManagementSystemClassLibrary.Model;

[Index(nameof(Email), IsUnique = true)]
public class Student
{
    [Key] public int StudentId { get; set; }
    [StringLength(50)] public string StudentName { get; set; }
    [StringLength(10)] public string Department { get; set; }
    [StringLength(2)] public string Course { get; set; }
    public int Grade { get; set; }
    [DataType(DataType.EmailAddress)] public string Email { get; set; }
    public bool IsLoggedIn { get; set; }
    [DataType(DataType.Password)] public string Password { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")] public User User { get; set; }
    [StringLength(255)] public string ProfileImgPath { get; set; }

    public Student(string studentName, string department, string course, int grade, string email, bool isLoggedIn,
        string password, int userId, string profileImgPath)
    {
        StudentName = studentName;
        Department = department;
        Course = course;
        Grade = grade;
        Email = email;
        IsLoggedIn = isLoggedIn;
        Password = password;
        UserId = userId;
        ProfileImgPath = profileImgPath;
    }

    public void UpdateStudent(int studentId, string studentName, string department, string course, int grade,
        string email, bool isLoggedIn, string password, int userId, string profileImgPath)
    {
        StudentId = studentId;
        StudentName = studentName;
        Department = department;
        Course = course;
        Grade = grade;
        Email = email;
        IsLoggedIn = isLoggedIn;
        Password = password;
        UserId = userId;
        ProfileImgPath = profileImgPath;
    }
}
