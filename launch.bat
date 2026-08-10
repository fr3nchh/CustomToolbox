@echo off
title Downiso
echo Building Downiso...
dotnet run --project src\CustomToolbox
if %errorlevel% neq 0 (
    echo.
    echo Build failed. Make sure .NET 8 SDK is installed.
    echo https://dotnet.microsoft.com/download
    pause
)
