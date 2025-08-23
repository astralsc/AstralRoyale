@echo off
setlocal
TITLE AstralRoyale: v1.0 (by: @fdz6 on GitHub)

if not exist "app" (
    echo 'app' does not exist.
    echo Please run 'Publish.bat' first.
    echo Press any key to exit.
    timeout /t 5 >nul
    exit /b
)

if not exist "app\ClashRoyale.dll" (
    echo 'ClashRoyale.dll' does not exist.
    echo Please run 'Publish.bat' first.
    echo Press any key to exit.
    timeout /t 5 >nul
    exit /b
)

echo Starting AstralRoyale...
dotnet app/ClashRoyale.dll