@echo off
setlocal

if not exist "ClashRoyale.Simulator.csproj" (
    echo 'ClashRoyale.Simulator.csproj' does not exist.
    echo Press any key to exit.
    timeout /t 5 >nul
    exit /b
)

start "" cmd /k "dotnet publish \"ClashRoyale.Simulator.csproj\" -c Release -o app & pause"
exit /b