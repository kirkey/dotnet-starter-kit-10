using FluentValidation;
using FSH.Module.Identity.Contracts.v1.Roles.UpsertRole;

namespace FSH.Module.Identity.Features.v1.Roles.UpsertRole;

public class UpsertRoleCommandValidator : AbstractValidator<UpsertRoleCommand>
{
    public UpsertRoleCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Role name is required.");
    }
}