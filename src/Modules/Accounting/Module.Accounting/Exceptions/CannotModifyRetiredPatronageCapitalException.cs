using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Accounting.Domain.Exceptions;

/// <summary>
/// Exception thrown when attempting to modify retired patronage capital.
/// </summary>
public class CannotModifyRetiredPatronageCapitalException : BadRequestException
{
    public CannotModifyRetiredPatronageCapitalException(Guid patronageCapitalId)
        : base($"Cannot modify retired patronage capital with ID {patronageCapitalId}")
    {
    }
}
