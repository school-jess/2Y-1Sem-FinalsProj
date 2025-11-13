using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class UserCreationDto
{
    [StringLength(50)] public string UserName { get; set; }
    public bool HasFine { get; set; }
    public bool HasLoan { get; set; }
    public bool IsAdmin { get; set; }

    public UserCreationDto(string userName, bool hasFine, bool hasLoan, bool isAdmin)
    {
        UserName = userName;
        HasFine = hasFine;
        HasLoan = hasLoan;
        IsAdmin = isAdmin;
    }
}

public class UserUpdateDto
{
    public int UserId { get; set; }
    [StringLength(50)] public string UserName { get; set; }
    public bool HasFine { get; set; }
    public bool HasLoan { get; set; }
    public bool IsAdmin { get; set; }

    public UserUpdateDto(int userId, string userName, bool hasFine, bool hasLoan, bool isAdmin)
    {
        UserId = userId;
        UserName = userName;
        HasFine = hasFine;
        HasLoan = hasLoan;
        IsAdmin = isAdmin;
    }
}

public class UserGet1Dto
{
    public int UserId { get; set; }
    [StringLength(50)] public string UserName { get; set; }
    public StudentUpdateDto? Student { get; set; }
    public FacultyUpdateDto? Faculty { get; set; }
    public bool HasFine { get; set; }
    public bool HasLoan { get; set; }
    public List<ReservationUpdateDto> Reservations { get; set; }
    public List<FineUpdateDto> Fines { get; set; }
    public List<LoanUpdateDto> Loans { get; set; }
    public bool IsAdmin { get; set; }

    public UserGet1Dto(int userId, string userName, StudentUpdateDto? student, FacultyUpdateDto? faculty, bool hasFine,
        bool hasLoan, List<ReservationUpdateDto> reservations, List<FineUpdateDto> fines, List<LoanUpdateDto> loans,
        bool isAdmin)
    {
        UserId = userId;
        UserName = userName;
        Student = student;
        Faculty = faculty;
        HasFine = hasFine;
        HasLoan = hasLoan;
        Reservations = reservations;
        Fines = fines;
        Loans = loans;
        IsAdmin = isAdmin;
    }
}

public class UserWithReservationsDto
{
    public int UserId { get; set; }
    [StringLength(50)] public string UserName { get; set; }
    public StudentUpdateDto? Student { get; set; }
    public FacultyUpdateDto? Faculty { get; set; }
    public bool HasFine { get; set; }
    public bool HasLoan { get; set; }
    public List<ReservationUserDto> Reservations { get; set; }
    public List<FineUpdateDto> Fines { get; set; }
    public List<LoanUpdateDto> Loans { get; set; }
    public bool IsAdmin { get; set; }

    public UserWithReservationsDto(int userId, string userName, StudentUpdateDto? student, FacultyUpdateDto? faculty,
        bool hasFine, bool hasLoan, List<ReservationUserDto> reservations, List<FineUpdateDto> fines,
        List<LoanUpdateDto> loans, bool isAdmin)
    {
        UserId = userId;
        UserName = userName;
        Student = student;
        Faculty = faculty;
        HasFine = hasFine;
        HasLoan = hasLoan;
        Reservations = reservations;
        Fines = fines;
        Loans = loans;
        IsAdmin = isAdmin;
    }
}
