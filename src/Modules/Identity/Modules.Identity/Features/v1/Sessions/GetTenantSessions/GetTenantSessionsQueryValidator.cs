using FluentValidation;
using FSH.Modules.Identity.Contracts.v1.Sessions.GetTenantSessions;

namespace FSH.Modules.Identity.Features.v1.Sessions.GetTenantSessions;

public sealed class GetTenantSessionsQueryValidator : AbstractValidator<GetTenantSessionsQuery>
{
    public GetTenantSessionsQueryValidator()
    {
        // Bounds mirror the handler's clamp window (page < 1 → 1, size outside 1..200 → 50).
        RuleFor(q => q.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 200)
            .WithMessage("Page size must be between 1 and 200.");
    }
}
