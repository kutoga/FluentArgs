from typing import Optional
import time

source_file = "Program.txt"

class bcolors:
    HEADER = '\033[95m'
    OKBLUE = '\033[94m'
    OKGREEN = '\033[92m'
    WARNING = '\033[93m'
    FAIL = '\033[91m'
    ENDC = '\033[0m'
    BOLD = '\033[1m'
    UNDERLINE = '\033[4m'

def exec_bash_command(command: str, hidden_append_str: Optional[str] = None) -> None:
    def echo_char_for_char(text: str) -> None:
        for char in text:
            print(f"echo -n '{char}'\n", end="", flush=True)
            if char != " " and char != "\t":
                time.sleep(0.05)
    print(f"echo -n {bcolors.OKGREEN}")
    echo_char_for_char("> ")
    print(f"echo -n {bcolors.ENDC}")
    print(f"echo -n {bcolors.OKBLUE}")
    echo_char_for_char(command)
    print(f"echo -n {bcolors.ENDC}")
    print("echo ''", flush=True)
    if hidden_append_str is not None:
        command += hidden_append_str
    print(command, flush=True)
    time.sleep(0.1)

def print_char_for_char(text: str) -> None:
    for char in text:
        print(char, end="", flush=True)
        if char != " " and char != "\t":
            time.sleep(0.05)

print("clear", flush=True)
time.sleep(1)

exec_bash_command("dotnet.exe new console", " --force")

print("rm -f Program.cs .Program.cs.sw*")
print("sed -i 's/netcoreapp3.0/netcoreapp2.2/g' *.csproj")

print("unbuffer ", end="")
exec_bash_command("vim Program.cs", " 2> /dev/null")
print_char_for_char("i")
time.sleep(0.5)

for line in open(source_file):
    print_char_for_char(line)
time.sleep(1)

print_char_for_char(chr(27))
print_char_for_char(":wq\n")

exec_bash_command("dotnet.exe run")

print("", flush=True)
time.sleep(1)
