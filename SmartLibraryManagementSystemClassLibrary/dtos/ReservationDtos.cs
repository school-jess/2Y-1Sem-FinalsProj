namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class ReservationCreationDto
{
    public int UserId { get; set; }
    public int BookId { get; set; }
    public DateTime ReservationDateTime { get; set; }
    public int CatalogId { get; set; }
    public DateTime ReservationReturnDateTime { get; set; }
    public bool HasReturned { get; set; }

    public ReservationCreationDto(int userId, int bookId, DateTime reservationDateTime, int catalogId,
        DateTime reservationReturnDateTime, bool hasReturned)
    {
        UserId = userId;
        BookId = bookId;
        ReservationDateTime = reservationDateTime;
        ReservationReturnDateTime = reservationReturnDateTime;
        CatalogId = catalogId;
        ReservationReturnDateTime = reservationReturnDateTime;
        HasReturned = hasReturned;
    }
}

public class ReservationUpdateDto
{
    public int ReservationId { get; set; }
    public int UserId { get; set; }
    public int BookId { get; set; }
    public DateTime ReservationDateTime { get; set; }
    public int CatalogId { get; set; }
    public DateTime ReservationReturnDateTime { get; set; }
    public bool HasReturned { get; set; }

    public ReservationUpdateDto(int reservationId, int userId, int bookId, DateTime reservationDateTime, int catalogId,
        DateTime reservationReturnDateTime, bool hasReturned)
    {
        ReservationId = reservationId;
        UserId = userId;
        BookId = bookId;
        ReservationDateTime = reservationDateTime;
        CatalogId = catalogId;
        ReservationReturnDateTime = reservationReturnDateTime;
        HasReturned = hasReturned;
    }
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
    public bool HasReturned { get; set; }

    public ReservationGet1Dto(int reservationId, UserUpdateDto user, BookUpdateDto book, DateTime reservationDateTime,
        CatalogUpdateDto catalog, FineUpdateDto fine, LoanUpdateDto loan, DateTime reservationReturnDateTime,
        bool hasReturned)
    {
        ReservationId = reservationId;
        User = user;
        Book = book;
        ReservationDateTime = reservationDateTime;
        Catalog = catalog;
        Fine = fine;
        Loan = loan;
        ReservationReturnDateTime = reservationReturnDateTime;
        HasReturned = hasReturned;
    }
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
    public bool HasReturned { get; set; }

    public ReservationUserDto(int reservationId, int userId, BookUpdateDto book, DateTime reservationDateTime,
        CatalogUpdateDto catalog, FineUpdateDto fine, LoanUpdateDto loan, DateTime reservationReturnDateTime, bool hasReturned)
    {
        ReservationId = reservationId;
        UserId = userId;
        Book = book;
        ReservationDateTime = reservationDateTime;
        Catalog = catalog;
        Fine = fine;
        Loan = loan;
        ReservationReturnDateTime = reservationReturnDateTime;
        HasReturned = hasReturned;
    }
}
