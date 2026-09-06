## Purpose

Lets tenants ask questions in natural language and receive streamed chatbot answers grounded in their own sources.

## ADDED Requirements

### Requirement: Chat sessions answer from tenant sources

A user SHALL be able to open a chat session, send a prompt, and receive an answer composed from the most relevant chunks of their tenant's ready sources, delivered as a stream.

#### Scenario: Grounded answer
- **WHEN** a user asks a question covered by their sources
- **THEN** the answer streams in and cites the sources it used

#### Scenario: No covering sources
- **WHEN** a user asks something none of their sources cover
- **THEN** the system says so plainly instead of inventing an answer

### Requirement: Sessions persist per tenant

Chat sessions and their message history SHALL persist per tenant and be listable, reopenable, and deletable by their owner.

#### Scenario: Session history
- **WHEN** a user reopens a past session
- **THEN** the full message history is shown in order

### Requirement: Chat respects source scope

Answers SHALL only ever ground in the asking tenant's ready sources, never in another tenant's content.

#### Scenario: Cross-tenant isolation
- **WHEN** two tenants hold different sources and ask the same question
- **THEN** each answer grounds only in its own tenant's sources

### Requirement: Chat runs target an agent's model and variant

Each chat session SHALL run against an explicitly chosen agent target carrying a model plus variant tier, defaulting to the tenant default where the user picks none.

#### Scenario: Variant respected
- **WHEN** a user chats through an agent pinned to the high variant
- **THEN** the run uses that model and variant for every turn of the session
