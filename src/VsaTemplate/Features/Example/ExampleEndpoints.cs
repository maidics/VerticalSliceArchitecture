using Microsoft.AspNetCore.Http.HttpResults;
using VsaTemplate.Common.Extensions;
using VsaTemplate.Common.Interfaces;
using VsaTemplate.Features.Example.Commands;
using VsaTemplate.Features.Example.Queries;

namespace VsaTemplate.Features.Example;

public class ExampleEndpoints : IEndpointGroup
{
    public static string RoutePrefix => "Examples";

    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost("", CreateExample);

        groupBuilder.MapGet("", GetExamples);

        groupBuilder.MapPut("", UpdateExample);

        groupBuilder.MapDelete("{exampleId:guid}", DeleteExample);

        groupBuilder.MapGet("{exampleId:guid}", GetExampleById);
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
        DeleteExampleCommandHandler handler,
        Guid exampleId,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(exampleId, cancellationToken);

        return result.ToTypedResult();
    }

    public static async Task<Results<Ok<ExampleDto>, ProblemHttpResult>> GetExampleById(
        GetExampleByIdQueryHandler handler,
        Guid exampleId,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.Handle(exampleId, cancellationToken);

        return result.ToTypedResult();
    }
}
