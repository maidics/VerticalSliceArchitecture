using VsaTemplate.Common.Interfaces;
using VsaTemplate.Common.Models;
using VsaTemplate.Database;

namespace VsaTemplate.Features.Example.Queries;

public sealed class GetExampleByIdQueryHandler : IHandler
{
    private readonly ApplicationDbContext _context;

    public GetExampleByIdQueryHandler(ApplicationDbContext context)
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
            .Select(x => new ExampleDto(x.Id, x.Content))
            .FirstOrDefaultAsync(x => x.Id == exampleId, cancellationToken);

        if (example is null)
            return Result.NotFound(["Example not found."]);

        return Result.Success(example);
    }
}
