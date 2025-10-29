namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class LoanCreationDto
{
    public int LoanAmount { get; set; }
    public int AmtLoanedSinceLastLoaned { get; set; }
    public int UserId { get; set; }
}


public class LoanUpdateDto
{
    public int LoanId { get; set; }
    public int LoanAmount { get; set; }
    public int AmtLoanedSinceLastLoaned { get; set; }
    public int UserId { get; set; }
}