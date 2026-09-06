using FluentValidation;

namespace FSH.Modules.Ai.Features.v1.Sources.AddWebSource;

public sealed class AddWebSourceCommandValidator : AbstractValidator<Contracts.v1.Sources.AddWebSourceCommand>
{
    public AddWebSourceCommandValidator()
    {
        RuleFor(x => x.Url)
            .NotEmpty()
            .Must(BeAbsoluteHttpUrl)
            .WithMessage("Url must be an absolute http(s) URL.")
            .MaximumLength(2048);

        RuleFor(x => x.Name)
            .MaximumLength(200)
            .When(x => x.Name is not null);
    }

    private static bool BeAbsoluteHttpUrl(string url) =>
        Uri.TryCreate(url?.Trim(), UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
}
