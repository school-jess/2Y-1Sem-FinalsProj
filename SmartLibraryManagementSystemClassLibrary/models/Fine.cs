using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class Fine
{
    [Key]
    public int FineId { get; set; }
    public int FineAmount { get; set; }
    public bool HasPayed { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public int ReservatonId { get; set; }
    [ForeignKey("ReservatonId")]
    public Reservation Reservaton { get; set; }

    public Fine(int fineId, int fineAmount, bool hasPayed, int userId, int reservatonId)
    {
        FineId = fineId;
        FineAmount = fineAmount;
        HasPayed = hasPayed;
        UserId = userId;
        ReservatonId = reservatonId;
    }

    public Fine(int fineAmount, bool hasPayed, int userId, int reservatonId)
    {
        FineAmount = fineAmount;
        HasPayed = hasPayed;
        UserId = userId;
        ReservatonId = reservatonId;
    }
}
