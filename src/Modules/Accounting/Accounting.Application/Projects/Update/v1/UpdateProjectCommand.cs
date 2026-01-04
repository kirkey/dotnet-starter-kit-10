using FSH.Framework.Core.Storage.File.Features;

namespace Accounting.Application.Projects.Update.v1;

/// <summary>
/// Command to update an existing project with validation and domain event publishing.
/// </summary>
/// <param name>The unique identifier of the project to update</param>
/// <param name>Updated project name</param>
/// <param name>Updated project start date</param>
/// <param name>Updated project end date (for completion/cancellation)</param>
/// <param name>Updated approved budget amount</param>
/// <param name>Updated project status (Active, Completed, On Hold, Cancelled)</param>
/// <param name>Updated client or customer name</param>
/// <param name>Updated project manager assignment</param>
/// <param name>Updated owning department</param>
/// <param name>Updated project description</param>
/// <param name>Updated project notes</param>
/// <param name>Updated image URL for the project</param>
public sealed record UpdateProjectCommand(
    DefaultIdType Id,
    string? Name,
    DateTime? StartDate,
    DateTime? EndDate,
    decimal? BudgetedAmount,
    string? Status,
    string? ClientName,
    string? ProjectManager,
    string? Department,
    string? Description,
    string? Notes,
    string? ImageUrl) : IRequest<UpdateProjectResponse>
{
    /// <summary>
    /// Optional image payload uploaded by the client. When provided, the image is uploaded to storage and ImageUrl is set from the saved file name.
    /// </summary>
    public FileUploadCommand? Image { get; init; }
}
