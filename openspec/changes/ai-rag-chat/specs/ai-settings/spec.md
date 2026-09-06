## Purpose

Gives operators one central place to configure AI providers, models, and secrets that ingestion and chat consume.

## ADDED Requirements

### Requirement: Providers and models are configurable

An operator SHALL be able to register AI providers, declare the models each serves, and set tenant defaults for chat and embeddings without redeploying.

#### Scenario: Provider setup
- **WHEN** an operator saves a provider with a model and a default designation
- **THEN** ingestion and chat offer that model from then on

### Requirement: Secrets are write-only

API keys and secrets SHALL be settable and replaceable but never readable back through any API or UI; listings SHALL show only configured/not-configured state.

#### Scenario: Key redaction
- **WHEN** an operator views provider settings after saving a key
- **THEN** the key value is never shown, only its presence

### Requirement: Misconfiguration fails loudly at use time

When no usable provider is configured for the requested capability, the operation SHALL fail with a clear error naming the missing piece instead of hanging or returning an empty answer.

#### Scenario: Missing provider
- **WHEN** a user chats with no chat model configured
- **THEN** they receive an explicit configuration error, not a silent failure

### Requirement: Provider models are auto-discovered

When an operator drafts a provider with an endpoint, protocol, and key, the system SHALL query the provider for its model catalog and present the candidates for selection; discovery SHALL write nothing, and only an explicit save SHALL persist the chosen models.

#### Scenario: Model discovery
- **WHEN** an operator enters a provider endpoint and key and requests discovery
- **THEN** the real model list from that provider is shown as selectable candidates

#### Scenario: Discovery writes nothing
- **WHEN** discovery runs
- **THEN** no stored provider, model, or secret changes until the operator explicitly saves
