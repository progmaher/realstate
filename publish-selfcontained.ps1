# Real Estate Application - Multi-Platform Self-Contained Deployment
param(
    [string]$Runtime = "win-x64",
    [switch]$MultiPlatform = $false,
    [switch]$Help = $false
)

if ($Help) {
    Write-Host "======================================================" -ForegroundColor Cyan
    Write-Host "Real Estate Application - Self-Contained Deployment" -ForegroundColor Cyan
    Write-Host "======================================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Usage:" -ForegroundColor Yellow
    Write-Host "  .\publish-selfcontained.ps1                    # Publish for win-x64 (default)"
    Write-Host "  .\publish-selfcontained.ps1 -Runtime linux-x64 # Publish for specific runtime"
    Write-Host "  .\publish-selfcontained.ps1 -MultiPlatform     # Publish for all platforms"
    Write-Host ""
    Write-Host "Supported Runtimes:" -ForegroundColor Yellow
    Write-Host "  - win-x64       (Windows 64-bit)"
    Write-Host "  - linux-x64     (Linux 64-bit)"
    Write-Host "  - osx-x64       (macOS 64-bit)"
    Write-Host "  - win-arm64     (Windows ARM 64-bit)"
    Write-Host "  - linux-arm64   (Linux ARM 64-bit)"
    Write-Host ""
    exit 0
}

function Publish-Runtime {
    param($RuntimeId)
    
    $OutputDir = ".\publish\$RuntimeId"
    
    Write-Host "Publishing for $RuntimeId..." -ForegroundColor Green
    Write-Host "Output directory: $OutputDir" -ForegroundColor Gray
    
    # Clean previous build for this runtime
    if (Test-Path $OutputDir) {
        Remove-Item -Path $OutputDir -Recurse -Force
    }
    New-Item -Path $OutputDir -ItemType Directory -Force | Out-Null
    
    # Publish with self-contained deployment
    $publishArgs = @(
        "publish",
        "--configuration", "Release",
        "--runtime", $RuntimeId,
        "--self-contained", "true",
        "--output", $OutputDir,
        "--verbosity", "normal"
    )
    
    $process = Start-Process -FilePath "dotnet" -ArgumentList $publishArgs -Wait -PassThru -NoNewWindow
    
    if ($process.ExitCode -eq 0) {
        Write-Host "✅ Successfully published for $RuntimeId" -ForegroundColor Green
        Write-Host "📁 Output: $OutputDir" -ForegroundColor Gray
        
        # Calculate size
        $size = (Get-ChildItem -Path $OutputDir -Recurse | Measure-Object -Property Length -Sum).Sum
        $sizeInMB = [math]::Round($size / 1MB, 2)
        Write-Host "📊 Size: $sizeInMB MB" -ForegroundColor Gray
        Write-Host ""
    } else {
        Write-Host "❌ Failed to publish for $RuntimeId" -ForegroundColor Red
        Write-Host ""
        return $false
    }
    return $true
}

Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "Real Estate Application - Self-Contained Deployment" -ForegroundColor Cyan
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""

# Create publish directory
if (!(Test-Path "publish")) {
    New-Item -Path "publish" -ItemType Directory | Out-Null
}

# Clean previous builds
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean --configuration Release

# Restore dependencies
Write-Host "Restoring dependencies..." -ForegroundColor Yellow
dotnet restore

# Build the application
Write-Host "Building application..." -ForegroundColor Yellow
dotnet build --configuration Release --no-restore

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed. Exiting." -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Starting publishing..." -ForegroundColor Yellow
Write-Host ""

if ($MultiPlatform) {
    $runtimes = @("win-x64", "linux-x64", "osx-x64")
    $success = $true
    
    foreach ($runtime in $runtimes) {
        if (!(Publish-Runtime -RuntimeId $runtime)) {
            $success = $false
        }
    }
    
    if ($success) {
        Write-Host "======================================================" -ForegroundColor Green
        Write-Host "✅ Multi-platform publishing completed!" -ForegroundColor Green
        Write-Host "======================================================" -ForegroundColor Green
    }
} else {
    $success = Publish-Runtime -RuntimeId $Runtime
    
    if ($success) {
        Write-Host "======================================================" -ForegroundColor Green
        Write-Host "✅ Publishing completed for $Runtime!" -ForegroundColor Green
        Write-Host "======================================================" -ForegroundColor Green
    }
}

Write-Host ""
Write-Host "Available deployments:" -ForegroundColor Cyan
Get-ChildItem -Path ".\publish" -Directory | ForEach-Object {
    $size = (Get-ChildItem -Path $_.FullName -Recurse | Measure-Object -Property Length -Sum).Sum
    $sizeInMB = [math]::Round($size / 1MB, 2)
    Write-Host "  📁 $($_.Name) - $sizeInMB MB" -ForegroundColor Gray
}

Write-Host ""
Write-Host "Each deployment is self-contained and includes all required runtime dependencies." -ForegroundColor Gray
Write-Host "Copy the appropriate folder to your target server and run the executable." -ForegroundColor Gray
Write-Host ""