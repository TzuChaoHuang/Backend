namespace Backend.Models.Form;

public class CreateReq
{
    public bool IsPassword { get; set; }
    public string? Password { get; set; }
    public string? Description { get; set; }
    public int ExpiredDate { get; set; }
    public string[] FileNames { get; set; } = [];
}