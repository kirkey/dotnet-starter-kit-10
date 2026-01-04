using FluentValidation;
using FSH.Module.Identity.Contracts.v1.Roles.UpdatePermissions;

namespace FSH.Module.Identity.Features.v1.Roles.UpdateRolePermissions;

public class UpdatePermissionsCommandValidator : AbstractValidator<UpdatePermissionsCommand>
{
    public UpdatePermissionsCommandValidator()
    {
        RuleFor(r => r.RoleId)
            .NotEmpty();
        RuleFor(r => r.Permissions)
            .NotNull();
    }
}