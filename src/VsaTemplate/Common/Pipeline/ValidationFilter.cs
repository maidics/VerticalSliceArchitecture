using FluentValidation;

namespace VsaTemplate.Common.Pipeline;

/* This makes sense if:
    - the request object is not created inside the minimal API method
    - CRUD operations that would take advantage of this:
        - Create: if there's a creation DTO (has to be validated: e.g.: registration) - without a DTO this is not required
        - Update: if there's an update DTO (has to be validated: e.g.: change FullName prop on profile) -
          even if one argument the request object should be passed in the HTTP body
        - Read: same pattern
        
    If you want to validate the request inside or after the Minimal API you have to inject it's validator and validate manually.
*/

//TODO: check if the framework will return negative HTTP response automatically if a query or route param is empty
public sealed class ValidationFilter<TRequest>(ILogger<ValidationFilter<TRequest>> logger)
    : IEndpointFilter
    where TRequest : notnull
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        var request = context.Arguments.OfType<TRequest>().FirstOrDefault();

        if (request is null)
        {
            throw new ArgumentException(
                $"Endpoint at '{context.HttpContext.Request.Path}' does not contain an argument of '{typeof(TRequest)}' type."
            );
        }

        var validators = context
            .HttpContext.RequestServices.GetServices<IValidator<TRequest>>()
            .ToList();

        if (validators.Count != 0)
        {
            var cancellationToken = context.HttpContext.RequestAborted;

            var validationResults = await Task.WhenAll(
                validators.Select(v =>
                    v.ValidateAsync(new ValidationContext<TRequest>(request), cancellationToken)
                )
            );

            var failures = validationResults
                .Where(r => !r.IsValid)
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Count != 0)
            {
                var errorsDictionary = failures
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

                logger.LogWarning(
                    "Validation failed for '{Path}' endpoint. Failures: {@ValidationErrors}",
                    context.HttpContext.Request.Path,
                    errorsDictionary
                );

                return TypedResults.ValidationProblem(errorsDictionary);
            }
        }

        return await next(context);
    }
}
