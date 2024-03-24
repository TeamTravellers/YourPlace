@echo off
cd "%~dp0YourPlace"
start "" "https://localhost:7017"  REM Open HTTPS URL in the default browser
dotnet run
