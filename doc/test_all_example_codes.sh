#!/bin/bash
set -e
cd "$(dirname "$0")"
find examples -type f -name '*.cs' | xargs bash ./DummyProject/test_many_example_codes.sh

