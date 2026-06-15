using VsaTemplate.Common.Interfaces;
using VsaTemplate.Common.Interfaces.Features;
using VsaTemplate.Common.Models;
using VsaTemplate.Database;

namespace VsaTemplate.Features.Examples.Commands;

public sealed class DeleteExampleCommandRequestHandler : IRequestHandler
{
    private readonly ApplicationDbContext _context;

    public DeleteExampleCommandRequestHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(string exampleId, CancellationToken cancellationToken)
    {
        var example = await _context.Examples.FirstOrDefaultAsync(
            x => x.Id == exampleId,
            cancellationToken
        );

        if (example is null)
            return Result.NotFound(["Example not found."]);

        _context.Examples.Remove(example);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
