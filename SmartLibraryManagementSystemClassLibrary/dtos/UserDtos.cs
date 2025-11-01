namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class UserCreationDto
{
    public int UserName { get; set; }
    public int StudentId { get; set; }
    public int FacultyId { get; set; }
    public bool HasFine { get; set; }
    public bool HasLoan { get; set; }
    public bool IsFaculty { get; set; }
    public bool IsAdmin { get; set; }
}

public class UserUpdateDto
{
    public int UserId { get; set; }
    public int UserName { get; set; }
    public int StudentId { get; set; }
    public int FacultyId { get; set; }
    public bool HasFine { get; set; }
    public bool HasLoan { get; set; }
    public bool IsFaculty { get; set; }
    public bool IsAdmin { get; set; }
}

public class UserGet1Dto
{
    public int UserId { get; set; }
    public int UserName { get; set; }
    public StudentUpdateDto Student { get; set; }
    public FacultyUpdateDto Faculty { get; set; }
    public bool HasFine { get; set; }
    public bool HasLoan { get; set; }
    public bool IsFaculty { get; set; }
    public List<ReservationUpdateDto> Reservations { get; set; }
    public bool IsAdmin { get; set; }
}
