using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VsaTemplate.Common.Models;

namespace VsaTemplate.Common.Extensions;

public static class ResultExtensions
{
    extension<T>(Result<T> result)
    {
        public Results<Ok<T>, ProblemHttpResult> ToTypedResult()
        {
            if (result.Succeeded)
            {
                return TypedResults.Ok(result.Value);
            }

            return result.MapResultFailure();
        }
    }

    extension(Result result)
    {
        public Results<NoContent, ProblemHttpResult> ToTypedResult()
        {
            if (result.Succeeded)
            {
                return TypedResults.NoContent();
            }

            return result.MapResultFailure();
        }

        private ProblemHttpResult MapResultFailure()
        {
            return TypedResults.Problem(CreateProblemDetails(result));
        }
    }

    private static int GetStatusCode(this ResultType resultTypes)
    {
        return resultTypes switch
        {
            ResultType.NotFound => StatusCodes.Status404NotFound,
            ResultType.Conflict => StatusCodes.Status409Conflict,
            ResultType.ExternalServiceError => StatusCodes.Status503ServiceUnavailable,
            ResultType.BusinessError => StatusCodes.Status400BadRequest,
            ResultType.InternalError => StatusCodes.Status500InternalServerError,
            ResultType.PaymentRequired => StatusCodes.Status402PaymentRequired,
            ResultType.Unauthorized => StatusCodes.Status401Unauthorized,
            ResultType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,
        };
    }

    private static ProblemDetails CreateProblemDetails(Result result)
    {
        var problemDetails = new ProblemDetails
        {
            Status = result.Type.GetStatusCode(),
            Title = result.Type.ToString(),
        };

        if (result.Errors.Length != 0)
        {
            //RFC 7807 standard
            problemDetails.Extensions["errors"] = result.Errors;
        }

        return problemDetails;
    }
}
