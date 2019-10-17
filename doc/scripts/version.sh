#!/bin/bash
set -e
cd "$(dirname "$0")/../.."

major=$(cat azure-pipelines.yml | grep -e '^  major: ' | grep -oe '[0-9][0-9]*')
minor=$(cat azure-pipelines.yml | grep -e '^  minor: ' | grep -oe '[0-9][0-9]*')
patch=$(cat azure-pipelines.yml | grep -e '^  patch: ' | grep -oe '[0-9][0-9]*')

echo "${major}.${minor}.${patch}"

