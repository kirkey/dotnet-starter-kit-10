## Purpose

Lets the chat surface AI agents available on the user's machine as selectable answer targets alongside models.

## ADDED Requirements

### Requirement: Local agents are discoverable

Where the deployment permits it, the system SHALL detect AI coding agents installed on the machine (their CLIs and credentials) and list them as available chat targets.

#### Scenario: Agent listing
- **WHEN** a user opens the chat target picker on a machine with agents installed
- **THEN** each detected agent appears with its name and availability

#### Scenario: No agents present
- **WHEN** no agents are detectable
- **THEN** the picker offers only configured models, with no error

### Requirement: Detection is explicit and safe

Detection SHALL only inspect well-known install locations and SHALL never execute agent binaries or read credential values; only presence and version metadata are reported.

#### Scenario: Safe probing
- **WHEN** detection runs
- **THEN** no agent process is launched and no secret value leaves the machine

### Requirement: Detections feed a runtime catalog

Each detection pass SHALL reconcile a runtime catalog recording every known agent-CLI family, its detected version, and its online/offline availability, so agent-to-runtime binding has a stable directory to reference.

#### Scenario: Runtime status
- **WHEN** a previously detected CLI disappears from the machine
- **THEN** its catalog entry flips to offline while its agents and history remain
