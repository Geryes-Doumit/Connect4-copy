#!/bin/bash

# Empêcher les child processes de rester en vie après Ctrl+C
trap 'kill 0' EXIT

echo "Launching Connect4 API on https://localhost:6969..."
dotnet run --project Connect4.API --urls "https://localhost:6969" &

echo "Launching Connect4 UI on https://localhost:7213..."
dotnet run --project Connect4.UI --urls "https://localhost:7213" &

echo "Both processes are running. Press Ctrl+C to stop."

# On attend qu'ils se terminent (ou qu'on presse Ctrl+C)
wait
