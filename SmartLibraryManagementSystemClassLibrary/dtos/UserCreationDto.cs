namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class UserCreationDto
{
    public int UserName { get; set; }
    public int StudentId { get; set; }
    public int FacultyId { get; set; }
    public bool HasFine { get; set; }
    public bool HasLoan { get; set; }
    public bool IsFaculty { get; set; }
}
