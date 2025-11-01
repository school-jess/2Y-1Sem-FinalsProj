namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class FacultyCreationDto
{
    public string FacultyName { get; set; }
    public string Department { get; set; }
    public string Subject { get; set; }
    public string Course { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsLoggedIn { get; set; }
}

public class FacultyUpdateDto
{
    public int FacultyId { get; set; }
    public string FacultyName { get; set; }
    public string Department { get; set; }
    public string Subject { get; set; }
    public string Course { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsLoggedIn { get; set; }
}

public class FacultyGet1Dto
{
    public int FacultyId { get; set; }
    public string FacultyName { get; set; }
    public string Department { get; set; }
    public string Subject { get; set; }
    public string Course { get; set; }
    public UserUpdateDto User { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsLoggedIn { get; set; }
}