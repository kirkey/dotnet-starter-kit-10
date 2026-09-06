## Why

Tenants need to chat with their own content (uploaded files, web links) and operators need a central place to configure AI providers, models, and keys. Tenants further need department-level AI agents with their own tasks — including scheduled work like web monitoring with email delivery — plus selectable model variants and provider models discovered automatically. The DeepSeek harness (`deepseek-ai/deepseek-harness`, MIT) proves the UX and settings-plane patterns, and Multica (`multica-ai/multica`) proves the agent/task model (identity, runs, Autopilot schedules, runtime auto-detection) — but Multica's custom license bars hosted/SaaS use and UI reuse without a commercial license, so both platforms contribute concepts only, re-expressed in this repo's .NET/React idioms. Doing this as one coherent change avoids incompatible halves.

## What Changes

- New `Modules.Ai` bounded context (runtime + `.Contracts`): document ingestion (upload → Markdown conversion → dual storage of original + MD → chunk → embed), web-link sources (fetch → MD → same pipeline), RAG chat (retrieve → answer with citations), and an AI settings plane (providers, models, API keys, defaults).
- Dashboard (tenant) chat UI inspired by the harness web UX: sessions, streaming answers, cited sources.
- Tenant departments create and design their own agents (identity, instructions, skills, runtime, model plus variant tier, access), each running manual, scheduled/recurring, and event-driven tasks with a tracked lifecycle (e.g. a web-watch task that emails results).
- Provider settings auto-fetch the provider's model list so users pick real models; per-agent variant tiers (default, normal, high, extra-high) follow what the bound runtime supports.
- UI study spans the harness web UX and Multica's agent/task concepts; aioui (a Python asyncio demo) and evia.co (a Shopify shop) proved to be dead ends, not AI UI references.
- Honest reuse verdict on the harness (see design): 0% code reuse (TS→C#), high design reuse (settings/credentials plane, chat UX); MIT license requires attribution, recorded in the design. Multica contributes concepts only — its license forbids hosted use and UI reuse without a commercial license.

## Capabilities

### New Capabilities

- `rag-documents`: file upload → Markdown conversion → dual (original + MD) storage → chunking → embeddings.
- `rag-web-sources`: web links as RAG sources through the same ingestion pipeline.
- `rag-chat`: prompt/query chatbot (sessions, streaming, citations) over tenant sources.
- `ai-settings`: provider/model/key/default configuration plane with secret redaction.
- `agent-detection`: discovery of local AI agents for chat targeting.
- `ai-agents`: department agents (identity, instructions, skills, runtime, model+variant, access, archive/restore).
- `agent-tasks`: manual, scheduled/recurring, and event-driven agent runs with lifecycle tracking and delivery (e.g. web-watch→email).

### Modified Capabilities

(none — `openspec/specs/` is empty; no existing specs to change.)

## Impact

- Code: new `src/Modules/Ai/` (+ `.Contracts`), EF migrations (new module folder; vector support per design decision), dashboard pages (chat, agents, tasks, settings), possibly `AppHost.cs` (vector-capable Postgres image).
- Systems: new external calls to embedding/chat provider APIs (keys in settings plane); MinIO holds originals + MD twins; Hangfire gains recurring agent schedules; no changes to existing modules (Files upload flow is reused via events, not modified).
- Assumptions recorded: "ASI settings" read as AI settings; chat UI lives in the dashboard (tenant) app; provider settings UI in admin; agent detection targets the machine running the API in local/dev (see design for the SaaS caveat); departments are tenant sub-scopes, not a new tenancy layer; variants are effort tiers (default/normal/high/extra-high); aioui and evia.co yielded no usable UI reference.
