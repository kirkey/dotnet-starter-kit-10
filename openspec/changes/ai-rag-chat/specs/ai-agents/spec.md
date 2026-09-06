## Purpose

Lets each tenant department create and operate its own AI agents with identity, skills, models, and access control.

## ADDED Requirements

### Requirement: Departments own agents

A department SHALL be able to create agents carrying a name, instructions, attached skills, a runtime binding, and a model plus variant; agents SHALL be editable, archivable/restorable, and duplicable with secrets never copied.

#### Scenario: Department creates an agent
- **WHEN** a department member with rights creates an agent with instructions and a model+variant
- **THEN** the agent appears in the picker and runs only for members granted access to it

#### Scenario: Agent archive
- **WHEN** an agent is archived
- **THEN** it disappears from pickers, takes no new runs, keeps history, and can be restored

### Requirement: Model variants are explicit effort tiers

Each agent SHALL declare a model plus a variant tier (default, normal, high, extra-high) subject to what the bound runtime supports; unsupported combinations SHALL be rejected at save time, never at run time.

#### Scenario: Unsupported variant rejected
- **WHEN** a user saves an agent with a variant its runtime does not support
- **THEN** the save fails with a message naming the supported variants
