#!/bin/bash
set -e
cd "$(dirname "$0")"
python3 ./doc/scripts/include_resolve.py ./doc/README.md > ./README.md

