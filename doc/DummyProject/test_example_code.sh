#!/bin/bash
set -e
code_file=$1
echo "Try to compile this example code file: $code_file"

dummy_project_dir="$(dirname "$0")"
rm -f "$dummy_project_dir/Program.cs"
cp $code_file "$dummy_project_dir/Program.cs"

cd "$dummy_project_dir"
dotnet build

