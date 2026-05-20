using FluentValidation;
using VsaTemplate.Common.Interfaces;
using VsaTemplate.Common.Models;
using VsaTemplate.Database;

namespace VsaTemplate.Features.Example.Commands;

public sealed record CreateExampleCommand(string Content);

public sealed class CreateExampleCommandValidator : AbstractValidator<CreateExampleCommand>
{
    public CreateExampleCommandValidator()
    {
        RuleFor(x => x.Content).NotEmpty();
    }
}

public sealed class CreateExampleCommandHandler : IHandler
{
    private readonly ApplicationDbContext _context;

    public CreateExampleCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(
        CreateExampleCommand command,
        CancellationToken cancellationToken
    )
    {
        var existing = await _context.Examples.FirstOrDefaultAsync(
            x => x.Content == command.Content,
            cancellationToken
        );

        if (existing is not null)
            return Result.Conflict([$"Example already exists with content: {command.Content}"]);

        var example = new Example { Content = command.Content };

        await _context.Examples.AddAsync(example, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
