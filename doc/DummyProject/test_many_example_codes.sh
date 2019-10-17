#!/bin/sh
set -e
for code_file in "$@"; do
    bash "$(dirname "$0")"/test_example_code.sh "$code_file"
done

