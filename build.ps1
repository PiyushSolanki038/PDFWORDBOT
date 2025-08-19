# Solankii Build Script
# This script helps build and run the Solankii AI Design Assistant

param(
    [switch]$Clean,
    [switch]$Build,
    [switch]$Run,
    [switch]$Test,
    [switch]$All
)

Write-Host "🎨 Solankii AI Design Assistant" -ForegroundColor Magenta
Write-Host "=================================" -ForegroundColor Magenta

# Set error action preference
$ErrorActionPreference = "Stop"

# Function to check if .NET 8 is installed
function Test-DotNet8 {
    try {
        $version = dotnet --version
        if ($version -like "8.*") {
            Write-Host "✅ .NET 8 found: $version" -ForegroundColor Green
            return $true
        } else {
            Write-Host "❌ .NET 8 not found. Current version: $version" -ForegroundColor Red
            Write-Host "Please install .NET 8 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
            return $false
        }
    } catch {
        Write-Host "❌ .NET SDK not found" -ForegroundColor Red
        Write-Host "Please install .NET 8 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Yellow
        return $false
    }
}

# Function to clean the solution
function Invoke-Clean {
    Write-Host "🧹 Cleaning solution..." -ForegroundColor Cyan
    dotnet clean
    if (Test-Path "bin") { Remove-Item -Recurse -Force "bin" }
    if (Test-Path "obj") { Remove-Item -Recurse -Force "obj" }
    Write-Host "✅ Clean completed" -ForegroundColor Green
}

# Function to build the solution
function Invoke-Build {
    Write-Host "🔨 Building solution..." -ForegroundColor Cyan
    dotnet build --configuration Release
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Build completed successfully" -ForegroundColor Green
    } else {
        Write-Host "❌ Build failed" -ForegroundColor Red
        exit 1
    }
}

# Function to run tests
function Invoke-Test {
    Write-Host "🧪 Running tests..." -ForegroundColor Cyan
    dotnet test --configuration Release --verbosity normal
    if ($LASTEXITCODE -eq 0) {
        Write-Host "✅ Tests completed successfully" -ForegroundColor Green
    } else {
        Write-Host "❌ Tests failed" -ForegroundColor Red
        exit 1
    }
}

# Function to run the application
function Invoke-Run {
    Write-Host "🚀 Starting Solankii..." -ForegroundColor Cyan
    Write-Host "📝 Note: Make sure to configure your OpenAI API key in appsettings.Development.json" -ForegroundColor Yellow
    
    # Check if configuration exists
    $configFile = "src/Solankii.API/appsettings.Development.json"
    if (-not (Test-Path $configFile)) {
        Write-Host "⚠️  Development configuration not found. Creating from template..." -ForegroundColor Yellow
        Copy-Item "src/Solankii.API/appsettings.json" $configFile
        Write-Host "📝 Please edit $configFile and add your OpenAI API key" -ForegroundColor Yellow
    }
    
    Write-Host "🌐 Starting API server..." -ForegroundColor Cyan
    Start-Process -FilePath "dotnet" -ArgumentList "run", "--project", "src/Solankii.API" -WindowStyle Normal
    
    Write-Host "⏳ Waiting for API to start..." -ForegroundColor Cyan
    Start-Sleep -Seconds 5
    
    Write-Host "🌐 Starting Web interface..." -ForegroundColor Cyan
    Start-Process -FilePath "dotnet" -ArgumentList "run", "--project", "src/Solankii.Web" -WindowStyle Normal
    
    Write-Host "✅ Solankii is starting up!" -ForegroundColor Green
    Write-Host "📱 API: http://localhost:5000" -ForegroundColor Cyan
    Write-Host "📱 Swagger: http://localhost:5000/swagger" -ForegroundColor Cyan
    Write-Host "📱 Web Interface: http://localhost:5001" -ForegroundColor Cyan
    Write-Host "🛑 Press Ctrl+C to stop all services" -ForegroundColor Yellow
}

# Main execution
try {
    # Check prerequisites
    if (-not (Test-DotNet8)) {
        exit 1
    }
    
    # Determine what to do based on parameters
    if ($All -or (-not $Clean -and -not $Build -and -not $Run -and -not $Test)) {
        # Default behavior: clean, build, test, run
        Invoke-Clean
        Invoke-Build
        Invoke-Test
        Invoke-Run
    } else {
        if ($Clean) { Invoke-Clean }
        if ($Build) { Invoke-Build }
        if ($Test) { Invoke-Test }
        if ($Run) { Invoke-Run }
    }
    
} catch {
    Write-Host "❌ An error occurred: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host "🎉 Solankii build script completed!" -ForegroundColor Green 