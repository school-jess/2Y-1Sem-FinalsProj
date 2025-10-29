namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class StudentCreationDto
{
    public string StudentName { get; set; }
    public string Department { get; set; }
    public string Course { get; set; }
    public int Grade { get; set; }
}

public class StudentUpdateDto
{
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public string Department { get; set; }
    public string Course { get; set; }
    public int Grade { get; set; }
}
