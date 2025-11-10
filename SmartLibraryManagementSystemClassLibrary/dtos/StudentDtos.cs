namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class StudentCreationDto
{
    public string StudentName { get; set; }
    public string Department { get; set; }
    public string Course { get; set; }
    public int Grade { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsLoggedIn { get; set; }
    public int UserId { get; set; }
}

public class StudentUpdateDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public string Department { get; set; }
    public string Course { get; set; }
    public int Grade { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsLoggedIn { get; set; }
    public int UserId { get; set; }
}

public class StudentGet1Dto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public string Department { get; set; }
    public string Course { get; set; }
    public int Grade { get; set; }
    public UserUpdateDto User { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsLoggedIn { get; set; }
}
