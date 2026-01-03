param(
    [string]$SpecUrl = "https://localhost:7030/openapi/v1.json",
    [string]$App = "all"  # all, playground, or basic
)

$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Resolve-Path (Join-Path $scriptDir ".." "..")

Write-Host "Ensuring dotnet local tools are restored..." -ForegroundColor Cyan
dotnet tool restore | Out-Host

# Define apps configuration
$appsToGenerate = @()

if ($App -eq "all" -or $App -eq "playground") {
    $appsToGenerate += @{
        Name = "Playground"
        ConfigPath = Join-Path $scriptDir "nswag-playground.json"
        OutputDir = Join-Path $repoRoot "src/Playground/Playground.Blazor/ApiClient"
    }
}

if ($App -eq "all" -or $App -eq "basic") {
    $appsToGenerate += @{
        Name = "Basic"
        ConfigPath = Join-Path $scriptDir "nswag-basic.json"
        OutputDir = Join-Path $repoRoot "src/Apps/Basic/Basic.Blazor/Api"
    }
}

# Generate clients for each app
foreach ($appConfig in $appsToGenerate) {
    Write-Host "Generating API client for $($appConfig.Name) from spec: $SpecUrl" -ForegroundColor Cyan
    New-Item -ItemType Directory -Force -Path $appConfig.OutputDir | Out-Null
    dotnet nswag run $appConfig.ConfigPath /variables:SpecUrl=$SpecUrl
    Write-Host "Done. Generated client for $($appConfig.Name) in $($appConfig.OutputDir)" -ForegroundColor Green
}

Write-Host "All done!" -ForegroundColor Green
