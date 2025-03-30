using Data.Interfaces;

namespace Data.Models;


public class RepositoryResult : IRepositoryResult
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }


    public static RepositoryResult Ok()
    {
        return new RepositoryResult
        {
            Success = true,
            StatusCode = 200
        };
    }

    public static RepositoryResult BadRequest(string errorMessage)
    {
        return new RepositoryResult
        {
            Success = false,
            StatusCode = 400,
            ErrorMessage = errorMessage
        };
    }

    public static RepositoryResult NotFound(string errorMessage)
    {
        return new RepositoryResult
        {
            Success = false,
            StatusCode = 404,
            ErrorMessage = errorMessage
        };
    }

    public static RepositoryResult AlreadyExists(string errorMessage)
    {
        return new RepositoryResult
        {
            Success = false,
            StatusCode = 409,
            ErrorMessage = errorMessage
        };
    }

    public static RepositoryResult InternalServerErrror(string errorMessage)
    {
        return new RepositoryResult
        {
            Success = false,
            StatusCode = 500,
            ErrorMessage = errorMessage
        };
    }
}


public class RepositoryResult<T> : RepositoryResult, IRepositoryResult<T>
{
    public required T Result { get; set; }

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
}
