namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class BookCreationDto
{
    public string BookName { get; set; }
    public string Author { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; }
}

public class BookUpdateDto
{
    public int BookId { get; set; }
    public string BookName { get; set; }
    public string Author { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; }
}

public class BookGet1Dto
{
    public int BookId { get; set; }
    public string BookName { get; set; }
    public string Author { get; set; }
    public CatalogUpdateDto Catalog { get; set; }
    public List<ReservationUpdateDto> Reservations { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string Synopsis { get; set; }
}