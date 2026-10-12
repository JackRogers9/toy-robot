# 4. Config-driven table with graceful fallback

- Status: accepted

## Context

The table is 5x5 by default, but driving it from configuration (size, and later
shape) lets the app be reconfigured without code changes and lets integration
tests exercise non-default boards through the real config pipeline. Bad or
missing values should degrade gracefully, not crash.

## Decision

- Table config is read from `appsettings.json` under `Table` (`Width`, `Height`,
  `Shape`), overridable by environment variables (`Table__Width`, …), no prefix.
- `TableFactory` (in `Cli`) maps `TableOptions` to an `ITable` — the single place
  that reads table config and selects the shape.
- Invalid/missing config **warns and falls back** rather than throwing:
  non-positive size → default 5x5; unrecognised shape → rectangular; each with a
  warning via `IRobotOutput`. An absent value is a silent, valid default.

## Consequences

- Table size/shape is reconfigurable without recompiling.
- Misconfiguration yields a clear warning and a working app, consistent across
  bad size and bad shape.
- `TableFactory` is the one growth point for future shapes (new `ITable` + new
  arm), keeping config-reading out of the domain.

## Alternatives considered

- **Bind `Shape` directly as an enum.** The config binder throws at bind time
  for an unrecognised enum value, defeating warn-and-fallback. Rejected — `Shape`
  is bound as a `string` and parsed tolerantly in the factory.
- **Throw on unknown shape/size.** Fail-fast is defensible but inconsistent with
  the lenient size handling. Rejected for warn-and-fallback.
- **Inline defaults on `TableOptions`.** Rejected to keep it a pure data holder;
  the fallback policy lives in `TableFactory`.
