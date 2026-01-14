###############################################################################
#
# Publish to Helpsite script for Sandcastle project
# 
###############################################################################

param(
    [string]$BabelFishRepositoryPath = $env:BF_REPO_PATH,
    [string]$SandcastleUN = $env:SHFB_UN,
    [string]$SandcastlePW = $env:SHFB_PW,
    [string]$SandcastleSiteIP = $env:SHFB_SITE_IP,
    [Parameter(Mandatory = $true)]
    [string]$Version,   # <-- version passed in by caller
    [string]$SandcastleProjDir = $BabelFishRepositoryPath + "\src\BabelFish\Sandcastle\BabelFishSandcastleBuild.shfbproj",
    [string]$SandcastleHelpFolder = "C:\temp\Help",
    [string]$SandcastleConfiguration = "Release"
)

$winScpDll = "C:\Program Files (x86)\WinSCP\WinSCPnet.dll"
Add-Type -Path $winScpDll

#here we're getting the MSBuild path, this is so the SHFB csproj is interpreted properly...
function Get-MSBuildPath {
    $vsRoot = "C:\Program Files\Microsoft Visual Studio"

    # Search all editions and versions for MSBuild.exe
    $msbuildCandidates = Get-ChildItem -Path $vsRoot -Directory -Recurse -ErrorAction SilentlyContinue |
        Where-Object { $_.Name -eq "Bin" -and (Test-Path "$($_.FullName)\MSBuild.exe") } |
        Select-Object -ExpandProperty FullName

    if ($msbuildCandidates.Count -gt 0) {
        # Pick the newest version by sorting the path (VS versions sort naturally)
        $best = $msbuildCandidates | Sort-Object -Descending | Select-Object -First 1
        return "$best\MSBuild.exe"
    }

    # Fallback: PATH
    $fromPath = Get-Command msbuild.exe -ErrorAction SilentlyContinue
    if ($fromPath) {
        return $fromPath.Source
    }

    throw "MSBuild.exe not found. Install Visual Studio or Build Tools."
}

#This will create the Sandcastle Build from the BabelFish build file.

# Ensure the SHFB project file path is provided and exists
if (-not $SandcastleProjDir) {
    Write-Host "Please specify the path to the .shfbproj file."
    exit 1
}

if (-not (Test-Path $SandcastleProjDir)) {
    Write-Host "SHFB project file not found: $SandcastleProjDir"
    exit 1
}

$msbuild = Get-MSBuildPath

Write-Host "Starting SHFB build for $SandcastleProjDir (Configuration: $SandcastleConfiguration)"

# Execute the dotnet build command
# The build process will use MSBuild internally to process the .shfbproj file
try {
    # The '&' operator ensures the command is executed and output is captured/streamed correctly # 
    $testingVar = @"
For BabelFish Version $Version
"@
    & $msbuild $SandcastleProjDir /t:Build /p:Configuration=$SandcastleConfiguration /p:BfVersion=$Version
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet build command failed with exit code: $LASTEXITCODE"
    }
    Write-Host "SHFB build completed successfully."
} catch {
    Write-Error $_.Exception.Message
    exit 1
}

#Confirm the website is ready to go, things propagated properly.
Write-Host "Website has been built, make sure that things are the way you want!" -ForegroundColor Green

Write-Host "Adding Google analytics to helpsite."
$scriptPath = Join-Path -Path $PSScriptRoot -ChildPath "AddGAToHelpsite.ps1"
& $scriptPath

$confirmation = Read-Host -Prompt "Do you want to continue to Delte the site and re-build it? Type 'Y' to proceed"
if ($confirmation -eq 'Y' -or $confirmation -eq 'y') {
    Write-Host "Deleting Helpsite" -ForegroundColor Cyan
    # Place the rest of your script's logic here
} else {
    Write-Host "As you wish." -ForegroundColor Red
    # You can exit the script here if needed
    exit 1
}

# -------------------------------
# SHARED progress handler
# -------------------------------
$progressHandler = {
    param($sender, $e)

    $percent = [int]($e.OverallProgress * 100)

    Write-Progress `
        -Activity $script:CurrentActivity `
        -Status "$percent% complete" `
        -PercentComplete $percent
}

# -------------------------------
# WinSCP session options
# -------------------------------
$sessionOptions = New-Object WinSCP.SessionOptions -Property @{
    Protocol = [WinSCP.Protocol]::Ftp
    HostName = $SandcastleSiteIP
    UserName = $SandcastleUN
    Password = $SandcastlePW
}

# ============================================================
# SESSION 1 — DELETE PHASE
# ============================================================

$deleteSession = New-Object WinSCP.Session
$deleteSession.Timeout = New-TimeSpan -Minutes 5

try {
    $deleteSession.add_FileTransferProgress($progressHandler)
    $deleteSession.Open($sessionOptions)

    # Attach shared handler BEFORE any operations
    $script:CurrentActivity = "Deleting remote help site"

    $deleteStart = Get-Date

    # List directory
    $directory = $deleteSession.ListDirectory("/httpdocs")
    $items = $directory.Files | Where-Object { $_.Name -notin @(".", "..") }

    $total = $items.Count
    $index = 0

    foreach ($item in $items) {
        $index++

        $percent = [int](($index / $total) * 100)

        Write-Progress `
            -Activity $script:CurrentActivity `
            -Status "$percent% complete" `
            -PercentComplete $percent

        $deleteSession.RemoveFiles("/httpdocs/$($item.Name)").Check()
    }

    Write-Progress -Activity $script:CurrentActivity -Completed

    $deleteEnd = Get-Date
    $deleteDuration = $deleteEnd - $deleteStart

    Write-Host "Deletion completed in $([int]$deleteDuration.TotalSeconds) seconds."
}
finally {
    $deleteSession.Dispose()
}

# ============================================================
# SESSION 2 — UPLOAD PHASE
# ============================================================

$uploadSession = New-Object WinSCP.Session
$uploadSession.Timeout = New-TimeSpan -Minutes 5

try {
    $uploadSession.add_FileTransferProgress($progressHandler)
    $uploadSession.Open($sessionOptions)

    # Attach SAME handler BEFORE upload
    $script:CurrentActivity = "Uploading help site"

    $uploadStart = Get-Date

    $uploadSession.PutFiles("$SandcastleHelpFolder\*", "/httpdocs/").Check()

    Write-Progress -Activity $script:CurrentActivity -Completed

    $uploadEnd = Get-Date
    $uploadDuration = $uploadEnd - $uploadStart

    Write-Host "Upload completed in $([int]$uploadDuration.TotalSeconds) seconds."
    Write-Host "File transfer successful!"
}
finally {
    $uploadSession.Dispose()
}
