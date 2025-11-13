using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class Loan
{
    [Key]
    public int LoanId { get; set; }
    public int LoanAmount { get; set; }
    public bool HasPayed { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
    public int ReservatonId { get; set; }
    [ForeignKey("ReservatonId")]
    public Reservation Reservaton { get; set; }

    public Loan(int loanId, int loanAmount, bool hasPayed, int userId, int reservationId)
    {
        LoanId = loanId;
        LoanAmount = loanAmount;
        HasPayed = hasPayed;
        UserId = userId;
        ReservatonId = reservationId;
    }

    public Loan(int loanAmount, bool hasPayed, int userId, int reservationId)
    {
        LoanAmount = loanAmount;
        HasPayed = hasPayed;
        UserId = userId;
        ReservatonId = reservationId;
    }
}
