#!/bin/bash
set -e
cd "$(dirname "$0")"

echo "$(date)   Start building README.md..."
python3 ./doc/scripts/include_resolve.py ./doc/README.md.template > ./README.md
echo "$(date)   Built README.md..."

echo "$(date)   Start building README_nuget.md..."
python3 ./doc/scripts/include_resolve.py ./doc/README_nuget.md.template > ./doc/README_nuget.md
echo "$(date)   Built README_nuget.md..."

