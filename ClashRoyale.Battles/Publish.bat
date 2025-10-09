@echo off
setlocal

if not exist "ClashRoyale.Battles.csproj" (
    echo 'ClashRoyale.Battles.csproj' does not exist.
    echo Press any key to exit.
    timeout /t 5 >nul
    exit /b
)

start "" cmd /k "dotnet publish \"ClashRoyale.Battles.csproj\" -c Release -o app & pause"
exit /b