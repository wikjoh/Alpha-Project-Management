namespace Business.Models;

public abstract class ServiceResult<TResult, TData> where TResult : ServiceResult<TResult, TData>, new()
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public TData? Data { get; set; }


    public static TResult Ok()
    {
        return new TResult
        {
            Success = true,
            StatusCode = 200
        };
    }

    public static TResult Ok(TData data)
    {
        return new TResult
        {
            Success = true,
            StatusCode = 200,
            Data = data
        };
    }

    public static TResult Created(TData data)
    {
        return new TResult
        {
            Success = true,
            StatusCode = 201,
            Data = data
        };
    }

    public static TResult BadRequest(string errorMessage)
    {
        return new TResult
        {
            Success = false,
            StatusCode = 400,
            ErrorMessage = errorMessage
        };
    }

    public static TResult NotFound(string errorMessage)
    {
        return new TResult
        {
            Success = false,
            StatusCode = 404,
            ErrorMessage = errorMessage
        };
    }

    public static TResult AlreadyExists(string errorMessage)
    {
        return new TResult
        {
            Success = false,
            StatusCode = 409,
            ErrorMessage = errorMessage
        };
    }

    public static TResult InternalServerErrror(string errorMessage)
    {
        return new TResult
        {
            Success = false,
            StatusCode = 500,
            ErrorMessage = errorMessage
        };
    }
}