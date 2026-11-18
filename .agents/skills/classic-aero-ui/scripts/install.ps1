param (
    [string]$TargetDir = (Get-Location).Path,
    [switch]$AutoConfigureApp
)

$ErrorActionPreference = "Stop"

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "   Classic Aero Enterprise UI - Project Setup Tool        " -ForegroundColor Cyan
Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host "Target Directory: $TargetDir" -ForegroundColor Yellow

$SkillDir = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
if (-not (Test-Path "$SkillDir\examples\ClassicWindowsTheme.xaml")) {
    $SkillDir = "$env:USERPROFILE\.gemini\config\skills\classic-aero-ui"
}

$ExamplesDir = Join-Path $SkillDir "examples"
if (-not (Test-Path $ExamplesDir)) {
    Write-Error "Skill examples folder not found at: $ExamplesDir"
    exit 1
}

# 1. Target Directories
$ThemesDir = Join-Path $TargetDir "Themes"
$IconsDir = Join-Path $TargetDir "Assets\Icons"
$AssetsDir = Join-Path $TargetDir "Assets"
$ServicesDir = Join-Path $TargetDir "Services"

New-Item -ItemType Directory -Force -Path $ThemesDir | Out-Null
New-Item -ItemType Directory -Force -Path $IconsDir | Out-Null
New-Item -ItemType Directory -Force -Path $ServicesDir | Out-Null

# 2. Copy Theme Dictionaries (All styles, Classic Aero Light & Dark Aero)
Write-Host "--> Copying Themes to $ThemesDir..." -ForegroundColor Green
$ThemeFiles = Get-ChildItem -Path $ExamplesDir -Filter "*Styles.xaml"
$ThemeFiles += Get-ChildItem -Path $ExamplesDir -Filter "ClassicWindowsTheme.xaml"
$ThemeFiles += Get-ChildItem -Path $ExamplesDir -Filter "DarkAeroTheme.xaml"
$ThemeFiles += Get-ChildItem -Path $ExamplesDir -Filter "SubHeaderBarTemplate.xaml"

foreach ($file in $ThemeFiles) {
    Copy-Item -Path $file.FullName -Destination $ThemesDir -Force
}

# 3. Copy Icons and Application Assets
Write-Host "--> Copying Icons to $IconsDir..." -ForegroundColor Green
Copy-Item -Path (Join-Path $ExamplesDir "ClassicIcons.xaml") -Destination $IconsDir -Force

# Copy neutral Aero crystal icon only if project does not already have an application icon
if (-not (Test-Path (Join-Path $AssetsDir "app_icon.png")) -and (Test-Path (Join-Path $ExamplesDir "app_icon.png"))) {
    Copy-Item -Path (Join-Path $ExamplesDir "app_icon.png") -Destination $AssetsDir
}
if (-not (Test-Path (Join-Path $AssetsDir "app_icon.ico")) -and (Test-Path (Join-Path $ExamplesDir "app_icon.ico"))) {
    Copy-Item -Path (Join-Path $ExamplesDir "app_icon.ico") -Destination $AssetsDir
}

# 4. Copy Services (ThemeManager.cs)
Write-Host "--> Copying ThemeManager.cs to $ServicesDir..." -ForegroundColor Green
Copy-Item -Path (Join-Path $ExamplesDir "ThemeManager.cs") -Destination $ServicesDir -Force

# 5. Copy AGENTS.md to Project Root (Enforcing Design Exclusivity)
Write-Host "--> Copying AGENTS.md to target root..." -ForegroundColor Green
Copy-Item -Path (Join-Path $ExamplesDir "AGENTS.md") -Destination $TargetDir -Force

# 6. Check App.xaml
$AppXamlPath = Join-Path $TargetDir "App.xaml"
if (Test-Path $AppXamlPath) {
    $xamlContent = Get-Content $AppXamlPath -Raw
    if ($xamlContent -notmatch "ClassicIcons\.xaml") {
        Write-Host "--> Note: Please register ClassicIcons.xaml and ClassicWindowsTheme.xaml in App.xaml resources." -ForegroundColor Yellow
    } else {
        Write-Host "--> App.xaml is already registered with Classic Aero resources." -ForegroundColor Green
    }
}

Write-Host "==========================================================" -ForegroundColor Cyan
Write-Host " Setup Complete! Classic Aero UI assets are ready in:" -ForegroundColor Green
Write-Host " - Themes/ (Component styles + ClassicWindowsTheme + DarkAeroTheme)" -ForegroundColor White
Write-Host " - Assets/Icons/ClassicIcons.xaml (67 high-DPI vector icons)" -ForegroundColor White
Write-Host " - Assets/app_icon.png (Application icon)" -ForegroundColor White
Write-Host " - Services/ThemeManager.cs (Dynamic runtime theme switching)" -ForegroundColor White
Write-Host " - AGENTS.md (Absolute design exclusivity rules)" -ForegroundColor White
Write-Host "==========================================================" -ForegroundColor Cyan
