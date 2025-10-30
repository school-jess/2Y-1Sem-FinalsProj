namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class FineCreationDto
{
    public int FineAmount { get; set; }
    public int AmtPayedSinceLastFine { get; set; }
    public int UserId { get; set; }
}

public class FineUpdateDto
{
    public int FineId { get; set; }
    public int FineAmount { get; set; }
    public int AmtPayedSinceLastFine { get; set; }
    public int UserId { get; set; }
}

public class FineGet1Dto
{
    public int FineId { get; set; }
    public int FineAmount { get; set; }
    public int AmtPayedSinceLastFine { get; set; }
    public UserUpdateDto User { get; set; }
    public ReservationUpdateDto Reservation { get; set; }
}
