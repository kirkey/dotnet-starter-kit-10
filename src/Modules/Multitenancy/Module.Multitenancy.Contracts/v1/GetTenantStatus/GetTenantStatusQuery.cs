using FSH.Module.Multitenancy.Contracts.Dtos;
using Mediator;

namespace FSH.Module.Multitenancy.Contracts.v1.GetTenantStatus;

public sealed record GetTenantStatusQuery(string TenantId) : IQuery<TenantStatusDto>;

