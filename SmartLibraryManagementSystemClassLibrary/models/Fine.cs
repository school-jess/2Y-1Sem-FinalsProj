using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class Fine
{
    [Key]
    public int FineId { get; set; }
    public int FineAmount { get; set; }
    public int AmtPayedSinceLastFine { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
}
