from typing import Iterable
from os import path, system, chdir, getcwd
import subprocess
import sys

Path = str


def resolve_includes(source_file: Path) -> Iterable[str]:
    include_str = "!INCLUDE:"
    script_str = "!SCRIPT:"

    def is_include(line: str) -> bool:
        return line.startswith(include_str)

    def is_script(line: str) -> bool:
        return line.startswith(script_str)

    def extract_include_source(line: str) -> Path:
        return line[len(include_str):]

    for line in open(source_file, "r"):
        line = line.rstrip('\n')
        if is_include(line):
            include_source_file = extract_include_source(line)
            if not path.isabs(include_source_file):
                include_source_file = path.join(path.dirname(source_file), include_source_file)
            yield from resolve_includes(include_source_file)
        elif is_script(line):
            cwd = getcwd()
            chdir(path.dirname(path.abspath(source_file)))
            code=line[len(script_str):]
            print(subprocess.run(['sh', '-c', code], stdout=subprocess.PIPE).stdout.decode('utf-8'))
            chdir(cwd)
        else:
            yield line

if __name__ == '__main__':
    (_, source_file) = sys.argv
    for line in resolve_includes(source_file):
        print(line)

