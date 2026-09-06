using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Providers;

namespace FSH.Modules.Ai.Features.v1.Providers.DiscoverModels;

public sealed class DiscoverModelsCommandValidator : AbstractValidator<DiscoverModelsCommand>
{
    public DiscoverModelsCommandValidator()
    {
        RuleFor(x => x.BaseUrl)
            .NotEmpty()
            .Must(url => Uri.TryCreate(url?.Trim(), UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            .WithMessage("BaseUrl must be an absolute http(s) URL.")
            .MaximumLength(2048);

        RuleFor(x => x.ApiKey)
            .MaximumLength(4096)
            .When(x => x.ApiKey is not null);
    }
}
