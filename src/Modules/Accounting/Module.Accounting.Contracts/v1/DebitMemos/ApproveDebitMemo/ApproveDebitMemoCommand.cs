using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DebitMemos.ApproveDebitMemo;


/// <summary>
/// Command to approve a DebitMemo.
/// </summary>
/// <param name="Id">DebitMemo ID to approve</param>
public record ApproveDebitMemoCommand(Guid Id) : ICommand;