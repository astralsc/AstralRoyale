@echo off
setlocal

if not exist "ClashRoyale.CsvConverter.csproj" (
    echo 'ClashRoyale.CsvConverter.csproj' does not exist.
    echo Press any key to exit.
    timeout /t 5 >nul
    exit /b
)

start "" cmd /k "dotnet publish \"ClashRoyale.CsvConverter.csproj\" -c Release -o app & pause"
exit /b