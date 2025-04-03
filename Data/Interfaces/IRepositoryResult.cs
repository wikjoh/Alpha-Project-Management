using Data.Models;

namespace Data.Interfaces;

public interface IRepositoryResult<T>
{
    bool Success { get; set; }
    int StatusCode { get; set; }
    string? ErrorMessage { get; set; }
    T? Data { get; set; }

    static abstract RepositoryResult<T> Ok();
    static abstract RepositoryResult<T> Ok(T data);
    static abstract RepositoryResult<T> Created(T data);
    static abstract RepositoryResult<T> AlreadyExists(string errorMessage);
    static abstract RepositoryResult<T> BadRequest(string errorMessage);
    static abstract RepositoryResult<T> InternalServerErrror(string errorMessage);
    static abstract RepositoryResult<T> NotFound(string errorMessage);
    static abstract RepositoryResult<T> NoContent();
}