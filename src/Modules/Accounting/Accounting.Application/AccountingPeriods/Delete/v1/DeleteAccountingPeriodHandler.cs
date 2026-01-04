namespace Accounting.Application.AccountingPeriods.Delete.v1;

/// <summary>
/// Handler that deletes an accounting period after validating existence. Throws if period not found.
/// </summary>
public sealed class DeleteAccountingPeriodHandler(
    ILogger<DeleteAccountingPeriodHandler> logger,
    [FromKeyedServices("accounting:periods")] IRepository<AccountingPeriod> repository)
    : IRequestHandler<DeleteAccountingPeriodCommand>
{
    /// <summary>
    /// Deletes the specified accounting period.
    /// </summary>
    public async Task Handle(DeleteAccountingPeriodCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var period = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (period == null) throw new AccountingPeriodNotFoundException(request.Id);

        try
        {
            await repository.DeleteAsync(period, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
        {
            logger.LogWarning(ex, "Failed to delete accounting period {PeriodId} due to dependent records", request.Id);
            throw new BadRequestException("Cannot delete accounting period with dependent records. Remove or reassign dependent records first.");
        }
    }
}
