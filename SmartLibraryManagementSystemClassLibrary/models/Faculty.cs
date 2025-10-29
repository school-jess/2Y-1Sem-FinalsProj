using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class Faculty
{
    [Key]
    public int FacultyId { get; set; }
    [StringLength(50)]
    public string FacultyName { get; set; }
    [StringLength(10)]
    public string Department { get; set; }
    [StringLength(20)]
    public string Subject { get; set; }
    [StringLength(2)]
    public string Course { get; set; }
    public User User { get; set; }
}
