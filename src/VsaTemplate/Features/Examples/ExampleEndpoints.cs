using Microsoft.AspNetCore.Http.HttpResults;
using VsaTemplate.Common.Extensions;
using VsaTemplate.Common.Interfaces.Features;
using VsaTemplate.Features.Examples.Commands;
using VsaTemplate.Features.Examples.Queries;

namespace VsaTemplate.Features.Examples;

public class ExampleEndpoints : IEndpointGroup
{
    public static string Prefix => "Examples";
    public static string[] Tags => [Prefix];

    public static void Map(IEndpointRouteBuilder builder)
    {
        builder.MapPost("", CreateExample);

        builder.MapGet("all", GetExamples);

        builder.MapPut("", UpdateExample);

        builder.MapDelete("", DeleteExample);

        builder.MapGet("", GetExampleById);

        builder.MapPut("append-content", AppendExampleContent);
    }

    public static async Task<Results<Ok<Guid>, ProblemHttpResult>> CreateExample(
        CreateExampleCommandHandler handler,
        CreateExampleCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(command, cancellationToken);

        return result.ToTypedResult();
    }

    public static async Task<Ok<List<ExampleDto>>> GetExamples(
        GetExamplesQueryHandler handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(cancellationToken);

        return TypedResults.Ok(result);
    }

    public static async Task<Results<NoContent, ProblemHttpResult>> UpdateExample(
        UpdateExampleCommandHandler handler,
        UpdateExampleCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(command, cancellationToken);

        return result.ToTypedResult();
    }

    public static async Task<Results<NoContent, ProblemHttpResult>> DeleteExample(
        [AsParameters] DeleteExampleCommand command,
        DeleteExampleCommandHandler handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(command, cancellationToken);

        return result.ToTypedResult();
    }

    public static async Task<Results<Ok<ExampleDto>, ProblemHttpResult>> GetExampleById(
        [AsParameters] GetExampleByIdQuery query,
        GetExampleByIdQueryHandler handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(query, cancellationToken);

        return result.ToTypedResult();
    }

    public static async Task<Results<NoContent, ProblemHttpResult>> AppendExampleContent(
        AppendExampleContentCommandHandler handler,
        AppendExampleContentCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(command, cancellationToken);

        return result.ToTypedResult();
    }
}
