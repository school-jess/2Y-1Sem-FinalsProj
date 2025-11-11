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
}
