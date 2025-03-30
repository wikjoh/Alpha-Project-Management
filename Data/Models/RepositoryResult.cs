using Data.Interfaces;

namespace Data.Models;


public class RepositoryResult<T> : IRepositoryResult<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public T? Result { get; set; } = default;


    public static RepositoryResult<T> Ok()
    {
        return new RepositoryResult<T>
        {
            Success = true,
            StatusCode = 200
        };
    }

    public static RepositoryResult<T> Ok(T result)
    {
        return new RepositoryResult<T>
        {
            Success = true,
            StatusCode = 200,
            Result = result
        };
    }

    public static RepositoryResult<T> Created(T result)
    {
        return new RepositoryResult<T>
        {
            Success = true,
            StatusCode = 201,
            Result = result
        };
    }

    public static RepositoryResult<T> BadRequest(string errorMessage)
    {
        return new RepositoryResult<T>
        {
            Success = false,
            StatusCode = 400,
            ErrorMessage = errorMessage
        };
    }

    public static RepositoryResult<T> NotFound(string errorMessage)
    {
        return new RepositoryResult<T>
        {
            Success = false,
            StatusCode = 404,
            ErrorMessage = errorMessage
        };
    }

    public static RepositoryResult<T> AlreadyExists(string errorMessage)
    {
        return new RepositoryResult<T>
        {
            Success = false,
            StatusCode = 409,
            ErrorMessage = errorMessage
        };
    }

    public static RepositoryResult<T> InternalServerErrror(string errorMessage)
    {
        return new RepositoryResult<T>
        {
            Success = false,
            StatusCode = 500,
            ErrorMessage = errorMessage
        };
    }
}
