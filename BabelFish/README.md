# BabbelFish
Dot Net Library that provides a façade for Shooter's Tech REST API interface.

NOTE: Library is considered a WORK-IN-PROGRESS and is NOT intended for production environment at this time.


## BabelFish Use Quick Start
using Scopos.BabelFish.;

using Scopos.BabelFish.Responses;

## Available APIs Quick Reference
### ORION MATCH API

using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.DataModel.OrionMatch;

var client = new OrionMatchAPIClient( "YourXAPIKey" );
var matchId = "1.1.2023011915575119.0";
var matchDetail = client.GetMatchDetailPublicAsync( matchId ).Result;

### DEFINITIONS

using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.APIClients;

var client = new DefinitionAPIClient( "YourXAPIKey" );
var setName = SetName.Parse("v1.0:ntparc:Three-Position Air Rifle Type");
var airRifleTypeDefinition = client.GetAttributeDefinitionAsync(setName).Result;
