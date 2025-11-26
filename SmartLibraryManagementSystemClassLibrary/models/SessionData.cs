using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Model;

public class SessionData
{
    [Key]
    [StringLength(128)]
    public string Key { get; set; }
    public string Value { get; set; }
    public DateTime Expiry { get; set; }

    public SessionData(string key, string value, DateTime expiry)
    {
        Key = key;
        Value = value;
        Expiry = expiry;
    }

    public void UpdateSessionData(string key, string value, DateTime expiry)
    {
        Key = key;
        Value = value;
        Expiry = expiry;
    }
}
