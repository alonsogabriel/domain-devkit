using System.Diagnostics.CodeAnalysis;

namespace DomainDevKit.Application;

public class Result
{
    public const string DEFAULT_SUCCESS_MSG = "Result succeed!";
    public const string DEFAULT_FAILURE_MSG = "Result failed.";
    protected Result()
    {
        throw new InvalidOperationException();
    }
    protected Result(bool success, string message)
    {
        Success = success;
        Message = message;
    }
    public static Result Succeed(string? message = null)
    {
        return new(true, message ?? DEFAULT_SUCCESS_MSG);
    }
    public static Result<T> Succeed<T>(T data, string? message = null)
        where T : class
    {
        return Result<T>.Succeed(data, message);
    }
    public static Result Fail(string? message = null)
    {
        return new(false, message ?? DEFAULT_FAILURE_MSG);
    }
    public static Result<T> Fail<T>(string? message = null)
        where T : class
    {
        return Result<T>.Fail(message);
    }
    public virtual bool Success { get; protected init; }
    public string Message { get; protected init; }
}

public sealed class Result<T> : Result
    where T : class
{
    private Result() { }
    private Result(bool success, T? data, string message)
        : base(success, message)
    {
        Data = data;
    }
    public static Result<T> Succeed(T data, string? message = null)
    {
        return new(true, data, message ?? DEFAULT_SUCCESS_MSG);
    }
    public new static Result<T> Fail(string? message = null)
    {
        return new(false, null, message ?? DEFAULT_FAILURE_MSG);
    }
    public T? Data { get; private init; }

    [MemberNotNullWhen(true, nameof(Data))]
    public override bool Success { get; protected init; }
}