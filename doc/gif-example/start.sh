#!/bin/bash
clear
unbuffer python3 bash_remote.py | /usr/bin/stdbuf -i0 -o0 -e0 bash

