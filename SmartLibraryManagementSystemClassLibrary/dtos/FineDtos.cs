namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class FineCreationDto
{
    public int FineAmount { get; set; }
    public int UserId { get; set; }
    public bool HasPayed { get; set; }
    public int ReservatonId { get; set; }

    public FineCreationDto(int fineAmount, int userId, bool hasPayed, int reservatonId)
    {
        FineAmount = fineAmount;
        UserId = userId;
        HasPayed = hasPayed;
        ReservatonId = reservatonId;
    }
}

public class FineUpdateDto
{
    public int FineId { get; set; }
    public int FineAmount { get; set; }
    public bool HasPayed { get; set; }
    public int UserId { get; set; }
    public int ReservatonId { get; set; }

    public FineUpdateDto(int fineId, int fineAmount, bool hasPayed, int userId, int reservatonId)
    {
        FineId = fineId;
        FineAmount = fineAmount;
        HasPayed = hasPayed;
        UserId = userId;
        ReservatonId = reservatonId;
    }

    public FineUpdateDto(int fineId, int fineAmount, bool hasPayed, int userId)
    {
        FineId = fineId;
        FineAmount = fineAmount;
        HasPayed = hasPayed;
        UserId = userId;
    }
}

public class FineGet1Dto
{
    public int FineId { get; set; }
    public int FineAmount { get; set; }
    public bool HasPayed { get; set; }
    public UserUpdateDto User { get; set; }
    public ReservationUpdateDto Reservation { get; set; }

    public FineGet1Dto(int fineId, int fineAmount, bool hasPayed, UserUpdateDto user, ReservationUpdateDto reservation)
    {
        FineId = fineId;
        FineAmount = fineAmount;
        HasPayed = hasPayed;
        User = user;
        Reservation = reservation;
    }
}
