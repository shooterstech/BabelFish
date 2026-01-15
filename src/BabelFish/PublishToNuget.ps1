###############################################################################
#
# Publish to NuGet Script for BabelFish.Core
# 
###############################################################################

param(
    [string]$BabelFishRepositoryPath = $env:BF_REPO_PATH,
    [string]$LocalNugetPath = $env:NUGET_PATH,
    [string]$NugetApiKey = $env:NUGET_API_KEY,
    [Parameter(Mandatory = $true)]
    [string]$Version,   # <-- version passed in by caller
    [string]$ProjectFile = $BabelFishRepositoryPath + "\src\BabelFish\BabelFish.csproj",
    [string]$BabelFishCoreFeedPath = $LocalNugetPath + "\BabelFish"
)

# Build the full package filename dynamically
$packagePath = "$BabelFishCoreFeedPath\Scopos.BabelFish.Core.$Version.nupkg"

Write-Host $packagePath $NugetApiKey 

dotnet nuget push $packagePath --api-key $NugetApiKey --source https://api.nuget.org/v3/index.json

Write-Host "The Nuget package has been pushed up, all seems fine so far." -ForegroundColor Green
$confirmation = Read-Host -Prompt "Do you want to continue to Rebuild the helsite as well? Type 'Y' to proceed"
if ($confirmation -eq 'Y' -or $confirmation -eq 'y') {
    Write-Host "Now we write the helpsite!" -ForegroundColor Cyan
    # Place the rest of your script's logic here
} else {
    Write-Host "As you wish." -ForegroundColor Red
    # You can exit the script here if needed
    exit 1
}

$scriptPath = Join-Path -Path $PSScriptRoot -ChildPath "PublishHelpsite.ps1"
& $scriptPath -$Version "$Version"
