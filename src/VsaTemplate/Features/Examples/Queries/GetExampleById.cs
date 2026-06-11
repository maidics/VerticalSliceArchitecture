using VsaTemplate.Common.Interfaces;
using VsaTemplate.Common.Models;
using VsaTemplate.Database;

namespace VsaTemplate.Features.Examples.Queries;

public sealed class GetExampleByIdQueryRequestHandler : IRequestHandler
{
    private readonly ApplicationDbContext _context;

    public GetExampleByIdQueryRequestHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<ExampleDto>> Handle(
        Guid exampleId,
        CancellationToken cancellationToken
    )
    {
        var example = await _context
            .Examples.AsNoTracking()
            .Where(x => x.Id == exampleId)
            .Select(x => new ExampleDto(x.Id, x.Content))
            .FirstOrDefaultAsync(cancellationToken);

        if (example is null)
            return Result.NotFound(["Example not found."]);

        return Result.Success(example);
    }
}
