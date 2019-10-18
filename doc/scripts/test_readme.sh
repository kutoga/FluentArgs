#!/bin/bash
set -e
cd "$(dirname "$0")/../.."

mv README.md .README.md.org
mv doc/README_nuget.md doc/.README_nuget.md.org

echo "Build README..."
bash ./build_readme.sh

echo "Compare built README with existing README (their contet should be equivalent)..."
if ! diff README.md .README.md.org; then
    mv .README.md.org README.md
    echo "FAILED! README.md content does not seem to be up-to-date."
    echo "Please execute ./build_readme.sh"
    exit 1
else
    mv .README.md.org README.md
    echo "README.md is up-to-date!"
fi
if ! diff doc/README_nuget.md doc/.README_nuget.md.org; then
    mv doc/.README_nuget.md.org doc/README_nuget.md
    echo "FAILED! doc/README_nuget.md content does not seem to be up-to-date."
    echo "Please execute ./build_readme.sh"
    exit 1
else
    mv doc/.README_nuget.md.org doc/README_nuget.md
fi

