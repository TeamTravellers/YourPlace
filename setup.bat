@echo off
cd "%~dp0YourPlace/YourPlace"
start "" "https://localhost:7017"  REM Open HTTPS URL in the default browser
dotnet run
