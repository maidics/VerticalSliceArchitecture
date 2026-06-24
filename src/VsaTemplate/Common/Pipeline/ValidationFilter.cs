using FluentValidation;
using VsaTemplate.Common.Interfaces;
using VsaTemplate.Common.Interfaces.Features;

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

public sealed class ValidationFilter : IEndpointFilter
{
    private readonly ILogger<ValidationFilter> _logger;
    private readonly IUser _user;

    public ValidationFilter(ILogger<ValidationFilter> logger, IUser user)
    {
        _logger = logger;
        _user = user;
    }

    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next
    )
    {
        var request = context.Arguments.OfType<IRequest>().FirstOrDefault();

        if (request is null)
        {
            return await next(context);
        }

        var type = request.GetType();
        var validatorType = typeof(IValidator<>).MakeGenericType(type);
        var validators = context
            .HttpContext.RequestServices.GetServices(validatorType)
            .Cast<IValidator>()
            .ToList();

        var validationContext = new ValidationContext<object>(request);

        if (validators.Count != 0)
        {
            var cancellationToken = context.HttpContext.RequestAborted;

            var validationResults = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(validationContext, cancellationToken))
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

                _logger.LogWarning(
                    "Request validation failed: {HttpMethod} {Path}, {@UserId}, {@ValidationErrors}",
                    context.HttpContext.Request.Method,
                    context.HttpContext.Request.Path.Value,
                    _user.Id,
                    errorsDictionary
                );

                return TypedResults.ValidationProblem(errorsDictionary);
            }
        }

        return await next(context);
    }
}
