# Toy Robot

A console simulation of a toy robot on a 5x5 table. It reads commands from
standard input — `PLACE X,Y,F`, `MOVE`, `LEFT`, `RIGHT`, `REPORT` — and is
prevented from falling off the table.

## Project layout

- `src/ToyRobot.Domain` — core model (`Robot`, `ITable`/`RectangularTable`, `Position`, `Direction`).
- `src/ToyRobot.Application` — command parsing and execution (Command pattern).
- `src/ToyRobot.Cli` — console host, configuration, composition root.
- `tests/` — `*.Domain.Tests`, `*.Application.Tests` (unit) and `*.Integration.Tests` (end-to-end).
- `test-data/` — sample input files (see its README).

## Dependencies

Run everything either **locally** or **through `make`/Docker** — pick one.

- **Local:** .NET SDK 10.0 (targets `net10.0`, developed against `10.0.400`).
- **Docker/make:** Docker (with `docker compose`), `make`, and `bash` (the
  Makefile and `integration_test.sh` are bash). No local SDK needed — it runs in
  the `dotnet` compose service. Requires a repo-root `.env` defining
  `DOCKER_REPO` (already present).

> **Windows:** the application and tests are fully cross-platform via the local
> `dotnet` path below. The `make` path needs bash, so run it under WSL or Git
> Bash — or just use the `dotnet` commands directly.

### Artifactory authentication

Base images (`iress.jfrog.io/iress-docker`) and NuGet packages (`iress-nuget`)
come from Iress Artifactory, so authenticate first — otherwise image pulls and
`dotnet restore` fail. Open Artifactory via Okta → user menu (top right) →
**Set Me Up**, and follow the Docker and NuGet instructions.

## Running the tests

Unit and integration tests are tagged, so each command runs only its own set.

```bash
# make (Docker)
make unit_test            # Domain + Application
make integration_test     # end-to-end against the built CLI

# local (dotnet)
dotnet test --filter "Category!=Integration"  # unit
dotnet test --filter "Category=Integration"   # integration
dotnet test                                   # everything
```

## Running the CLI

Reads commands from stdin — type them, or pipe a file. Quit with `Ctrl-C` or
end-of-input (`Ctrl-D`).

```bash
# local (dotnet)
dotnet run --project src/ToyRobot.Cli                             # interactive
dotnet run --project src/ToyRobot.Cli < test-data/example-a.txt   # piped

# make (Docker)
make run         # interactive, in the dotnet SDK container
make image       # build the application image
make run_image   # run that image
```

Example: `PLACE 1,2,EAST / MOVE / MOVE / LEFT / MOVE / REPORT` → `3,3,NORTH`.
See `test-data/` for more inputs and expected outputs.

## Configuration

Table size and shape come from `src/ToyRobot.Cli/appsettings.json` (default: 5x5
rectangular) and can be overridden with env vars — `Table__Width`,
`Table__Height`, `Table__Shape`. Invalid values warn and fall back to the
defaults rather than failing.

`appsettings.json` is copied to the build output as `PreserveNewest`, so after
editing it rebuild (or `dotnet run`) to refresh the copy the binary reads;
running a stale build can otherwise pick up old config.
