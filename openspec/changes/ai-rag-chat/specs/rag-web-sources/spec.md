## Purpose

Lets tenants add public web links as chat knowledge without uploading files, using the same pipeline as documents.

## ADDED Requirements

### Requirement: Web links become ingested sources

A user SHALL be able to add a web link as a knowledge source; the system SHALL fetch the page, convert its readable content to Markdown, and ingest it through the same chunk pipeline as uploaded files.

#### Scenario: Successful link ingestion
- **WHEN** a user adds a reachable article URL
- **THEN** the system stores the fetched Markdown and marks the source ready for chat

#### Scenario: Unreachable or unsupported link
- **WHEN** a user adds a URL that cannot be fetched or yields no readable content
- **THEN** the system marks the source as failed with a reason and stores nothing searchable

### Requirement: Link sources refresh on demand

A user SHALL be able to re-fetch a web source to pick up page changes, producing a new Markdown snapshot that replaces the old chunks.

#### Scenario: Manual refresh
- **WHEN** a user refreshes a web source
- **THEN** chat answers afterwards ground in the new snapshot, not the old one
