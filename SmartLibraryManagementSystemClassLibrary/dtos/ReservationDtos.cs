namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class ReservationCreationDto
{
    public int UserId { get; set; }
    public int BookId { get; set; }
    public DateTime ReservationDateTime { get; set; }
    public int CatalogId { get; set; }
    public DateTime ReservationReturnDateTime { get; set; }
}

public class ReservationUpdateDto
{

    public int ReservationId { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public DateTime ReservationDateTime { get; set; }
    public int CatalogId { get; set; }
    public DateTime ReservationReturnDateTime { get; set; }
}

public class ReservationGet1Dto
{

    public int ReservationId { get; set; }
    public UserUpdateDto User { get; set; }
    public BookUpdateDto Book { get; set; }
    public DateTime ReservationDateTime { get; set; }
    public CatalogUpdateDto Catalog { get; set; }
    public FineUpdateDto Fine { get; set; }
    public LoanUpdateDto Loan { get; set; }
    public DateTime ReservationReturnDateTime { get; set; }
}

public class ReservationUserDto
{

    public int ReservationId { get; set; }
    public int UserId { get; set; }
    public BookUpdateDto Book { get; set; }
    public DateTime ReservationDateTime { get; set; }
    public CatalogUpdateDto Catalog { get; set; }
    public FineUpdateDto Fine { get; set; }
    public LoanUpdateDto Loan { get; set; }
    public DateTime ReservationReturnDateTime { get; set; }
}
