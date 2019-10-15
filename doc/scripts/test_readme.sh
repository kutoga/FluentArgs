#!/bin/bash
set -e
cd "$(dirname "$0")/../.."

mv README.md .README.md.org

echo "Build README..."
./build_readme.sh

echo "Compare built README with existing README (their contet should be equivalent)..."
if ! diff README.md .README.md.org; then
    mv .README.md.org README.md
    echo "FAILED! README content does not seem to be up-to-date."
    echo "Please execute ./build_readme.sh"
    exit 1
else
    mv .README.md.org README.md
    echo "Everything is fine :)"
fi

