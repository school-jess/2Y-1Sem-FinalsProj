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
    public int UserId { get; set; }
    public int ReservatonId { get; set; }
    public bool HasPayed { get; set; }

    public FineUpdateDto(int fineId, int fineAmount, int userId, int reservatonId, bool hasPayed)
    {
        FineId = fineId;
        FineAmount = fineAmount;
        UserId = userId;
        ReservatonId = reservatonId;
        HasPayed = hasPayed;
    }

    public FineUpdateDto(int fineId, int fineAmount, int userId, bool hasPayed)
    {
        FineId = fineId;
        FineAmount = fineAmount;
        UserId = userId;
        HasPayed = hasPayed;
    }
}

public class FineGet1Dto
{
    public int FineId { get; set; }
    public int FineAmount { get; set; }
    public UserUpdateDto User { get; set; }
    public ReservationUpdateDto Reservation { get; set; }
    public bool HasPayed { get; set; }

    public FineGet1Dto(int fineId, int fineAmount, UserUpdateDto user, ReservationUpdateDto reservation, bool hasPayed)
    {
        FineId = fineId;
        FineAmount = fineAmount;
        User = user;
        Reservation = reservation;
        HasPayed = hasPayed;
    }
}
