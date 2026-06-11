using FluentValidation;
using VsaTemplate.Common.Interfaces;
using VsaTemplate.Common.Models;
using VsaTemplate.Database;

namespace VsaTemplate.Features.Examples.Commands;

public sealed record AppendExampleContentCommand(Guid ExampleId, string AdditionalContent)
    : IRequest;

public sealed class AppendExampleContentCommandValidator
    : AbstractValidator<AppendExampleContentCommand>
{
    public AppendExampleContentCommandValidator()
    {
        RuleFor(x => x.AdditionalContent).NotEmpty().WithMessage("Additional content is required.");
    }
}

public sealed class AppendExampleContentCommandHandler : IRequestHandler
{
    private readonly ApplicationDbContext _context;

    public AppendExampleContentCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(
        AppendExampleContentCommand command,
        CancellationToken cancellationToken
    )
    {
        var example = await _context.Examples.FirstOrDefaultAsync(
            x => x.Id == command.ExampleId,
            cancellationToken
        );

        if (example is null)
            return Result.NotFound(["Example not found."]);

        var existing = await _context
            .Examples.AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Content == example.Content + command.AdditionalContent,
                cancellationToken
            );

        if (existing is not null)
            return Result.Conflict([
                $"Example with '{example.Content + command.AdditionalContent}' content already exists.",
            ]);

        example.AppendContent(command.AdditionalContent);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
