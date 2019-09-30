import time

source_file = "Program.txt"

def exec_bash_command(command: str) -> None:
    for char in f"> {command}":
        print(f"echo -n '{char}'\n", end="", flush=True)
        if char != " " and char != "\t":
            time.sleep(0.05)
    print("echo ''", flush=True)
    print(command, flush=True)

def print_char_for_char(text: str) -> None:
    for char in text:
        print(char, end="", flush=True)
        if char != " " and char != "\t":
            time.sleep(0.05)

exec_bash_command("dotnet.exe new console --force")

print("rm -f Program.cs")

print("unbuffer ", end="")
exec_bash_command("vim Program.cs 2> /dev/null\n")
print_char_for_char("i")
time.sleep(0.5)

for line in open(source_file):
    print_char_for_char(line)
time.sleep(1)

print_char_for_char(chr(27))
print_char_for_char(":wq\n")

exec_bash_command("dotnet.exe run")

