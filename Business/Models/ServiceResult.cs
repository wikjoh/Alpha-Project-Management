namespace Business.Models;

public abstract class ServiceResult
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string? Error { get; set; }
}
