# Migrate all seeder files from old structure to new vertical slice architecture

$sourcePath = "/Users/kirkeypsalms/Projects/ExternalProjects/src/api/modules/MicroFinance/MicroFinance.Infrastructure/Persistence/Seeders"
$targetPath = "/Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src/Modules/Microfinance/Modules.Microfinance/Data/Seeders"

# Create target directory
New-Item -ItemType Directory -Path $targetPath -Force | Out-Null

Write-Host "Migrating seeder files..." -ForegroundColor Cyan

# Get all seeder files
$seederFiles = Get-ChildItem -Path $sourcePath -Filter "*Seeder.cs"

$migrated = 0
foreach ($file in $seederFiles) {
    $content = Get-Content $file.FullName -Raw
    
    # Replace old namespaces with new ones
    $content = $content -replace 'using FSH\.Starter\.WebApi\.MicroFinance\.Domain\.Entities;', 'using FSH.Modules.Microfinance.Domain;'
    $content = $content -replace 'namespace FSH\.Starter\.WebApi\.MicroFinance\.Infrastructure\.Persistence\.Seeders;', 'namespace FSH.Modules.Microfinance.Data.Seeders;'
    
    # Replace MicroFinanceDbContext with Microfinance DbContext
    $content = $content -replace 'MicroFinanceDbContext', 'MicrofinanceDbContext'
    
    # Fix entity names to match new structure (they should be the same)
    # Most entities should already be correct, but let's ensure consistency
    
    # Write to new location
    $targetFile = Join-Path $targetPath $file.Name
    Set-Content -Path $targetFile -Value $content -NoNewline
    
    Write-Host "  ✓ Migrated: $($file.Name)" -ForegroundColor Green
    $migrated++
}

Write-Host "`n✅ Migrated $migrated seeder files successfully!" -ForegroundColor Cyan
Write-Host "Next: Update MicrofinanceDbInitializer to call all seeders"
