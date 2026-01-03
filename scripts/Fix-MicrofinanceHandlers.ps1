# Fix all Create*Handler.cs files to use ICurrentUser properly

$handlersPath = "/Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src/Modules/Microfinance/Modules.Microfinance/Features"

Get-ChildItem -Path $handlersPath -Recurse -Filter "*Create*Handler.cs" | ForEach-Object {
    $file = $_.FullName
    $content = Get-Content $file -Raw
    
    # Skip if already properly configured
    if ($content -match 'ICurrentUser currentUser,\s+MicrofinanceDbContext context\)' -and 
        $content -notmatch 'ICurrentUser currentUser,\s+ICurrentUser') {
        return
    }
    
    # Add using statement if missing
    if ($content -notmatch 'using FSH\.Framework\.Core\.Context;') {
        $content = "using FSH.Framework.Core.Context;`n" + $content
    }
    
    # Fix duplicate ICurrentUser parameters
    $content = $content -replace 'ICurrentUser currentUser,\s*ICurrentUser currentUser,\s*ICurrentUser currentUser,', 'ICurrentUser currentUser,'
    $content = $content -replace 'ICurrentUser currentUser,\s*ICurrentUser currentUser,', 'ICurrentUser currentUser,'
    
    # Fix constructor - add ICurrentUser if missing
    if ($content -match 'public class (\w+)\(\s*MicrofinanceDbContext context\)') {
        $content = $content -replace '(public class \w+)\(\s*MicrofinanceDbContext context\)', '$1(ICurrentUser currentUser, MicrofinanceDbContext context)'
    }
    
    # Replace context usage with currentUser
    $content = $content -replace 'context\.TenantInfo\?\.Identifier \?\? "root"', 'currentUser.GetTenant() ?? "root"'
    $content = $content -replace 'context\.UserId', 'currentUser.GetUserId()'
    $content = $content -replace 'context\.UserName', 'currentUser.Name ?? "System"'
    
    Set-Content -Path $file -Value $content -NoNewline
    Write-Host "Fixed: $($_.Name)" -ForegroundColor Green
}

Write-Host "`nAll Create handlers fixed!" -ForegroundColor Cyan
