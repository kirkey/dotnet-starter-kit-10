using FSH.Module.Accounting.Contracts.v1.Payees;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Payees.GetPayee;

/// <summary>
/// Query to retrieve a single payee by ID.
/// </summary>
/// <param name="Id">Payee ID to retrieve</param>
public record GetPayeeQuery(Guid Id) : IQuery<PayeeDto>;