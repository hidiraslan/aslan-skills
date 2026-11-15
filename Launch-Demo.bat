@echo off
title Classic Aero UI Studio Launcher
cd /d "%~dp0"

echo ========================================================
echo   Classic Aero UI Studio - Windows Enterprise Design
echo ========================================================
echo.

:: 1. If compiled executable exists, launch it immediately
if exist "bin\Debug\net10.0-windows\ClassicAeroStudio.exe" (
    echo Starting Classic Aero UI Studio...
    start "" "bin\Debug\net10.0-windows\ClassicAeroStudio.exe"
    exit /b 0
)

:: 2. Otherwise, check for .NET SDK/Runtime and build/run
echo Binary not found. Building and running via .NET SDK...
where dotnet >nul 2>&1
if %ERRORLEVEL% neq 0 (
    echo.
    echo [ERROR] .NET SDK or Desktop Runtime was not found on your system.
    echo Please install the .NET 10 Desktop Runtime or SDK from:
    echo https://dotnet.microsoft.com/download
    echo.
    pause
    exit /b 1
)

dotnet run
if %ERRORLEVEL% neq 0 (
    echo.
    echo [ERROR] Application failed to start.
    pause
    exit /b %ERRORLEVEL%
)
