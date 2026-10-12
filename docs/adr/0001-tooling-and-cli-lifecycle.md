# 1. Containerized tooling (Three Musketeers) and CLI exit behaviour

- Status: accepted

## Context

Two operational concerns. Contributors need a consistent, low-dependency way to
build, test, and run regardless of host setup. And the CLI is a long-running
REPL that must exit cleanly (status 0), not with a non-zero signal code.

## Decision

### Three Musketeers (Make + Docker + Compose)

Every task is a `make` target running inside a Docker Compose service, so the
only host requirements are Docker and `make`: `unit_test`, `integration_test`,
`build`, `run`, `image`, `run_image`.

The unit/integration split is driven by an xUnit trait filter
(`Category!=Integration` / `Category=Integration`) against the whole solution, so
the solution is the source of truth — adding a project needs no Makefile change.

Container builds redirect output off the host-mounted tree (via a root
`Directory.Build.props` using the SDK's `UseArtifactsOutput`, gated on an
`ARTIFACTS_PATH` env var set only in the compose service), so container and
host/IDE builds never clobber each other's `obj`/`bin`.

### CLI exit behaviour

Quitting relies on `Ctrl-C` and end-of-input (`Ctrl-D` / end of a piped file).
`Ctrl-C` cancels the default SIGINT kill (which would exit 130), unblocks the
read, prints the goodbye line, and exits 0.

## Consequences

- One entry point (`make <target>`) for every task; CI and local runs behave
  identically.
- New test projects are picked up by the trait filter automatically.
- Two clean ways to quit (Ctrl-C, EOF), both exit 0.

## Alternatives considered

- **Run tooling directly on the host.** Requires every contributor to install
  and match an SDK; drifts across machines. Rejected for the pinned,
  containerized toolchain.
- **Hand-list test projects per make target.** A maintenance trap when projects
  are added. Rejected — the trait filter makes the solution the source of truth.
- **Per-project `obj`/`bin` volumes, or hand-rolled `BaseIntermediateOutputPath`
  redirects.** The volume list needed a line per project; the hand-rolled path
  redirect broke with duplicate-`AssemblyInfo` (`CS0579`) errors. Rejected for
  the SDK's `UseArtifactsOutput`, which handles the compile-item exclusions.
