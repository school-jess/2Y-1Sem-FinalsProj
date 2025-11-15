using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class FacultyCreationDto
{
    [StringLength(50)] public string FacultyName { get; set; }
    [StringLength(10)] public string Department { get; set; }
    [StringLength(20)] public string Subject { get; set; }
    [StringLength(2)] public string Course { get; set; }
    [DataType(DataType.EmailAddress)] public string Email { get; set; }
    [DataType(DataType.Password)] public string Password { get; set; }
    public bool IsLoggedIn { get; set; }
    public int UserId { get; set; }

    public FacultyCreationDto(string facultyName, string department, string subject, string course, string email,
        string password, bool isLoggedIn, int userId)
    {
        FacultyName = facultyName;
        Department = department;
        Subject = subject;
        Course = course;
        Email = email;
        Password = password;
        IsLoggedIn = isLoggedIn;
        UserId = userId;
    }
}

public class FacultyUpdateDto
{
    public int FacultyId { get; set; }
    [StringLength(50)] public string FacultyName { get; set; }
    [StringLength(10)] public string Department { get; set; }
    [StringLength(20)] public string Subject { get; set; }
    [StringLength(2)] public string Course { get; set; }
    [DataType(DataType.EmailAddress)] public string Email { get; set; }
    [DataType(DataType.Password)] public string Password { get; set; }
    public bool IsLoggedIn { get; set; }
    public int UserId { get; set; }

    public FacultyUpdateDto(int facultyId, string facultyName, string department, string subject, string course,
        string email, string password, bool isLoggedIn, int userId)
    {
        FacultyId = facultyId;
        FacultyName = facultyName;
        Department = department;
        Subject = subject;
        Course = course;
        Email = email;
        Password = password;
        IsLoggedIn = isLoggedIn;
        UserId = userId;
    }

    public FacultyUpdateDto(int facultyId, string facultyName, string department, string subject, string course,
        string email, string password, bool isLoggedIn)
    {
        FacultyId = facultyId;
        FacultyName = facultyName;
        Department = department;
        Subject = subject;
        Course = course;
        Email = email;
        Password = password;
        IsLoggedIn = isLoggedIn;
    }

    public FacultyUpdateDto()
    {
    }
}

public class FacultyGet1Dto
{
    public int FacultyId { get; set; }
    [StringLength(50)] public string FacultyName { get; set; }
    [StringLength(10)] public string Department { get; set; }
    [StringLength(20)] public string Subject { get; set; }
    [StringLength(2)] public string Course { get; set; }
    public UserUpdateDto User { get; set; }
    [DataType(DataType.EmailAddress)] public string Email { get; set; }
    [DataType(DataType.Password)] public string Password { get; set; }
    public bool IsLoggedIn { get; set; }

    public FacultyGet1Dto(int facultyId, string facultyName, string department, string subject, string course,
        UserUpdateDto user, string email, string password, bool isLoggedIn)
    {
        FacultyId = facultyId;
        FacultyName = facultyName;
        Department = department;
        Subject = subject;
        Course = course;
        User = user;
        Email = email;
        Password = password;
        IsLoggedIn = isLoggedIn;
    }
}