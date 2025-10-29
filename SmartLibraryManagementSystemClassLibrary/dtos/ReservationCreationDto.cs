namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class ReservationCreationDto
{
    public int UserId { get; set; }
    public int BookId { get; set; }
    public DateTime ReservationTime { get; set; }
    public int CatalogId { get; set; }
}
