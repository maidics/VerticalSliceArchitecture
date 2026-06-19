using FluentValidation;
using VsaTemplate.Common.Interfaces.Features;
using VsaTemplate.Common.Models;
using VsaTemplate.Infrastructure.Database;

namespace VsaTemplate.Features.Examples.Commands;

public sealed record DeleteExampleCommand(string Id) : IRequest;

public sealed class DeleteExampleCommandValidator : AbstractValidator<DeleteExampleCommand>
{
    public DeleteExampleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public sealed class DeleteExampleCommandHandler : IRequestHandler
{
    private readonly ApplicationDbContext _context;

    public DeleteExampleCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(
        DeleteExampleCommand command,
        CancellationToken cancellationToken
    )
    {
        var example = await _context.Examples.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (example is null)
            return Result.NotFound(["Example not found."]);

        _context.Examples.Remove(example);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
