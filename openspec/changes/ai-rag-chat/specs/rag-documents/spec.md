## Purpose

Lets tenants turn their uploaded files into searchable Markdown knowledge that chat answers are grounded in.

## ADDED Requirements

### Requirement: Uploaded files produce a Markdown twin

Every file a user uploads for knowledge SHALL be converted to Markdown, and storage SHALL hold both the original file and its Markdown twin side by side.

#### Scenario: Successful conversion
- **WHEN** a user uploads a supported file to the knowledge library
- **THEN** the system stores the original and produces a Markdown twin linked to it

#### Scenario: Unsupported file type
- **WHEN** a user uploads a file type the converter does not support
- **THEN** the system keeps the original, marks the source as conversion-failed, and explains why

### Requirement: Converted sources become retrievable chunks

The Markdown twin of each successfully converted source SHALL be split into chunks suitable for similarity search, and each chunk SHALL carry its source reference.

#### Scenario: Source ready for chat
- **WHEN** conversion and chunking finish for a source
- **THEN** the source is marked ready and its chunks are searchable

### Requirement: Source lifecycle is tenant-scoped

Users SHALL only see, query, and delete knowledge sources belonging to their own tenant, and deleting a source SHALL remove its chunks and twins.

#### Scenario: Tenant isolation
- **WHEN** a user lists knowledge sources
- **THEN** only their tenant's sources appear

#### Scenario: Source deletion
- **WHEN** a user deletes a source
- **THEN** its chunks and stored twins are removed and chat no longer cites them
