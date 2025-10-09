@echo off
setlocal

if not exist "ClashRoyale.Management.csproj" (
    echo 'ClashRoyale.Management.csproj' does not exist.
    echo Press any key to exit.
    timeout /t 5 >nul
    exit /b
)

start "" cmd /k "dotnet publish \"ClashRoyale.Management.csproj\" -c Release -o app & pause"
exit /b