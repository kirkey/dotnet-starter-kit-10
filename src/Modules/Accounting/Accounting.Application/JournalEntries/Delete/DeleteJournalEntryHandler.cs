namespace Accounting.Application.JournalEntries.Delete;

public sealed class DeleteJournalEntryHandler(
    [FromKeyedServices("accounting:journals")] IRepository<JournalEntry> repository)
    : IRequestHandler<DeleteJournalEntryCommand>
{
    public async Task Handle(DeleteJournalEntryCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entry = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (entry == null) throw new JournalEntryNotFoundException(request.Id);

        // Guard: cannot delete posted/approved or reversed journal entries
        if (entry.Status == "Posted" || entry.Status == "Approved")
            throw new JournalEntryAlreadyPostedException(request.Id);

        if (entry.IsReversed)
            throw new JournalEntryCannotBeModifiedException(request.Id);

        await repository.DeleteAsync(entry, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
