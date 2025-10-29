namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class FineCreationDto
{
    public int FineAmount { get; set; }
    public int AmtPayedSinceLastFine { get; set; }
    public int UserId { get; set; }
}
