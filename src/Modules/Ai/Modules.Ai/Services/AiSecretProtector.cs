using Microsoft.AspNetCore.DataProtection;

namespace FSH.Modules.Ai.Services;

/// <summary>
/// Encrypts/decrypts provider API keys at rest. Keys must be recoverable for outbound calls,
/// so Data Protection (not hashing) — same rationale as the Webhooks secret protector.
/// </summary>
public interface IAiSecretProtector
{
    string? Protect(string? plaintext);
    string? Unprotect(string? ciphertext);
}

public sealed class AiSecretProtector : IAiSecretProtector
{
    private readonly IDataProtector _protector;

    public AiSecretProtector(IDataProtectionProvider provider)
    {
        ArgumentNullException.ThrowIfNull(provider);
        _protector = provider.CreateProtector("FSH.Ai.ProviderApiKey.v1");
    }

    public string? Protect(string? plaintext) =>
        string.IsNullOrEmpty(plaintext) ? null : _protector.Protect(plaintext);

    public string? Unprotect(string? ciphertext) =>
        string.IsNullOrEmpty(ciphertext) ? null : _protector.Unprotect(ciphertext);
}
