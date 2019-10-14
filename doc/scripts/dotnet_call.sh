#!/bin/bash
set -e
code_file="$(realpath "$1")"
args="${@:2}"

cd "$(dirname "$0")/../DummyProject/"
rm -f Program.cs
cp "$code_file" Program.cs
dotnet run -- $args 2>&1 | grep -v '/DummyProject.csproj'
