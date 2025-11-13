using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class StudentCreationDto
{
    [StringLength(50)]
    public string StudentName { get; set; }
    [StringLength(10)]
    public string Department { get; set; }
    [StringLength(2)]
    public string Course { get; set; }
    public int Grade { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool IsLoggedIn { get; set; }
    public int UserId { get; set; }

    public StudentCreationDto(string studentName, string department, string course, int grade, string email, string password, bool isLoggedIn, int userId)
    {
        StudentName = studentName;
        Department = department;
        Course = course;
        Grade = grade;
        Email = email;
        Password = password;
        IsLoggedIn = isLoggedIn;
        UserId = userId;
    }
}

public class StudentUpdateDto
{
    public int StudentId { get; set; }
    [StringLength(50)]
    public string StudentName { get; set; }
    [StringLength(10)]
    public string Department { get; set; }
    [StringLength(2)]
    public string Course { get; set; }
    public int Grade { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool IsLoggedIn { get; set; }
    public int UserId { get; set; }

    public StudentUpdateDto(int studentId, string studentName, string department, string course, int grade, string email, string password, bool isLoggedIn, int userId)
    {
        StudentId = studentId;
        StudentName = studentName;
        Department = department;
        Course = course;
        Grade = grade;
        Email = email;
        Password = password;
        IsLoggedIn = isLoggedIn;
        UserId = userId;
    }
}

public class StudentGet1Dto
{
    public int StudentId { get; set; }
    [StringLength(50)]
    public string StudentName { get; set; }
    [StringLength(10)]
    public string Department { get; set; }
    [StringLength(2)]
    public string Course { get; set; }
    public int Grade { get; set; }
    public UserUpdateDto User { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool IsLoggedIn { get; set; }

    public StudentGet1Dto(int studentId, string studentName, string department, string course, int grade, UserUpdateDto user, string email, string password, bool isLoggedIn)
    {
        StudentId = studentId;
        StudentName = studentName;
        Department = department;
        Course = course;
        Grade = grade;
        User = user;
        Email = email;
        Password = password;
        IsLoggedIn = isLoggedIn;
    }
}
