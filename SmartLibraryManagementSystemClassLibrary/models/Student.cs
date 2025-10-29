using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class Student
{
    public int StudentId { get; set; }
    [StringLength(50)]
    public string StudentName { get; set; }
    [StringLength(5)]
    public string Department { get; set; }
    [StringLength(2)]
    public string Course { get; set; }
    public int Grade { get; set; }
}
