# Local veritabanına migration uygulama script'i
# Kullanım: .\migration-update-local.ps1

Write-Host "🔄 Local veritabanına migration uygulanıyor..." -ForegroundColor Cyan

dotnet ef database update `
    --project DefenceDB.DAL `
    --startup-project DefenceDB.WebUI

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Migration başarıyla uygulandı! (Local)" -ForegroundColor Green
} else {
    Write-Host "❌ Migration uygulanamadı!" -ForegroundColor Red
    exit 1
}
