@echo off
setlocal
TITLE AstralRoyale: v1.0 (by: @fdz6 on GitHub)

if not exist "ClashRoyale.csproj" (
    echo 'ClashRoyale.csproj' does not exist.
    echo Press any key to exit.
    timeout /t 5 >nul
    exit /b
)

start "" cmd /k "dotnet publish \"ClashRoyale.csproj\" -c Release -o app & pause"
exit /b