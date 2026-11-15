$staging = Join-Path $env:TEMP "classic-aero-staging"
if (Test-Path $staging) { Remove-Item -Recurse -Force $staging }
New-Item -ItemType Directory -Force -Path "$staging\Themes" | Out-Null
New-Item -ItemType Directory -Force -Path "$staging\Assets\Icons" | Out-Null
New-Item -ItemType Directory -Force -Path "$staging\Templates" | Out-Null
New-Item -ItemType Directory -Force -Path "$staging\Services" | Out-Null

$skillRoot = Split-Path -Parent $PSScriptRoot
$src = Join-Path $skillRoot "examples"
Copy-Item "$src\*Styles.xaml" "$staging\Themes" -Force
Copy-Item "$src\ClassicWindowsTheme.xaml" "$staging\Themes" -Force
Copy-Item "$src\DarkAeroTheme.xaml" "$staging\Themes" -Force
Copy-Item "$src\SubHeaderBarTemplate.xaml" "$staging\Themes" -Force
Copy-Item "$src\ClassicIcons.xaml" "$staging\Assets\Icons" -Force
Copy-Item "$src\app_icon.*" "$staging\Assets" -Force
Copy-Item "$src\*Template.*" "$staging\Templates" -Force
Copy-Item "$src\AGENTS.md" "$staging" -Force
Copy-Item "$src\README.md" "$staging" -Force
Copy-Item "$src\ThemeManager.cs" "$staging\Services" -Force

$zipPath = Join-Path $skillRoot "classic-aero-ui-bundle.zip"
if (Test-Path $zipPath) { Remove-Item -Force $zipPath }
Compress-Archive -Path "$staging\*" -DestinationPath $zipPath -Force

# Sync to global skill directory
$globalDir = "$env:USERPROFILE\.gemini\config\skills\classic-aero-ui"
if (Test-Path $globalDir) {
    Copy-Item $zipPath "$globalDir\classic-aero-ui-bundle.zip" -Force
}

Remove-Item -Recurse -Force $staging
Write-Host "Zip bundle created successfully at: $zipPath"
