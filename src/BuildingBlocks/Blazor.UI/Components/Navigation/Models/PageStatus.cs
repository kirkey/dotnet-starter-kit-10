namespace FSH.Framework.Blazor.UI.Components.Navigation.Models;

/// <summary>
/// Represents the development/implementation status of a page or feature.
/// </summary>
public enum PageStatus
{
    /// <summary>
    /// Feature is fully implemented and tested.
    /// </summary>
    Completed,

    /// <summary>
    /// Feature is currently being developed.
    /// </summary>
    InProgress,

    /// <summary>
    /// Feature is planned but not yet started.
    /// </summary>
    Planned,

    /// <summary>
    /// Feature status is not specified.
    /// </summary>
    None
}

