namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class BookCreationDto
{
    public string BookName { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
}

public class BookUpdateDto
{
    public int BookId { get; set; }
    public string BookName { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }
}