using FSH.Module.Identity.Contracts.DTOs;
using Mediator;

namespace FSH.Module.Identity.Contracts.v1.Tokens.TokenGeneration;

public record GenerateTokenCommand(
    string Email,
    string Password)
    : ICommand<TokenResponse>;