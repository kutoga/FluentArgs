from typing import Optional
import os
import time

source_file = "Program.cs"

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
    print(f"echo -n {bcolors.OKGREEN}", flush=True)
    echo_char_for_char("> ")
    print(f"echo -n {bcolors.ENDC}", flush=True)
    print(f"echo -n {bcolors.OKBLUE}", flush=True)
    echo_char_for_char(command)
    print(f"echo -n {bcolors.ENDC}", flush=True)
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

print("rm -Rf png2jpeg; mkdir png2jpeg; cd png2jpeg", flush=True)
exec_bash_command("dotnet.exe new console", " --force")

print("sed -i 's/netcoreapp3.0/netcoreapp2.2/g' *.csproj", flush=True)
print("rm Program.cs", flush=True)

print("unbuffer ", end="")
exec_bash_command("vim Program.cs", " 2> /dev/null")
print(":redraw", flush=True)
time.sleep(1)
print_char_for_char("i")
time.sleep(2)

for line in open(source_file):
    print_char_for_char(line)
time.sleep(2)

print_char_for_char(chr(27))
print_char_for_char(":wq\n")

exec_bash_command("dotnet.exe add package System.Drawing.Common")
exec_bash_command("dotnet.exe add reference ../../../source/FluentArgs/FluentArgs.csproj")
#exec_bash_command("dotnet.exe build -c Release")
exec_bash_command("dotnet.exe publish -c Release -r win10-x64")
print("cp ../1x1.png bin/Release/netcoreapp2.2/win10-x64/publish/image.png", flush=True)
exec_bash_command("cd bin/Release/netcoreapp2.2/win10-x64/publish")

exec_bash_command("dotnet.exe png2jpeg.dll")
time.sleep(2)
exec_bash_command("dotnet.exe png2jpeg.dll -h")
time.sleep(2)
exec_bash_command("dotnet.exe png2jpeg.dll --input image.png --output image.jpeg")
time.sleep(2)
exec_bash_command("dotnet.exe png2jpeg.dll -i image.png -o image.jpeg")
time.sleep(2)
exec_bash_command("dotnet.exe png2jpeg.dll -i image.png -o image.jpeg --quality 20")
time.sleep(2)
exec_bash_command("dotnet.exe png2jpeg.dll -i image.png -o image.jpeg --quality 100 -v")
time.sleep(2)

print("", flush=True)
time.sleep(1)
