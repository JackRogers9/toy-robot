# 3. Integration tests run the real CLI as a child process

- Status: accepted

## Context

Beyond unit tests, we wanted end-to-end tests of the real deployable —
configuration, parser, executor, domain, and the console loop together — driven
as a user would: feed stdin, assert on stdout. Other IFH services use
Testcontainers for integration tests, so it was the obvious starting point.

## Decision

Integration tests run the published `ToyRobot.Cli` as a child process
(`System.Diagnostics.Process`), pipe commands to stdin, and assert on captured
stdout. No Docker/container lifecycle in the test code.

Helpers keep it single-responsibility: `CliProcessRunner` (launch + concurrent
stdout/stderr capture), `CliLocator` (find the built DLL via the referenced
assembly), `ReportOutput` (parse REPORT lines), `EnvironmentVariables` (map a
`TableOptions` to config env vars), composed by `IntegrationTestBase`. Tests are
tagged `[Trait("Category","Integration")]` so they run separately from unit
tests.

## Consequences

- Exercises the genuinely composed application end to end.
- No container lifecycle in test code; runs the same locally and in the `make`
  flow.
- Assumes the CLI is built (the project reference puts the DLL beside the test
  output).
- Tests the composed application, not the Docker image — the image is still
  exercised by `make image` / `make run-image`.

## Alternatives considered

- **Testcontainers.** Built for ephemeral networked dependencies with ports and
  wait strategies (why services use it for Solace/Valkey). A port-less,
  short-lived stdin/stdout CLI gets none of that value, and it would put
  container lifecycle back into test code. A spike confirmed the fit was poor;
  `docker run -i` via a process was what actually worked. Rejected.
- **Run the Docker image as the thing under test** (docker-out-of-docker).
  Worked, but puts container lifecycle in the test path. Dropped in favour of
  running the published app as a plain process.
- **Make it a long-running service and connect over the network** (the service-
  repo pattern). Rejected — would require inventing a server mode the spec does
  not need, purely to fit a test topology.
