namespace Data.Interfaces;

public interface IRepositoryResult
{
    bool Success { get; set; }
    int StatusCode { get; set; }
    string? ErrorMessage { get; set; }
}

public interface IRepositoryResult<T>
{
    bool Success { get; set; }
    int StatusCode { get; set; }
    T Result { get; set; }
}