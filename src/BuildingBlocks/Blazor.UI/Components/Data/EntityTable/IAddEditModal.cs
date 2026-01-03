namespace FSH.Framework.Blazor.UI.Components.Data.EntityTable;

public interface IAddEditModal<TRequest>
{
    TRequest RequestModel { get; }
    bool IsCreate { get; }
    void ForceRender();
}
