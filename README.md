# BabbelFish
Dot Net Library that provides a façade for Shooter's Tech REST API interface.

NOTE: Library is considered a WORK-IN-PROGRESS and is NOT intended for production environment at this time.

===============================
BabelFish Use Quick Start
===============================
using BabelFish;
using BabelFish.Responses;

===============================
Available APIs Quick Reference
===============================
using BabelFish.DataModel.OrionMatch;
using BabelFish.Requests.OrionMatchAPI; // (Optional if using parameters instead of Request object)
using BabelFish.Responses.OrionMatchAPI;

OrionMatchAPIClient.GetMatchDetailAsync(GetMatchRequest MatchRequest)		// MatchRequest.MatchID="1.2899.1040248529.0"
OrionMatchAPIClient.GetMatchDetailAsync(string MatchID)				// "1.2899.1040248529.0"

OrionMatchAPIClient.GetResultListAsync(GetResultListRequest ResultListRequest)	// ResultListRequest.MatchID="1.2899.1040248529.0", ResultListRequest.ResultListName="Individual - All"
OrionMatchAPIClient.GetResultListAsync(string MatchID, string ResultListName)	// "1.2899.1040248529.0", "Individual - All"

OrionMatchAPIClient.GetResultCourseOfFireDetail(GetResultCOFDetailRequest ResultCofDetail).ConfigureAwait(false); //ResultCofDetail.ResultCOFID="03b0a667-f184-404b-8ba7-751599b7fd0b"
OrionMatchAPIClient.GetResultCourseOfFireDetail("03b0a667-f184-404b-8ba7-751599b7fd0b").ConfigureAwait(false);    //"03b0a667-f184-404b-8ba7-751599b7fd0b"
===============================
using BabelFish.DataModel.Definitions;
using BabelFish.Responses.DefinitionAPI;
using Attribute = BabelFish.DataModel.Definitions.Attribute;

GetDefinitionResponse<Attribute> AttributeResponse = await DefinitionAPIClient.GetAttributeDefinitionAsync(setName).ConfigureAwait(false);


===============================
Example API instantiation and call
===============================
// Namespaces
using BabelFish;				// API client
using BabelFish.DataModel.OrionMatch;		// DataModel Objects
using BabelFish.Requests.OrionMatchAPI; 	// Request Objects - (Now optional if using parameters)
using BabelFish.Responses.OrionMatchAPI;	// Response Objects hold status + object returned

// Declare x-api-key for use
string XApiKey = "[enter key here]";

// Instantiate API instance with x-api-key
OrionMatchAPIClient client = new OrionMatchAPIClient(XApiKey);

// Setup request object for GetMatchDetail; Add MatchID to query 
// (Now optional if using parameters)
GetMatchRequest MatchDetailsRequest = new GetMatchRequest();
MatchDetailsRequest.MatchID = "1.2899.1040248529.0";


// Call GetMatchDetailAsync API in to GetMatchReponse object
GetMatchResponse MatchResponse = await client.GetMatchDetailAsync(MatchDetailsRequest).ConfigureAwait((false));
//Alternatively, skip the Request object and pass in parameter
GetMatchResponse MatchResponse = await client.GetMatchDetailAsync("1.2899.1040248529.0").ConfigureAwait((false));


// Isolate Match object and Message Objectinto their own to work with
Match MatchObject = MatchResponse.Match;
MessageResponse messageResponse = MatchResponse.MessageResponse;

// RETURN VALUES
if (MatchResponse.StatusCode == HttpStatusCode.OK)
	MatchResponse.StatusCode is returned HttpStatusCode
	MatchResponse.Body holds raw JSON string returned
	MatchResponse.Value is generated string of Match: {MatchDetail for 2021 Great American Rifle Open Day 1}
	MatchResponse.TimeToRun is time CallAPI took, formatted: {minutes:seconds:milliseconds}
		FYI: inital run about 600-700ms, subsequent runs 100-200ms
	MatchResponse.Match object populated (!null)

if (MatchResponse.StatusCode != HttpStatusCode.OK)
	MessageResponse.ResponseCodes holds error code returned from API List<string>
	MessageResponse.Message holds error Message returned from API List<string>
