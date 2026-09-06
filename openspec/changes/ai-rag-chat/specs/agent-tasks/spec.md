## Purpose

Lets agents do scheduled and on-demand work with a tracked lifecycle, including web monitoring with email delivery.

## ADDED Requirements

### Requirement: Runs have a tracked lifecycle

Every agent run SHALL move through queued → running → completed/failed with timestamps, and failures SHALL record the cause and retry or stop per policy.

#### Scenario: Failed run surfaces cause
- **WHEN** a run fails
- **THEN** its history shows the error, the retry outcome, and it never fails silently

### Requirement: Scheduled and recurring runs

A user SHALL be able to schedule agent work on a cron expression (e.g. a web-watch task that emails results), with run history kept per schedule.

#### Scenario: Web-watch email task
- **WHEN** a scheduled web-watch run detects changes
- **THEN** it emails the configured recipients and records the delivery in run history

#### Scenario: Quiet run leaves no clutter
- **WHEN** a scheduled run finds nothing to report
- **THEN** it records only its run history without creating tickets or noise
