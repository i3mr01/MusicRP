# PowerShell script to help trigger GitHub Actions and create release
# Run this script to help with the release process

Write-Host "🚀 i3mr0 MusicRP Release Helper" -ForegroundColor Green
Write-Host "=================================" -ForegroundColor Green

# Check if we're in a git repository
if (-not (Test-Path ".git")) {
    Write-Host "❌ Not in a git repository!" -ForegroundColor Red
    exit 1
}

# Check current status
Write-Host "📋 Current Git Status:" -ForegroundColor Yellow
git status --short

# Check if we have the tag
Write-Host "`n🏷️ Available Tags:" -ForegroundColor Yellow
git tag -l

# Check if tag is pushed
Write-Host "`n🔍 Checking if v1.0.0 tag is pushed..." -ForegroundColor Yellow
$tagExists = git ls-remote --tags origin | Select-String "v1.0.0"
if ($tagExists) {
    Write-Host "✅ Tag v1.0.0 is pushed to GitHub" -ForegroundColor Green
} else {
    Write-Host "❌ Tag v1.0.0 not found on GitHub" -ForegroundColor Red
    Write-Host "Pushing tag..." -ForegroundColor Yellow
    git push origin v1.0.0
}

Write-Host "`n📦 Release Files Available:" -ForegroundColor Yellow
if (Test-Path "i3mr0-MusicRP-v1.0.0-Release.zip") {
    Write-Host "✅ i3mr0-MusicRP-v1.0.0-Release.zip" -ForegroundColor Green
} else {
    Write-Host "❌ Release zip not found" -ForegroundColor Red
}

if (Test-Path "i3mr0-MusicRP-v1.0.0/") {
    Write-Host "✅ i3mr0-MusicRP-v1.0.0/ folder" -ForegroundColor Green
} else {
    Write-Host "❌ Release folder not found" -ForegroundColor Red
}

Write-Host "`n🌐 Next Steps:" -ForegroundColor Cyan
Write-Host "1. Go to: https://github.com/i3mr01/MusicRP" -ForegroundColor White
Write-Host "2. Click 'Releases' on the right side" -ForegroundColor White
Write-Host "3. Click 'Create a new release'" -ForegroundColor White
Write-Host "4. Select tag 'v1.0.0'" -ForegroundColor White
Write-Host "5. Upload the release files" -ForegroundColor White
Write-Host "6. Publish the release" -ForegroundColor White

Write-Host "`n📋 Release Files to Upload:" -ForegroundColor Cyan
Write-Host "- i3mr0-MusicRP-v1.0.0-Release.zip (main release)" -ForegroundColor White
Write-Host "- Or individual files from i3mr0-MusicRP-v1.0.0/ folder" -ForegroundColor White

Write-Host "`n🎯 GitHub Actions Check:" -ForegroundColor Cyan
Write-Host "1. Go to: https://github.com/i3mr01/MusicRP/actions" -ForegroundColor White
Write-Host "2. Look for 'Build and Release' workflow" -ForegroundColor White
Write-Host "3. Check if it ran when we pushed the tag" -ForegroundColor White

Write-Host "`n✨ Your i3mr0 MusicRP is ready for release!" -ForegroundColor Green
