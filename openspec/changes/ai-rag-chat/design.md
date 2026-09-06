## Context

See `proposal.md` (Why). Verified repo facts shaping this design: the Files module already runs presigned-upload → finalize (the ingestion hook); the Chat module already models channels/messages (do not duplicate it without cause); storage is MinIO/S3 presigned; background work runs on Hangfire; cross-module communication goes through Contracts/events; there is **no** LLM/embedding client, **no** pgvector reference, and the Aspire Postgres image has no vector extension (local dev PG is 16.3, also without it). The harness (`deepseek-ai/deepseek-harness`, MIT, TypeScript/Cordis, `npx dsh web` on :3080) contributes a settings/credentials plane (layered values, redacted secrets, write-only keys) and a session-based chat UX worth studying — not code worth porting.

## Goals / Non-Goals

**Goals:**
- Answer the feasibility questions directly (reuse verdict, RAG options, agent-detection scope) so apply never re-litigates them.
- One new bounded context, minimal additive touches elsewhere by approval (event fields, `FileType.Document` rules); tenant isolation by construction.

**Non-Goals:**
- No line-for-line port of harness code; no fork/vendor of the harness repo.
- No training/fine-tuning, no autonomous agent execution — chat answers and optional agent targeting only.

## Decisions

1. **Harness reuse verdict: 0% code, high design reuse.** TypeScript/Cordis cannot run in (or be meaningfully transpiled to) this .NET backend, so 90–99% reuse is not possible as code. What transfers: the settings/credentials plane semantics (layered config, redacted listings, write-only secrets, revision-guarded writes), the session/chat UX shape, and provider-selection policy (explicit id or unambiguous single default). MIT license requires keeping DeepSeek's copyright notice wherever design text is closely adapted — record it in the new module's docs.
2. **New `Modules.Ai` bounded context** (runtime + `.Contracts`, four-place registration, per-module migration folder). Alternative (split Rag/Chat/Settings modules) rejected: one registration cost, one migration folder, and the three areas share the source/chunk/embedding model. The existing Chat module is NOT reused for RAG chat (its domain is channel messaging with membership semantics; RAG sessions need source-scoped grounding) — but its SignalR/SSE realtime pattern is.
3. **Vectors: pgvector-first (recommended), Qdrant as documented fallback.** pgvector keeps vectors in the module migration with tenant-filtered rows and zero new infrastructure; cost is switching the Aspire Postgres image to a pgvector variant and creating the extension per database (local dev PG 16.3 needs the extension installed or developers use the Aspire container). Qdrant sidecar wins only past ~millions of chunks or advanced filtering — recorded as the escape hatch, not phase one. Embeddings come from the configured provider's embedding API. Dimension policy (decided 1.2, refined for the HNSW 2000-dimension float cap): `halfvec(3072)` with cosine distance (`halfvec_cosine_ops`, HNSW) — the widest common provider dimension at half precision; smaller embeddings are zero-padded, which preserves cosine ordering for unit-norm vectors. Verified: `Pgvector.EntityFrameworkCore` 0.3.0 restores/builds clean against the repo's Npgsql 10.0.1, and an EF nearest-neighbor query over halfvec translates and returns correctly.
4. **Ingestion reuses the Files flow via events, with one additive event extension.** Finalize completion publishes the integration event; the Ai module subscribes, runs text-extraction → Markdown → chunk → embed as a Hangfire job chain, and stores the MD twin beside the original in MinIO. The event carries `StorageKey` + `OriginalFileName` (additive optional fields; old messages deserialize with nulls, which the handler tolerates as a failed ingestion with reason) so the handler can download bytes without a cross-module lookup. Web links enter the same chain after a fetch step. Phase-one formats: markdown/text natively, PDF/DOCX via libraries (deferred to a follow-up; currently recorded conversion-failed per the spec); anything else records conversion-failed per the spec.
5. **Settings plane mirrors the harness semantics.** Tenant-scoped providers/models/defaults; listings redact secrets (presence only); writes require revision match to avoid lost updates; a request with no usable provider fails loudly naming the missing piece. Settings UI in the admin app; chat UI in the dashboard app.
6. **Agent detection is local-and-explicit.** A server-hosted API cannot see a user's laptop: detection probes well-known install locations, never executes binaries or reads secret values, and only runs where the deployment permits (local/dev first). Treat any "fleet-wide agent inventory" ask as out of scope until agents self-register.
7. **Multica contributes concepts only — license bar.** Multica's agent/task/Autopilot/runtime model maps closely to departments, scheduled runs, variants-as-thinking-levels, and CLI auto-detection, but its custom license forbids hosted/SaaS use and UI reuse without a commercial license. Study the docs, re-express the patterns; port zero code and zero UI. (Contrast: the harness is MIT — attribution suffices.)
8. **Departments are tenant sub-scopes, variants are effort tiers.** No new tenancy layer: a department is an organizational grouping inside a tenant that owns agents. Variant tiers (default, normal, high, extra-high) mirror thinking-level overrides; unsupported model+variant pairs are rejected at save time against the bound runtime's matrix.
9. **Scheduled runs are Hangfire recurring jobs.** The repo already runs Hangfire: Autopilot-style cron/webhook triggers, per-run lifecycle history, and delivery (e.g. email) compose from existing pieces. Quiet runs record history only.
10. **Model discovery is draft-first.** Like the harness `discoverModels`: endpoint + protocol + key from the unsaved form → candidate list → explicit save persists. Discovery writes nothing and never stores the draft key.

## Risks / Trade-offs

- [pgvector image/extension churn across dev, Aspire, and deploy Postgres] → Mitigation: extension creation lives in the module migration (runs everywhere migrations run); document the local-PG prerequisite.
- [Embedding dimension locked at migration time vs later model swap] → Mitigation: fix a max dimension with padding/truncation policy decided in the dimension task; record the policy in settings docs.
- [Provider API costs/latency on every chat turn] → Mitigation: per-tenant defaults + streaming so partial answers render early; quotas ride the existing quota system.
- [Harness UX imitation drifts into copy-paste of TS idioms] → Mitigation: study-then-adapt task requires re-expression in Radix/Tailwind dashboard patterns with a reviewer check.
- [Multica-inspired code or UI slips in] → Mitigation: concepts only, enforced at review against the license bar; any closely adapted text carries attribution and stays clear of hosted/UI reuse.
- [Variant matrix explodes across runtimes] → Mitigation: variants validated at save time per runtime; unsupported pairs fail fast with the supported list.
- [Scheduled runs leak credentials or spam recipients] → Mitigation: schedules store references, never secrets; quiet runs write history only; email delivery reuses the mailing service with tenant sender identity.

## Migration Plan

Additive: new module projects, new migration folder, new dashboard pages, new settings endpoints. No changes to existing tables, routes, or configs. Rollback is revert (plus dropping the Ai tables/bucket prefix). Provider keys live only in the settings store, never in code or logs.
