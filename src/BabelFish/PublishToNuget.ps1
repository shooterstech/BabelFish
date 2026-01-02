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