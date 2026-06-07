using Microsoft.AspNetCore.Http.HttpResults;
using VsaTemplate.Common.Extensions;
using VsaTemplate.Common.Interfaces;
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

        builder.MapGet("", GetExamples);

        builder.MapPut("", UpdateExample);

        builder.MapDelete("{exampleId:guid}", DeleteExample);

        builder.MapGet("{exampleId:guid}", GetExampleById);

        builder.MapPut("append-content", AppendExampleContent);
    }

    public static async Task<Results<Ok<Guid>, ProblemHttpResult>> CreateExample(
        CreateExampleCommandRequestHandler requestHandler,
        CreateExampleCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await requestHandler.Handle(command, cancellationToken);

        return result.ToTypedResult();
    }

    public static async Task<Ok<List<ExampleDto>>> GetExamples(
        GetExamplesQueryRequestHandler requestHandler,
        CancellationToken cancellationToken
    )
    {
        var result = await requestHandler.Handle(cancellationToken);

        return TypedResults.Ok(result);
    }

    public static async Task<Results<NoContent, ProblemHttpResult>> UpdateExample(
        UpdateExampleCommandRequestHandler requestHandler,
        UpdateExampleCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await requestHandler.Handle(command, cancellationToken);

        return result.ToTypedResult();
    }

    public static async Task<Results<NoContent, ProblemHttpResult>> DeleteExample(
        DeleteExampleCommandRequestHandler requestHandler,
        Guid exampleId,
        CancellationToken cancellationToken
    )
    {
        var result = await requestHandler.Handle(exampleId, cancellationToken);

        return result.ToTypedResult();
    }

    public static async Task<Results<Ok<ExampleDto>, ProblemHttpResult>> GetExampleById(
        GetExampleByIdQueryRequestHandler requestHandler,
        Guid exampleId,
        CancellationToken cancellationToken
    )
    {
        var result = await requestHandler.Handle(exampleId, cancellationToken);

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
