SHELL := /bin/bash
.ONESHELL:
.SILENT:

DOTNET := docker compose run --rm dotnet dotnet

.PHONY:
restore:
	$(DOTNET) restore

.PHONY:
build:
	$(DOTNET) build

.PHONY:
test:
	$(DOTNET) test

.PHONY:
run:
	$(DOTNET) run --project src/ToyRobot.Cli

.PHONY:
image:
	docker compose build app

.PHONY:
run-image:
	docker compose run --rm app
