namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class LoanCreationDto
{
    public int LoanAmount { get; set; }
    public bool HasPayed { get; set; }
    public int UserId { get; set; }
    public int ReservationId { get; set; }
}

public class LoanUpdateDto
{
    public int LoanId { get; set; }
    public int LoanAmount { get; set; }
    public bool HasPayed { get; set; }
    public int UserId { get; set; }
    public int ReservationId { get; set; }

    public LoanUpdateDto(int loanId, int loanAmount, bool hasPayed, int userId, int reservationId)
    {
        LoanId = loanId;
        LoanAmount = loanAmount;
        HasPayed = hasPayed;
        UserId = userId;
        ReservationId = reservationId;
    }
}

public class LoanGet1Dto
{
    public int LoanId { get; set; }
    public int LoanAmount { get; set; }
    public bool HasPayed { get; set; }
    public UserUpdateDto User { get; set; }
    public ReservationUpdateDto Reservation { get; set; }

    public LoanGet1Dto(int loanId, int loanAmount, bool hasPayed, UserUpdateDto user, ReservationUpdateDto reservation)
    {
        LoanId = loanId;
        LoanAmount = loanAmount;
        HasPayed = hasPayed;
        User = user;
        Reservation = reservation;
    }
}