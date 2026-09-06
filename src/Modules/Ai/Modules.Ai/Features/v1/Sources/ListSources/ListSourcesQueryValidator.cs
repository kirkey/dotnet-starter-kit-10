using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Sources;

namespace FSH.Modules.Ai.Features.v1.Sources.ListSources;

public sealed class ListSourcesQueryValidator : AbstractValidator<ListSourcesQuery>
{
    public ListSourcesQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .When(x => x.PageNumber.HasValue);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 200)
            .When(x => x.PageSize.HasValue);

        RuleFor(x => x.Search)
            .MaximumLength(200)
            .When(x => x.Search is not null);
    }
}
