using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class Reservation
{
    [Key]
    public int ReservationId { get; set; }
    public User User { get; set; }
    public Book Book { get; set; }
    public TimeSpan ReservationTime { get; set; }
}
