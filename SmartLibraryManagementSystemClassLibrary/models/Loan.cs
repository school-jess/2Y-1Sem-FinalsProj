using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class Loan
{
    [Key]
    public int LoanId { get; set; }
    public int LoanAmount { get; set; }
    public Reservation Reservation { get; set; }
    public int AmtLoanedSinceLastLoaned { get; set; }
}
