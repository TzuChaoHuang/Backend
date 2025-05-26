namespace Backend.Models.File;

public class UploadReq
{
    public bool IsPassword { get; set; }
    public string? Password { get; set; }
    public string? Description { get; set; }
}