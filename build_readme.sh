#!/bin/bash
set -e
cd "$(dirname "$0")"
echo "$(date)   Start building README.md..."
python3 ./doc/scripts/include_resolve.py ./doc/README.md.template > ./README.md
echo "$(date)   Built README.md..."

