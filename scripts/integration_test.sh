#!/usr/bin/env bash
set -eo pipefail

echo "Running integration tests..."
dotnet test --filter "Category=Integration" --logger "console;verbosity=minimal"
