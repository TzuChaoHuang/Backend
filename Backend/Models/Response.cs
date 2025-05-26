namespace Backend.Models;

public class Response
{
    public bool Success { get; set; }
    public string? Msg { get; set; }
    public object? Data { get; set; }

    public Response(bool success, string? msg = null, object? data = null)
    {
        Success = success;
        Msg = msg;
        Data = data;
    }
}