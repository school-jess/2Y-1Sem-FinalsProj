namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class CatalogCreationDto
{
    public int BookId { get; set; }
    public int Copies { get; set; }
}

public class CatalogUpdateDto
{
    public int CatalogId { get; set; }
    public int BookId { get; set; }
    public int Copies { get; set; }
}
