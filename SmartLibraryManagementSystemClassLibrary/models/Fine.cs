using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class Fine
{
    [Key]
    public int FineId { get; set; }
    public int FineAmount { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public int ReservatonId { get; set; }
    [ForeignKey("ReservatonId")]
    public Reservation Reservaton { get; set; }

    public Fine(int fineId, int fineAmount, int userId, int reservatonId)
    {
        FineId = fineId;
        FineAmount = fineAmount;
        UserId = userId;
        ReservatonId = reservatonId;
    }

    public Fine(int fineAmount, int userId, int reservatonId)
    {
        FineAmount = fineAmount;
        UserId = userId;
        ReservatonId = reservatonId;
    }
}
