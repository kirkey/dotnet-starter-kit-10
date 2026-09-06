using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Runtimes;

namespace FSH.Modules.Ai.Features.v1.Runtimes.RefreshRuntimeCatalog;

public sealed class RefreshRuntimeCatalogCommandValidator : AbstractValidator<RefreshRuntimeCatalogCommand>
{
    public RefreshRuntimeCatalogCommandValidator()
    {
        // No fields to validate; the pairing test requires the validator to exist.
    }
}
