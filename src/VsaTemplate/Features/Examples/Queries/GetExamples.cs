using VsaTemplate.Common.Interfaces;
using VsaTemplate.Common.Interfaces.Features;
using VsaTemplate.Database;

namespace VsaTemplate.Features.Examples.Queries;

public sealed class GetExamplesQueryRequestHandler : IRequestHandler
{
    private readonly ApplicationDbContext _context;

    public GetExamplesQueryRequestHandler(ApplicationDbContext context)
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
