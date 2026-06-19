using System.Collections.Frozen;

namespace VsaTemplate.Common.Models;

public class Result
{
    protected Result(bool succeeded, IEnumerable<string> errors, ResultType type)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
        Type = type;
    }

    public bool Succeeded { get; }
    public string[] Errors { get; }
    public ResultType Type { get; }

    public static Result Success(ResultType type = ResultType.Success)
    {
        if (type is ResultType.Redirect)
            throw new InvalidOperationException(
                $"'{ResultType.Redirect}' must be used with {nameof(Result)}<T>"
            );

        ThrowIfFailureType(type);

        return new Result(true, [], type);
    }

    public static Result<T> Success<T>(T value, ResultType successType = ResultType.Success)
    {
        // method will check values
        return Result<T>.Success(value, successType);
    }

    public static ResultFailure Failure(ResultType failureType, IEnumerable<string> errors)
    {
        return new ResultFailure(failureType, errors);
    }

    public static ResultFailure NotFound(IEnumerable<string> errors)
    {
        return new ResultFailure(ResultType.NotFound, errors);
    }

    public static ResultFailure Conflict(IEnumerable<string> errors)
    {
        return new ResultFailure(ResultType.Conflict, errors);
    }

    public static ResultFailure ExternalServiceError(IEnumerable<string> errors)
    {
        return new ResultFailure(ResultType.ExternalServiceError, errors);
    }

    public static ResultFailure BusinessError(IEnumerable<string> errors)
    {
        return new ResultFailure(ResultType.BusinessError, errors);
    }

    public static ResultFailure InternalError(IEnumerable<string> errors)
    {
        return new ResultFailure(ResultType.InternalError, errors);
    }

    public static ResultFailure PaymentRequired(IEnumerable<string> errors)
    {
        return new ResultFailure(ResultType.PaymentRequired, errors);
    }

    public static ResultFailure Unauthorized(IEnumerable<string> errors)
    {
        return new ResultFailure(ResultType.Unauthorized, errors);
    }

    public static ResultFailure Forbidden(IEnumerable<string> errors)
    {
        return new ResultFailure(ResultType.Forbidden, errors);
    }

    public static ResultFailure Cancelled(IEnumerable<string> errors)
    {
        return new ResultFailure(ResultType.Cancelled, errors);
    }

    // IMPLICIT CONVERSION: ResultFailure => Result
    public static implicit operator Result(ResultFailure failure)
    {
        return new Result(false, failure.Errors, failure.Type);
    }

    public static FrozenSet<ResultType> SuccessTypes => [ResultType.Success, ResultType.Redirect];

    protected static void ThrowIfFailureType(ResultType type)
    {
        if (!SuccessTypes.Contains(type))
            throw new InvalidOperationException($"'{type}' is not a success type.");
    }
}

public class Result<T> : Result
{
    private Result(bool succeeded, IEnumerable<string> errors, ResultType type, T value)
        : base(succeeded, errors, type)
    {
        Value = value;
    }

    public T Value
    {
        get
        {
            if (!Succeeded)
            {
                throw new InvalidOperationException("Failed result does not have inner value.");
            }

            return field;
        }
    }

    public static implicit operator Result<T>(ResultFailure failure)
    {
        return new Result<T>(false, failure.Errors, failure.Type, default!);
    }

    public static Result<T> Success(T value, ResultType successType = ResultType.Success)
    {
        ThrowIfFailureType(successType);

        ArgumentNullException.ThrowIfNull(value);
        return new Result<T>(true, [], ResultType.Success, value);
    }

    public Result<TOther> ToFailure<TOther>()
    {
        if (Succeeded)
        {
            throw new InvalidOperationException(
                "Cannot convert to new Result failure when Result is succeeded."
            );
        }

        return new Result<TOther>(succeeded: false, errors: Errors, value: default!, type: Type);
    }
}

public class ResultFailure
{
    public ResultType Type { get; }
    public string[] Errors { get; }

    public ResultFailure(ResultType failureType, IEnumerable<string> errors)
    {
        ThrowIfSuccessType(failureType);

        Type = failureType;
        Errors = errors.ToArray();
    }

    private static void ThrowIfSuccessType(ResultType type)
    {
        if (Result.SuccessTypes.Contains(type))
            throw new InvalidOperationException($"'{type}' is not a failure type.");
    }
}

public enum ResultType
{
    Success, //Ok, NoContent
    Redirect,

    Cancelled,
    NotFound,
    Conflict,
    ExternalServiceError, //TypedResults.Problem
    BusinessError, //BadRequest
    InternalError, //internal server error
    PaymentRequired,
    Unauthorized,
    Forbidden,
}
