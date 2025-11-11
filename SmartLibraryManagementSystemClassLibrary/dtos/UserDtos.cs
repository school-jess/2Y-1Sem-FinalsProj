using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class UserCreationDto
{
    [StringLength(50)]
    public string UserName { get; set; }
    public bool HasFine { get; set; }
    public bool HasLoan { get; set; }
    public bool IsAdmin { get; set; }
}

public class UserUpdateDto
{
    public int UserId { get; set; }
    [StringLength(50)]
    public string UserName { get; set; }
    public bool HasFine { get; set; }
    public bool HasLoan { get; set; }
    public bool IsAdmin { get; set; }
}

public class UserGet1Dto
{
    public int UserId { get; set; }
    [StringLength(50)]
    public string UserName { get; set; }
    public StudentUpdateDto? Student { get; set; }
    public FacultyUpdateDto? Faculty { get; set; }
    public bool HasFine { get; set; }
    public bool HasLoan { get; set; }
    public List<ReservationUpdateDto> Reservations { get; set; }
    public List<FineUpdateDto> Fines { get; set; }
    public List<LoanUpdateDto> Loans { get; set; }
    public bool IsAdmin { get; set; }
}

public class UserWithReservationsDto
{
    public int UserId { get; set; }
    [StringLength(50)]
    public string UserName { get; set; }
    public StudentUpdateDto? Student { get; set; }
    public FacultyUpdateDto? Faculty { get; set; }
    public bool HasFine { get; set; }
    public bool HasLoan { get; set; }
    public List<ReservationUserDto> Reservations { get; set; }
    public List<FineUpdateDto> Fines { get; set; }
    public List<LoanUpdateDto> Loans { get; set; }
    public bool IsAdmin { get; set; }
}
