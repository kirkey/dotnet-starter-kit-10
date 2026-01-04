namespace Accounting.Application.PrepaidExpenses.Queries;

/// <summary>
/// Specification to find prepaid expense by prepaid number.
/// </summary>
public class PrepaidExpenseByNumberSpec : Specification<PrepaidExpense>
{
    public PrepaidExpenseByNumberSpec(string prepaidNumber)
    {
        Query.Where(p => p.PrepaidNumber == prepaidNumber);
    }
}

/// <summary>
/// Specification to find prepaid expense by ID.
/// </summary>
public class PrepaidExpenseByIdSpec : Specification<PrepaidExpense>
{
    public PrepaidExpenseByIdSpec(DefaultIdType id)
    {
        Query.Where(p => p.Id == id);
    }
}

/// <summary>
/// Specification for searching prepaid expenses with filters.
/// </summary>
public class PrepaidExpenseSearchSpec : Specification<PrepaidExpense>
{
    public PrepaidExpenseSearchSpec(Search.v1.SearchPrepaidExpensesRequest request)
    {
        Query
            .Where(p => p.PrepaidNumber.Contains(request.PrepaidNumber!), !string.IsNullOrWhiteSpace(request.PrepaidNumber))
            .Where(p => p.Status == request.Status, !string.IsNullOrWhiteSpace(request.Status))
            .Where(p => p.IsFullyAmortized == request.IsFullyAmortized, request.IsFullyAmortized.HasValue)
            .Where(p => p.StartDate >= request.StartDateFrom, request.StartDateFrom.HasValue)
            .Where(p => p.StartDate <= request.StartDateTo, request.StartDateTo.HasValue)
            .Where(p => p.VendorId == request.VendorId, request.VendorId.HasValue);

        Query.OrderByDescending(p => p.StartDate).ThenBy(p => p.PrepaidNumber);
    }
}


