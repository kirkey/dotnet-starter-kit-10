namespace FSH.Framework.Blazor.UI.Components.Data.EntityTable;

/// <summary>
/// Validation component that displays server-side validation errors.
/// </summary>
public class FshValidation : ComponentBase
{
    private ValidationMessageStore? _messageStore;

    [CascadingParameter]
    private EditContext? CurrentEditContext { get; set; }

    protected override void OnInitialized()
    {
        if (CurrentEditContext is null)
        {
            throw new InvalidOperationException(
                $"{nameof(FshValidation)} requires a cascading " +
                $"parameter of type {nameof(EditContext)}. " +
                $"For example, you can use {nameof(FshValidation)} " +
                $"inside an {nameof(EditForm)}.");
        }

        _messageStore = new ValidationMessageStore(CurrentEditContext);

        CurrentEditContext.OnValidationRequested += (s, e) =>
            _messageStore?.Clear();
        CurrentEditContext.OnFieldChanged += (s, e) =>
            _messageStore?.Clear(e.FieldIdentifier);
    }

    public void DisplayErrors(IDictionary<string, ICollection<string>> errors)
    {
        if (CurrentEditContext is not null && errors is not null)
        {
            foreach (var err in errors)
            {
                _messageStore?.Add(CurrentEditContext.Field(err.Key), err.Value);
            }

            CurrentEditContext.NotifyValidationStateChanged();
        }
    }

    public void ClearErrors()
    {
        _messageStore?.Clear();
        CurrentEditContext?.NotifyValidationStateChanged();
    }
}
