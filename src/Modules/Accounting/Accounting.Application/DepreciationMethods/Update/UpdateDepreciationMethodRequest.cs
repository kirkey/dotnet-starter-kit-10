namespace Accounting.Application.DepreciationMethods.Update;

public record UpdateDepreciationMethodRequest(
    DefaultIdType Id,
    string? MethodCode,
    string? MethodName,
    string? CalculationFormula,
    bool IsActive,
    string? Description,
    string? Notes) : IRequest<DefaultIdType>;
