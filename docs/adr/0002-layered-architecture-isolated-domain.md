# 2. Layered architecture with an isolated domain

- Status: accepted

## Context

The app has three concerns: movement rules (robot, table, geometry), command
handling (parse, execute), and delivery (console, configuration). Combining them
couples the rules to the console and makes the rules hard to test in isolation.

## Decision

Three projects, dependencies pointing strictly inward:

- `ToyRobot.Domain` — core model (`Robot`, `ITable`/`RectangularTable`,
  `Position`, `Direction`). Depends on nothing.
- `ToyRobot.Application` — command parsing/execution and the I/O abstractions
  (`IRobotInput`, `IRobotOutput`). Depends on `Domain`.
- `ToyRobot.Cli` — console host, configuration, composition root. Depends on
  `Application`.

The I/O abstractions live in `Application`; the concrete `Console`
implementations live in `Cli`, so the outer layer depends on inner-layer
abstractions, never the reverse.

## Consequences

- The domain is pure and unit-testable without standing up console, config, or
  I/O.
- Console and configuration are swappable without touching the rules.
- More ceremony than one project (three csprojs), justified by the testability
  it buys.

## Alternatives considered

- **Single flat project.** Simplest, but tangles rules with console/config and
  is far harder to test. Rejected — the split is cheap and pays off immediately.
