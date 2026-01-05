using FSH.Module.Accounting.Contracts.v1.Bills;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Bills.GetBill;

/// <summary>
/// Get Bill query.
/// </summary>
public record GetBillQuery(Guid Id) : IQuery<BillDto>;