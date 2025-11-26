using System.ComponentModel.DataAnnotations;

namespace SmartLibraryManagementSystemClassLibrary.Dtos;

public class SessionDataCreationDto
{
    [StringLength(128)] public string Key { get; set; }
    public string Value { get; set; }
    public DateTime Expiry { get; set; }

    public SessionDataCreationDto(string key, string value, DateTime expiry)
    {
        Key = key;
        Value = value;
        Expiry = expiry;
    }
}

public class SessionDataUpdateDto
{
    [StringLength(128)] public string Key { get; set; }
    public string Value { get; set; }
    public DateTime Expiry { get; set; }

    public SessionDataUpdateDto(string key, string value, DateTime expiry)
    {
        Key = key;
        Value = value;
        Expiry = expiry;
    }
}

public class SessionDataGet1Dto
{
    [StringLength(128)] public string Key { get; set; }
    public string Value { get; set; }
    public DateTime Expiry { get; set; }

    public SessionDataGet1Dto(string key, string value, DateTime expiry)
    {
        Key = key;
        Value = value;
        Expiry = expiry;
    }
}