﻿using Data.Interfaces;

namespace Data.Models;


public class RepositoryResult<T> : IRepositoryResult<T>
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public T? Data { get; set; } = default;


    public static RepositoryResult<T> Ok()
    {
        return new RepositoryResult<T>
        {
            Success = true,
            StatusCode = 200
        };
    }

    public static RepositoryResult<T> Ok(T data)
    {
        return new RepositoryResult<T>
        {
            Success = true,
            StatusCode = 200,
            Data = data
        };
    }

    public static RepositoryResult<T> Created(T data)
    {
        return new RepositoryResult<T>
        {
            Success = true,
            StatusCode = 201,
            Data = data
        };
    }

    public static RepositoryResult<T> NoContent()
    {
        return new RepositoryResult<T>
        {
            Success = true,
            StatusCode = 204
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
