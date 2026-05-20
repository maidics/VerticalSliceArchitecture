using VsaTemplate.Common.Interfaces;
using VsaTemplate.Database;

namespace VsaTemplate.Features.Example.Queries;

public sealed class GetExamplesQueryHandler : IHandler
{
    private readonly ApplicationDbContext _context;

    public GetExamplesQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ExampleDto>> Handle(CancellationToken cancellationToken)
    {
        return await _context
            .Examples.AsNoTracking()
            .Select(x => new ExampleDto(x.Id, x.Content))
            .ToListAsync(cancellationToken);
    }
}
