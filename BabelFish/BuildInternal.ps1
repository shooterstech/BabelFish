param(
    [string]$BabelFishRepositoryPath = $env:BF_REPO_PATH,
    [string]$LocalNugetPath = $env:NUGET_PATH,
    [string]$ProjectFile = $BabelFishRepositoryPath + "\BabelFish\BabelFish.csproj",
    [string]$BabelFishCoreFeedPath = $LocalNugetPath + "\BabelFish"
)

# setx BF_REPO_PATH C:\Users\erikkanderson\Documents\Programming\BabelFish
# setx NUGET_PATH "G:\Shared drives\Development Builds\Nuget"
# Remember to restart powershell

if ( -not $BabelFishRepositoryPath ) {
    throw "Environment Variable BF_REPO_PATH not set"
}

if ( -not $LocalNugetPath ) {
    throw "Environment Variable NUGET_PATH not set"
}

Write-Host "Loading project file: $ProjectFile"
Write-Host "Will save to directory: $BabelFishCoreFeedPath"

[xml]$csproj = Get-Content $ProjectFile -Encoding UTF8

# Locate the FileVersion element
$currentFileVersion = $csproj.Project.PropertyGroup.FileVersion

if (-not $currentFileVersion) {
    throw "FileVersion element not found in $ProjectFile"
}

Write-Host "Current FileVersion: $currentFileVersion"

# Parse and increment the last segment
$parts = $currentFileVersion.Split('.')
$parts[3] = [int]$parts[3] + 1
$newVersion = $parts -join '.'

Write-Host "New FileVersion: $newVersion"

# Update the XML
$csproj.Project.PropertyGroup.FileVersion = $newVersion
$csproj.Save($ProjectFile)

Write-Host "Updated file $ProjectFile"

# Build the package
Write-Host "Packing project..."
dotnet pack $ProjectFile -c Debug 2>$null | Out-Null

# Find the generated nupkg
$package = Get-ChildItem -Recurse -Filter "*.nupkg" | Sort-Object LastWriteTime -Descending | Select-Object -First 1

Write-Host "Copying package $($package.Name) to feed: $BabelFishCoreFeedPath"
Copy-Item $package.FullName $BabelFishCoreFeedPath -Force

Write-Host "Done."