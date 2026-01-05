using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.CreditMemos.ApproveCreditMemo;


/// <summary>
/// Command to approve a CreditMemo.
/// </summary>
/// <param name="Id">CreditMemo ID to approve</param>
public record ApproveCreditMemoCommand(Guid Id) : ICommand;