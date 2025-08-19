@echo off
echo 🎨 Starting Solankii AI Design Assistant...
echo.

echo 🔨 Building solution...
dotnet build --configuration Release
if %ERRORLEVEL% neq 0 (
    echo ❌ Build failed
    pause
    exit /b 1
)

echo.
echo 🚀 Starting API server...
start "Solankii API" cmd /k "dotnet run --project src/Solankii.API"

echo ⏳ Waiting for API to start...
timeout /t 5 /nobreak >nul

echo 🌐 Starting Web interface...
start "Solankii Web" cmd /k "dotnet run --project src/Solankii.Web"

echo.
echo ✅ Solankii is starting up!
echo 📱 API: http://localhost:5000
echo 📱 Swagger: http://localhost:5000/swagger
echo 📱 Web Interface: http://localhost:5001
echo.
echo 🛑 Close the command windows to stop the services
pause 