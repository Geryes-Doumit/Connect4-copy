@echo off
echo Launching the Connect4 API and UI on Windows...

REM 1) Lancer l'API dans une nouvelle fenêtre
start "Connect4 API" dotnet run --project "Connect4.API" --urls "https://localhost:6969"

REM 2) Lancer l'UI dans une nouvelle fenêtre
start "Connect4 UI" dotnet run --project "Connect4.UI" --urls "https://localhost:7213"


echo Both windows have been started. Close them or press Ctrl+C here to exit this script.
pause
