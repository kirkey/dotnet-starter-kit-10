# Modules.Ai — RAG chat, department agents, scheduled runs

Tenant-scoped knowledge Q&A over uploaded files and web links (Markdown twins, pgvector
chunks, provider or built-in local models), plus department agents with instructions, model
variants, access control, and Hangfire manual/scheduled/webhook runs with history.

## Design reuse attribution (MIT)

The settings/credentials plane semantics (layered config, redacted listings, write-only keys,
revision-guarded writes), the session/chat UX shape, and the provider-selection policy were
adapted from the design of the `deepseek-ai/deepseek-harness` project (MIT License,
© the deepseek-harness contributors). No harness code is vendored or ported; only the
interaction patterns were re-expressed for this stack. Multica concepts (agent/task model,
variants as thinking levels) were studied from documentation only — no Multica code or UI is
reused (its custom license forbids hosted/SaaS use without a commercial license).

## Persistence rules learned the hard way

- **DbContext captures its tenant at resolution.** Background code (Hangfire jobs, anonymous
  webhook handlers) must create a fresh scope, install the tenant from the tenant store, and
  resolve the DbContext *afterwards* — never reuse a context built under a null tenant.
- **Child adds on dirtied parents.** Adding to the navigation collection of an
  already-modified tracked parent mis-tracks the new rows as `Modified` (0-row concurrency
  failure on save). Either add children before dirtying the parent, mark fresh rows
  `EntityState.Added` explicitly, or manage rows via the DbSet (bulk delete + `AddRange`).
  `AiProvider.SetModels` is therefore for new aggregates only; updates manage rows explicitly.
- **Shared connections need the pgvector plugin.** `ScopedDbConnectionProvider` builds
  Postgres connections from a data source with the pgvector resolver factory, otherwise
  `HalfVector` parameters fail to serialize at insert time.
