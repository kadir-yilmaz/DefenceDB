# Migration oluşturma script'i
# Kullanım: .\migration-add.ps1 "MigrationAdi"

param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)

Write-Host "🚀 Migration oluşturuluyor: $MigrationName" -ForegroundColor Cyan

dotnet ef migrations add $MigrationName `
    --project DefenceDB.DAL `
    --startup-project DefenceDB.WebUI

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Migration başarıyla oluşturuldu!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Sonraki adımlar:" -ForegroundColor Yellow
    Write-Host "  1. Local'e uygulamak için: .\migration-update-local.ps1" -ForegroundColor White
    Write-Host "  2. Git push yap: git add . && git commit -m 'migration: $MigrationName' && git push" -ForegroundColor White
} else {
    Write-Host "❌ Migration oluşturulamadı!" -ForegroundColor Red
    exit 1
}
