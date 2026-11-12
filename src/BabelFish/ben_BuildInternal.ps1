###############################################################################
# Build Scopos.BabelFish.Core and copy it to the internal NuGet feed.
# Defaults to this checkout, independently of the caller's working directory.
###############################################################################
[CmdletBinding()]
param(
    [string]$BabelFishRepositoryPath,
    [string]$LocalNugetPath = $env:NUGET_PATH,
    [string]$ProjectFile,
    [string]$BabelFishCoreFeedPath,
    # Optional explicit version; otherwise increment the project's current version.
    [string]$PackageVersion,
    # After copying the package, update the four Rezults Core references.
    [switch]$UpdateRezults,
    # Defaults to the Rezults checkout beside BabelFish.
    [string]$RezultsRepositoryPath
)

$ErrorActionPreference = 'Stop'

if (-not $BabelFishRepositoryPath) {
    $BabelFishRepositoryPath = (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path
}
if (-not $ProjectFile) {
    $ProjectFile = Join-Path $BabelFishRepositoryPath 'src\BabelFish\BabelFish.csproj'
}
$ProjectFile = (Resolve-Path -LiteralPath $ProjectFile).Path
if (-not $BabelFishCoreFeedPath) {
    if (-not $LocalNugetPath) {
        throw 'Set NUGET_PATH or pass -LocalNugetPath or -BabelFishCoreFeedPath.'
    }
    $BabelFishCoreFeedPath = Join-Path $LocalNugetPath 'BabelFish'
}

Get-Command dotnet -ErrorAction Stop | Out-Null
$projectText = [IO.File]::ReadAllText($ProjectFile)
[xml]$csproj = $projectText
$currentVersion = [string]$csproj.Project.PropertyGroup.Version
$packageId = [string]$csproj.Project.PropertyGroup.PackageId
if (-not $packageId) { throw "PackageId not found in $ProjectFile" }

# Increment numeric prerelease counters (2.1.10-beta.2 -> 2.1.10-beta.3)
# or the last numeric component of a stable version (1.2.3.4 -> 1.2.3.5).
if ($currentVersion -match '^(\d+\.\d+\.\d+)-(\w[\w.-]*?)\.(\d+)$') {
    $revision = [int]$Matches[3] + 1
    $newVersion = '{0}-{1}.{2}' -f $Matches[1], $Matches[2], $revision
    $newFileVersion = '{0}.{1}' -f $Matches[1], $revision
} elseif ($currentVersion -match '^\d+\.\d+\.\d+(\.\d+)?$') {
    $parts = $currentVersion.Split('.')
    $parts[$parts.Length - 1] = [string]([int]$parts[-1] + 1)
    $newVersion = $parts -join '.'
    $newFileVersion = $newVersion
} else {
    throw "Cannot increment version '$currentVersion'. Expected a numeric version or a prerelease ending in .N."
}

if ($PackageVersion) {
    if ($PackageVersion -match '^(\d+\.\d+\.\d+)-\w[\w.-]*?\.(\d+)$') {
        $newFileVersion = '{0}.{1}' -f $Matches[1], $Matches[2]
    } elseif ($PackageVersion -match '^\d+\.\d+\.\d+(\.\d+)?$') {
        $newFileVersion = $PackageVersion
    } else {
        throw "Invalid PackageVersion '$PackageVersion'. Expected a numeric version or a prerelease ending in .N."
    }
    $newVersion = $PackageVersion
}

$packageName = "$packageId.$newVersion.nupkg"
function Assert-PackageVersionUnused {
    if (Test-Path -LiteralPath $BabelFishCoreFeedPath) {
        $existingPackage = Get-ChildItem -LiteralPath $BabelFishCoreFeedPath -Recurse -File -Filter $packageName |
            Select-Object -First 1
        if ($existingPackage) {
            throw "Package version $newVersion already exists in the feed: $($existingPackage.FullName). Choose an unused version with -PackageVersion."
        }
    }
}

# Fail before building or changing project metadata if this version is in the feed.
Assert-PackageVersionUnused

foreach ($element in @('Version', 'FileVersion')) {
    if ([regex]::Matches($projectText, "<$element>[^<]*</$element>").Count -ne 1) {
        throw "Expected exactly one $element element in $ProjectFile."
    }
}

# Validate all consumer edits before building; write them only after the package
# has been successfully copied to the feed. Preserve surrounding project text.
$rezultsUpdates = @()
if ($UpdateRezults) {
    if (-not $RezultsRepositoryPath) {
        $RezultsRepositoryPath = Join-Path (Split-Path $BabelFishRepositoryPath) 'Rezults'
    }
    $referencePattern = '(<PackageReference\b[^>]*\bInclude="Scopos\.BabelFish\.Core"[^>]*\bVersion=")[^"]*(")'
    foreach ($relativeProject in @(
        'Genesis.Classes\Genesis.Classes.csproj',
        'Genesis.Rezults\Genesis.Rezults.csproj',
        'Henchmen\Henchmen.csproj',
        'HenchmenTrials\HenchmenTrials.csproj'
    )) {
        $consumerPath = (Resolve-Path -LiteralPath (Join-Path $RezultsRepositoryPath $relativeProject)).Path
        $consumerText = [IO.File]::ReadAllText($consumerPath)
        if ([regex]::Matches($consumerText, $referencePattern).Count -ne 1) {
            throw "Expected exactly one Scopos.BabelFish.Core PackageReference with a Version attribute in $consumerPath."
        }
        $rezultsUpdates += [pscustomobject]@{
            Path = $consumerPath
            Text = [regex]::Replace($consumerText, $referencePattern, ('${1}' + $newVersion + '${2}'))
        }
    }
}

# A unique output directory prevents selecting a stale or unrelated package.
$outputDirectory = Join-Path (Split-Path $ProjectFile) ('bin\InternalPackages\' + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $outputDirectory -Force | Out-Null
New-Item -ItemType Directory -Path $BabelFishCoreFeedPath -Force | Out-Null

Write-Host "Packing $ProjectFile : $currentVersion -> $newVersion"
& dotnet pack $ProjectFile -c Release --output $outputDirectory "-p:Version=$newVersion" "-p:PackageVersion=$newVersion" "-p:FileVersion=$newFileVersion"
if ($LASTEXITCODE -ne 0) {
    throw "dotnet pack failed (exit code $LASTEXITCODE). Project version was not changed."
}

$packagePath = Join-Path $outputDirectory "$packageId.$newVersion.nupkg"
if (-not (Test-Path -LiteralPath $packagePath -PathType Leaf)) {
    throw "dotnet pack did not produce the expected package: $packagePath"
}
# Recheck after packing and refuse overwrites even if another build races us.
Assert-PackageVersionUnused
$feedPackagePath = Join-Path $BabelFishCoreFeedPath $packageName
[IO.File]::Copy($packagePath, $feedPackagePath, $false)

# Persist the version only after packing and copying both succeed. Keep the
# project's formatting; FileVersion must be numeric, even for prerelease packages.
$updatedText = [regex]::Replace($projectText, '<Version>[^<]*</Version>', "<Version>$newVersion</Version>")
$updatedText = [regex]::Replace($updatedText, '<FileVersion>[^<]*</FileVersion>', "<FileVersion>$newFileVersion</FileVersion>")
[IO.File]::WriteAllText($ProjectFile, $updatedText, (New-Object Text.UTF8Encoding $false))
Write-Host "Copied $packageId.$newVersion.nupkg to $BabelFishCoreFeedPath"
foreach ($consumerUpdate in $rezultsUpdates) {
    [IO.File]::WriteAllText($consumerUpdate.Path, $consumerUpdate.Text, (New-Object Text.UTF8Encoding $false))
    Write-Host "Updated $($consumerUpdate.Path): Scopos.BabelFish.Core $newVersion"
}
Write-Host "Updated project Version=$newVersion, FileVersion=$newFileVersion. Done."
