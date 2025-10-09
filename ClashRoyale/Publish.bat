@echo off
setlocal

if not exist "ClashRoyale.csproj" (
    echo 'ClashRoyale.csproj' does not exist.
    echo Press any key to exit.
    timeout /t 5 >nul
    exit /b
)

start "" cmd /k "dotnet publish \"ClashRoyale.csproj\" -c Release -o app && copy /Y filter.json app\ && echo filter.json copied to app folder. & pause"
exit /b