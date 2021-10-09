#!/bin/bash
tput smcup

# Script for bash (ubuntu) to play the game with keeping your previous output.

function cleanup {
    tput rmcup
    # Your cleanup code here
}
dotnet run
trap cleanup EXIT