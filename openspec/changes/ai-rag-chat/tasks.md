## 1. Module scaffold and vector infrastructure

- [x] 1.1 Scaffold `Modules.Ai` runtime + `.Contracts` projects with `IModule`/`[assembly: FshModule]`, register in all four places (Api + DbMigrator Mediator assemblies and module arrays), and verify the API boots with the module loaded
- [x] 1.2 Decide embedding dimensions and distance policy, switch the Aspire Postgres image to a pgvector variant, create the extension in the module migration, and verify `list-pending`/`apply` runs clean plus a vector round-trip test (verified: `InitialAi` applies clean on pgvector:pg18 with cosine round-trip correct; `Pgvector.EntityFrameworkCore` 0.3.0 compatible with Npgsql 10.0.1; homebrew-PG16 dev DBs need the extension installed or must use the Aspire container — see AGENTS.md)
- [x] 1.3 Define the source/chunk/embedding entities with tenant isolation and per-module snapshot, and verify `Architecture.Tests` stays green

## 2. Ingestion (files + links → Markdown twins → chunks → embeddings)

- [x] 2.1 Subscribe to the Files finalize integration event and implement extract-to-Markdown plus dual MinIO storage (original + MD twin), and verify an uploaded txt/md file yields both objects and a ready source (verified live on pgvector fshai21: txt → Ready + byte-identical twin `uploads/aisource/*.md`; png → Failed with reason; event extended additively with StorageKey/OriginalFileName; FileType.Document rules added; unique (SourceRef,TenantId) index)
- [x] 2.2 Implement web-link fetch → readable-Markdown ingestion into the same pipeline with failure reasons, and verify a reachable URL becomes searchable while a dead URL records failure without stored chunks (verified live: example.com → Ready + twin; dead URL → Failed with DNS reason; AddWebSource/Refresh/ListSources endpoints; Ai.Tests 25 green)
- [x] 2.3 Implement chunking + embedding via the configured provider as a Hangfire job chain with retry, and verify a source reaches ready state end to end plus source deletion removes chunks and twins (verified live: txt → Ready + embedded chunk via local-hash-3072; DELETE removed row+chunks+twin; fixed job tenant-scope pattern + pgvector resolver on shared connections)

## 3. AI settings plane

- [x] 3.1 Implement provider/model/default CRUD with write-only secrets, redacted listings, and revision-guarded writes, and verify keys are never readable via API and stale revisions conflict (first ICommand in the change: re-add the Ai Contracts Mediator marker in both Programs per the MSG0007 comment) (verified live: CRUD + 409 on stale revision + HasApiKey-only listings + default switching; fixed nav-collection tracking trap with explicit DbSet row management)
- [x] 3.2 Implement provider selection (explicit id or unambiguous single default, loud failure otherwise) consumed by ingestion and chat, and verify misconfiguration returns the documented explicit error (verified live: no-provider chat → 503 naming the removed local provider)

## 4. RAG chat API and UI

- [x] 4.1 Implement chat sessions/messages endpoints (retrieve top chunks tenant-scoped → stream answer with citations → persist history), and verify a grounded answer cites sources while an uncovered question gets the honest fallback (verified live: JSON + SSE answers with citations, honest fallback, ordered history, owner-scoped)
- [x] 4.2 Build the dashboard chat pages (sessions list, streaming thread, citations, target picker) adapted from the harness UX study to Radix/Tailwind patterns, and verify route-mocked Playwright coverage (verified: 3 Playwright tests green + lint clean)
- [x] 4.3 Implement opt-in local agent detection (well-known paths only, no execution, no secret reads) surfaced as chat targets, and verify detection lists installed agents and degrades cleanly to models-only (verified live: 5 CLI families detected with credential presence, no execution; picker lists them with availability)

## 5. Verification

- [x] 5.1 Run the full gate (`Architecture.Tests`, `make test-unit`, `make test-integration`) and the live double-submit + cross-tenant isolation probes; verify all green withvalidator pairing holding for every new handler (verified: 12 unit projects + 746 integration + 5 middleware green; double-submit creates no dupes; acme sees zero root sources and gets honest fallback)
- [x] 5.2 Run `openspec validate ai-rag-chat --strict` and fix any findings; verify planning-complete with the MIT attribution recorded (valid; attribution + persistence lessons in `src/Modules/Ai/README.md`)

## 6. Department agents

- [x] 6.1 Implement agent CRUD (identity, instructions, skills, runtime binding, model+variant with save-time matrix check, access, archive/restore without copying secrets), and verify an archived agent takes no runs while history survives (verified live: CRUD + 400 matrix rejection + 403 access gate + 409 archived + picker exclusion + copy + guarded dept delete)
- [x] 6.2 Build the dashboard agent pages (list with online/workload status, designer, access editor), and verify route-mocked Playwright coverage (verified: 3 Playwright tests green)

## 7. Scheduled and recurring tasks

- [x] 7.1 Implement manual/scheduled/webhook agent runs on Hangfire with queued → running → completed/failed history, retries, and email delivery, and verify a web-watch schedule emails on change while a quiet run writes history only (verified live: manual/prompt runs, web-watch change + quiet rerun, anonymous webhook w/ token, rotate, cancel; email best-effort via IMailService)
- [x] 7.2 Verify a failed run records its cause, retries per policy, and never fails silently (verified live: dead-URL run retried 3x → Failed with cause, RetryCount=2)

## 8. Variants, discovery, and runtime catalog

- [x] 8.1 Implement the runtime catalog (known CLI families, versions, online/offline) backing detection and agent binding, and verify a removed CLI flips to offline without losing agents or history (verified live: refresh → 5 online + local-model offline; agents intact; unit-tested reconcile logic)
- [x] 8.2 Implement draft-first provider model discovery (candidates listed, nothing persisted until explicit save), and verify the draft key is never stored (verified live against a stub /models endpoint: candidates returned, provider/secret counts unchanged, draft key absent)
