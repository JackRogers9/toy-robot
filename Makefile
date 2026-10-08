SHELL := /bin/bash
.ONESHELL:
.SILENT:

.PHONY:
build:
	docker compose build
	
.PHONY:
run:
	docker compose run --rm app
