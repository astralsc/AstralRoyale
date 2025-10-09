@echo off
setlocal

if not exist "app" (
    echo 'app' does not exist.
    echo Please run 'Publish.bat' first.
    echo Press any key to exit.
    timeout /t 5 >nul
    exit /b
)

if not exist "app\ClashRoyale.Battles.dll" (
    echo 'ClashRoyale.Battles.dll' does not exist.
    echo Please run 'Publish.bat' first.
    echo Press any key to exit.
    timeout /t 5 >nul
    exit /b
)

dotnet app/ClashRoyale.Battles.dll