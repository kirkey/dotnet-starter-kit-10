using FSH.Framework.Core.Storage.File.Features;

namespace Accounting.Application.Projects.Create.v1;

/// <summary>
/// Command to create a new project with budget tracking and job costing capabilities.
/// </summary>
/// <param name>Project name (required)</param>
/// <param name>Project start date</param>
/// <param name>Approved budget amount (must be non-negative)</param>
/// <param name>Optional client or customer name</param>
/// <param name>Optional project manager assignment</param>
/// <param name>Optional owning department</param>
/// <param name>Optional project description</param>
/// <param name>Optional project notes</param>
/// <param name>Optional image URL for the project</param>
public sealed record CreateProjectCommand(
    string Name,
    DateTime StartDate,
    decimal BudgetedAmount,
    string? ClientName,
    string? ProjectManager,
    string? Department,
    string? Description,
    string? Notes,
    string? ImageUrl) : IRequest<CreateProjectResponse>
{
    /// <summary>
    /// Optional image payload uploaded by the client. When provided, the image is uploaded to storage and ImageUrl is set from the saved file name.
    /// </summary>
    public FileUploadCommand? Image { get; init; }
}
