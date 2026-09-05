## 1. Baseline and inventories

- [x] 1.1 Re-run the audit inventory queries and record current counts (awaits vs `ConfigureAwait(false)`, `catch (Exception)` sites, bare `IgnoreQueryFilters()`, `Map*` vs `RequirePermission`, POSTs vs `WithIdempotency`); verify `dotnet test src/Tests/Architecture.Tests` is green before any edit
- [x] 1.2 Produce the classified endpoint inventory (all 177 `Map*` sites → gated or anonymous-by-design with reason) and save it as a working note in the change folder; verify every site is classified with no orphans

## 2. Behavior-neutral fixes

- [x] 2.1 Add missing `.ConfigureAwait(false)` to awaits in module request paths, jobs, and hosted services; verify with zero non-conforming awaits outside tests/UI and a passing `test-unit` run
- [x] 2.2 Triage the 26 `catch (Exception)` sites into keep / narrow / remove per design Decision 4 (token-handler sites already verified compliant — keep); verify the global ProblemDetails mapping is unchanged and module unit suites pass
- [x] 2.3 Convert trash/restore queries that need only deleted rows to `IgnoreQueryFilters([QueryFilters.SoftDelete])` and prove an explicit tenant re-filter behind every remaining bare call; verify `Integration.Tests` passes (Docker)

## 3. Endpoint auth, idempotency, and test lock

- [x] 3.1 Gate every endpoint the inventory marked as missing-but-required with `.RequirePermission(...)` using the module's permission constants; verify the inventory shows zero unclassified sites
- [ ] 3.2 Add `.WithIdempotency()` to replay-safe POSTs after confirming the key-optional behavior for clients that send no key; verify double-submit creates no duplicates via handler/integration tests
- [x] 3.3 Add `Endpoints_Should_Be_Gated_Or_Allowlisted` to `Architecture.Tests` mirroring the KnownMissing pattern; verify it passes with the inventory's anonymous-by-design list

## 4. Missing validators (allowlist → zero)

- [x] 4.1 Billing invoices: add validators for `VoidInvoice`, `MarkInvoicePaid`, `IssueInvoice` commands and `GetMyInvoices`, `GetInvoices` queries; remove all five from the allowlists and verify the pairing tests pass
- [x] 4.2 Catalog products, categories, brands: add validators for the 6 command + 6 query handlers on the allowlists (`Delete/RestoreProduct`, `Delete/RestoreCategory`, `Delete/RestoreBrand`, search/list-trashed queries); remove them from the allowlists and verify the pairing tests pass
- [x] 4.3 Identity, Multitenancy, Tickets: add validators for `EnrollTwoFactor`, `EndImpersonation`, `RetryTenantProvisioning`, `ResetTenantTheme`, ticket `Restore/Resolve/Reopen/Assign`, `GetTenantSessions`, `SearchTickets`, `ListTrashedTickets`; remove them from the allowlists and verify the pairing tests pass
- [x] 4.4 For every validator added in 4.1–4.3, add invalid-input handler tests proving rejection via `ValidationBehavior` and record any newly-rejected input as a contract impact; verify allowlists are empty and full unit suites pass

## 5. Final verification

- [ ] 5.1 Run the full gate: `dotnet test src/Tests/Architecture.Tests`, `make test-unit`, and `make test-integration` (Docker); verify all green with no new skips or allowlist entries
- [ ] 5.2 Run `openspec validate --change api-best-practices-alignment` (add `--strict` if available) and fix any findings; verify the change reports planning-complete with all artifacts done
