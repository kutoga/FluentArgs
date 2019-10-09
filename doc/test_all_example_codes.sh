#!/bin/bash
set -e
cd "$(dirname "$0")"
find examples -type f -name '*.cs' | sort | xargs bash ./DummyProject/test_many_example_codes.sh

(cd DummyProject; dotnet add package System.Drawing.Common)
bash ./DummyProject/test_many_example_codes.sh ./gif-example/Program.cs

