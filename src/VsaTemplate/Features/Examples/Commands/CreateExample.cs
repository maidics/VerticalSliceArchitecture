using FluentValidation;
using VsaTemplate.Common.Interfaces;
using VsaTemplate.Common.Models;
using VsaTemplate.Database;

namespace VsaTemplate.Features.Examples.Commands;

public sealed record CreateExampleCommand(string Content) : IRequest;

public sealed class CreateExampleCommandValidator : AbstractValidator<CreateExampleCommand>
{
    public CreateExampleCommandValidator()
    {
        RuleFor(x => x.Content).NotEmpty();
    }
}

public sealed class CreateExampleCommandRequestHandler : IRequestHandler
{
    private readonly ApplicationDbContext _context;

    public CreateExampleCommandRequestHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(
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

        var example = new Examples.Example { Content = command.Content };

        await _context.Examples.AddAsync(example, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(example.Id);
    }
}
