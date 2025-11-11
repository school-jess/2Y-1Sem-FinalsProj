using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class FacultyCreationDto
{
    [StringLength(50)]
    public string FacultyName { get; set; }
    [StringLength(10)]
    public string Department { get; set; }
    [StringLength(20)]
    public string Subject { get; set; }
    [StringLength(2)]
    public string Course { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool IsLoggedIn { get; set; }
    public int UserId { get; set; }
}

public class FacultyUpdateDto
{
    public int FacultyId { get; set; }
    [StringLength(50)]
    public string FacultyName { get; set; }
    [StringLength(10)]
    public string Department { get; set; }
    [StringLength(20)]
    public string Subject { get; set; }
    [StringLength(2)]
    public string Course { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool IsLoggedIn { get; set; }
    public int UserId { get; set; }
}

public class FacultyGet1Dto
{
    public int FacultyId { get; set; }
    [StringLength(50)]
    public string FacultyName { get; set; }
    [StringLength(10)]
    public string Department { get; set; }
    [StringLength(20)]
    public string Subject { get; set; }
    [StringLength(2)]
    public string Course { get; set; }
    public UserUpdateDto User { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    public string Password { get; set; }
    public bool IsLoggedIn { get; set; }
}