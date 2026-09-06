using FluentValidation;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Providers;
using FSH.Modules.Ai.Domain;

namespace FSH.Modules.Ai.Features.v1.Providers.UpdateProvider;

public sealed class UpdateProviderCommandValidator : AbstractValidator<UpdateProviderCommand>
{
    public UpdateProviderCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Revision).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);

        RuleFor(x => x.BaseUrl)
            .Must(BeAbsoluteHttpUrl)
            .When(x => !string.IsNullOrWhiteSpace(x.BaseUrl))
            .WithMessage("BaseUrl must be an absolute http(s) URL.")
            .MaximumLength(2048);

        RuleFor(x => x.ChatModel).MaximumLength(256);
        RuleFor(x => x.EmbeddingModel).MaximumLength(256);
        RuleFor(x => x.EmbeddingDimensions).InclusiveBetween(64, AiChunk.EmbeddingDimensions);

        RuleFor(x => x)
            .Must(c => c.Models.Count > 0 || !string.IsNullOrWhiteSpace(c.ChatModel) || !string.IsNullOrWhiteSpace(c.EmbeddingModel))
            .WithMessage("Declare at least one model: a chat model, an embedding model, or catalog entries.");

        RuleForEach(x => x.Models).ChildRules(m =>
        {
            m.RuleFor(x => x.ModelId).NotEmpty().MaximumLength(256);
            m.RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(256);
            m.RuleFor(x => x).Must(x => x.SupportsChat || x.SupportsEmbeddings)
                .WithMessage("A model must support at least one capability.");
        });
    }

    private static bool BeAbsoluteHttpUrl(string? url) =>
        Uri.TryCreate(url?.Trim(), UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
}
