# BabbelFish
Dot Net Library that provides a façade for Shooter's Tech REST API interface.

NOTE: Library is considered a WORK-IN-PROGRESS and is NOT intended for production environment at this time.


## BabelFish Use Quick Start
using ShootersTech.BabelFish.;

using ShootersTech.BabelFish.Responses;

## Available APIs Quick Reference
### ORION MATCH API
using ShootersTech.BabelFish.Requests.OrionMatchAPI; // (Optional if using parameters instead of Request object)

using ShootersTech.BabelFish.Responses.OrionMatchAPI;

using ShootersTech.BabelFish.DataModel.OrionMatch;

OrionMatchAPIClient.GetMatchDetailAsync(GetMatchRequest MatchRequest)		// MatchRequest.MatchID="1.2899.1040248529.0"

OrionMatchAPIClient.GetMatchDetailAsync(string MatchID)				// "1.2899.1040248529.0"

OrionMatchAPIClient.GetResultListAsync(GetResultListRequest ResultListRequest)	// ResultListRequest.MatchID="1.2899.1040248529.0", ResultListRequest.ResultListName="Individual - All"

OrionMatchAPIClient.GetResultListAsync(string MatchID, string ResultListName)	// "1.2899.1040248529.0", "Individual - All"

### DEFINITIONS
using ShootersTech.BabelFish.DataModel.Definitions;

using ShootersTech.BabelFish.Responses.DefinitionAPI;

using Attribute = ShootersTech.BabelFish.DataModel.Definitions.Attribute;

GetDefinitionResponse<Attribute> AttributeResponse = await DefinitionAPIClient.GetAttributeDefinitionAsync(setName).ConfigureAwait(false);

## Example API instantiation and call
// Namespaces
> using ShootersTech;

// API client using
> ShootersTech.BabelFish.DataModel.OrionMatch;

// DataModel Objects
> using ShootersTech.BabelFish.Requests.OrionMatchAPI;

// Request Objects - (Now optional if using parameters)
> using ShootersTech.BabelFish.Requests.OrionMatchAPI;

// Response Objects hold status + object returned
> using ShootersTech.BabelFish.Responses.OrionMatchAPI;

// Declare x-api-key for use
> string XApiKey = "[enter key here]";

// Instantiate API instance with x-api-key
> OrionMatchAPIClient client = new OrionMatchAPIClient(XApiKey);

// Setup request object for GetMatchDetail; Add MatchID to query
// (Now optional if using parameters)
> GetMatchRequest MatchDetailsRequest = new GetMatchRequest();
>
> MatchDetailsRequest.MatchID = "1.2899.1040248529.0";

// Call GetMatchDetailAsync API in to GetMatchReponse object
> GetMatchResponse MatchResponse = await client.GetMatchDetailAsync(MatchDetailsRequest).ConfigureAwait((false));

//Alternatively, skip the Request object and pass in parameter GetMatchResponse
> MatchResponse = await client.GetMatchDetailAsync("1.2899.1040248529.0").ConfigureAwait((false));

// Isolate Match object and Message Objectinto their own to work with
> Match MatchObject = MatchResponse.Match;
>
> MessageResponse messageResponse = MatchResponse.MessageResponse;

// RETURN VALUES
> if (MatchResponse.StatusCode == HttpStatusCode.OK)
>
> MatchResponse.StatusCode is returned HttpStatusCode
>
> MatchResponse.Body holds raw JSON string returned MatchResponse.
>
> Value is generated string of Match: {MatchDetail for 2021 Great American Rifle Open Day 1} 
>
> MatchResponse.TimeToRun is time CallAPI took, formatted: {minutes:seconds:milliseconds} 
>
> FYI: inital run about 600-700ms, subsequent runs 100-200ms 
>
> MatchResponse.Match object populated (!null)

> if (MatchResponse.StatusCode != HttpStatusCode.OK)
> MessageResponse.ResponseCodes holds error code returned from API List
> MessageResponse.Message holds error Message returned from API List

# BabelFish Versioning
*__v1.0.14.0__*

Added None as option to ResultListFormat.LinkTo

Updated README.md with markdown code

*__v1.0.13.0__*

Changed GetMatchSearchRequest.ShootingStyle from string to List<string>

*__v1.0.12.0__*

Add ScoreHistoryAPI

Update namespace to ShootersTech.BabelFish

Updated Orion MatchSearch function

*__v1.0.11.0__*

Add GetVersionAPI -> GetVersion() with VersionService, VersionLevel Enum helpers

*__v1.0.10.0__*

Implement Definition Caching in local memory/file system

Add UserSettings values to control cache functionality

*__v1.0.9.0__*

Add Get and Set Attribute Value functionality

*__v1.0.8.0__*

Implement Cognito Authentication on the back end via username/password retrieving RefreshToken, IdToken, AccessToken, DeviceToken.

Add ApiClient GetAuthTokens() and UpdateAuthTokens() to retrieve/reset authentication tokens in a single session.

Add AuthAPIClient that authenticates user via Cognito and returns Tokens.

*__v1.0.7.0__*

Implement AWS Signer v4 functionality for Authenticated APIs

Add Allowed UserSettings to support AWS Signing

*__v1.0.6.0__*

Introduce User Settings: 

 Add App.config that contains defaults and reads User Settings file BabelFish_User_Settings.config

 Add incoming UserSettings Dictionary<string,string> as alternative to config files

Combined .NETStandard2.0, .NET6.0 compatibility into single compilation

Addition of GetMatchSearch()

Addition of GetSquaddingList()

*__v1.0.5.0__*

Update Definitions logic

Addition of Definitions

*__v1.0.4.0__*

Move Message Response out of returned object

Add Unit Tests Project

Add self-contained External API for zipcode information

*__v1.0.3.0__*

Addition of GetResultCourseOfFireDetail()

Moved ResponseTemplate.cs from /DataModel/OrionMatch to /DataModel

*__v1.0.2.0__*

Updated data structure

Addition of GetResultList

Modified return json string processing in to object

*__v1.0.1.0__*

Addition of Response.TimeToRun yielding time (minutes:seconds:milliseconds) to query API and process returned json in to object

*__v1.0.0.0__*

Initial development build implementing GetMatchDetailAsync()