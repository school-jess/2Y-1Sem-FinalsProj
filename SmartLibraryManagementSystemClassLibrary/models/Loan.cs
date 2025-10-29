using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class Loan
{
    [Key]
    public int LoanId { get; set; }
    public int LoanAmount { get; set; }
    public int AmtLoanedSinceLastLoaned { get; set; }
    public int UserId { get; set; }
    [ForeignKey("UserId")]
    public User User { get; set; }
}
