namespace Domain.SharedKernel;

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public string? Error { get; }

    public object? Value { get; }

    protected Result(bool isSuccess, object? value = null, string? error = null)
    {
        if (isSuccess && !string.IsNullOrEmpty(error))
        {
            throw new InvalidOperationException("A successful result cannot have an error message.");
        }
        if (!isSuccess && string.IsNullOrWhiteSpace(error))
        {
            throw new InvalidOperationException("A failed result must have an error message.");
        }

        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result Success() => new Result(true);
    public static Result<T> Success<T>(T value) => new Result<T>(value, true);

    public static Result Failure(string error) => new Result(false, error: error);

    public static Result<T> Failure<T>(string error) => new Result<T>(default, false, error);


    public static Result Combine(params Result[] results)
    {
        var failedResults = results.Where(r => r.IsFailure).ToList();

        if (!failedResults.Any())
        {
            return Success();
        }

        var errors = failedResults.Select(r => r.Error).Distinct().ToArray();
        var combinedError = string.Join("; ", errors);

        return Failure(combinedError);
    }

    public static implicit operator Task<Result>(Result result) => Task.FromResult(result);
}

public class Result<T> : Result
{
    public new T Value => IsSuccess
        ? (T)base.Value!
        : throw new InvalidOperationException("Cannot access the value of a failed result.");

    protected internal Result(T? value, bool isSuccess, string? error = null)
        : base(isSuccess, value, error)
    {
        if (isSuccess && value == null && default(T) != null)
        {
            throw new InvalidOperationException("A successful result must have a non-null value for a non-nullable type T.");
        }
    }

    public static Result<T> Success(T value) => new Result<T>(value, true, string.Empty);


    public new static Result<T> Failure(string error) => new Result<T>(default, false, error);

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator T(Result<T> result) => result.Value;
}