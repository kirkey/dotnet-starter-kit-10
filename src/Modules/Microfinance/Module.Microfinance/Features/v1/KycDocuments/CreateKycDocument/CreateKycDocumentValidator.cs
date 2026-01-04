namespace FSH.Module.Microfinance.Features.v1.KycDocuments.CreateKycDocument;

public class CreateKycDocumentValidator : AbstractValidator<CreateKycDocumentCommand>
{
    public CreateKycDocumentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
