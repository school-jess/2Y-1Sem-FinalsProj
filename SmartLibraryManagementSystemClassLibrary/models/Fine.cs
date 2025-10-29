using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class Fine
{
    [Key]
    public int FineId { get; set; }
    public int FineAmount { get; set; }
    public Reservation Reservation { get; set; }
    public int AmtPayedSinceLastFine { get; set; }
}
