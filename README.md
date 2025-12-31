# BabelFish
Scopos' dot net library that provides:
 * Scopos' data model for the sport of shooting.
 * A façade for Scopos' REST API interface to interact with match data, score history, and definitions.
 * Scopos' data actors for formatting results, calculating results lists, and tournament calculations.
 * And more!

BabelFish classes are document at [cdn.scopos.tech/Documentation/Babelfish/index.html](https://cdn.scopos.tech/Documentation/Babelfish/index.html).

## License
This project is licensed under the Apache License 2.0 - see the LICENSE file for details.

## NuGet Package Avaliable

BabelFish is avaliable as a NuGet package. To download it, create a Package Source to our NuGet feed.
1. Open Visual Studio.
2. Go to Tools → NuGet Package Manager → Package Manager Settings.
3. In the left panel, select Package Sources.
4. Click the + button to add a new source.
5. Set the Name to Scopos NuGet.
6. Set the Source to:
https://scopos-nuget.s3.us-east-1.amazonaws.com/index.json
7. Click Update, then OK.

## BabelFish Quick Use Start Guide

The following examples are part of our [scopos-labs](https://github.com/shooterstech/scopos-labs/tree/master/csharp/Command%20Line%20Examples) open source example repository.

### ORION MATCH API
Demonstrates how to load information about a match and its result lists. Then using the Result List Intermediate Formatter, print out the result lists.

```csharp
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.DataActors.ResultListFormatter.UserProfile;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime;

//You may use GyaHV300my60rs2ylKug5aUgFnYBj6GrU6V1WE33 as a x-api-key to start working with our API.
//However, this api key is limited in its use, and should not be used in any real application.
Initializer.Initialize( "GyaHV300my60rs2ylKug5aUgFnYBj6GrU6V1WE33", false );
DefinitionAPIClient.LocalStoreDirectory = new DirectoryInfo( @"C:\temp" );

var client = new OrionMatchAPIClient();

//A MatchID uniquely identifies a match
var matchId = new MatchID( "1.1.2025100109364878.1" );

//Retreives information about the match
var getMatchResponse = await client.GetMatchAsync( matchId );
if (getMatchResponse.HasOkStatusCode) {
    var match = getMatchResponse.Match;
    Console.WriteLine( match.Name );      // October 2025 - Scopos' 3 Position 3x20 Air Rifle Virtual Match
    Console.WriteLine( match.StartDate ); // 10/01/2025 00:00:00
    Console.WriteLine( match.EndDate );   // 10/31/2025 00:00:00

    //Loop through and find the primary result lists
    foreach (var resultEvent in match.ResultEvents) {
        foreach (var resultListAbbr in resultEvent.ResultLists) {
            if (resultListAbbr.Primary) {

                //Retreives the result list (note this command only reteives the start of the list).
                var getResultListResponse = await client.GetResultListAsync( matchId, resultListAbbr.ResultName );
                if (getResultListResponse.HasOkStatusCode) {

                    var resultList = getResultListResponse.ResultList;

                    //Reteive the recommended RESULT LIST FORMAT to use on this Result List
                    var resultListFormat = await resultList.GetResultListFormatDefinitionAsync();

                    //Instantiate a Result List Intermediate Formatted instance, to easily allow us to print out the results.
                    var rlif = new ResultListIntermediateFormatted( resultList, resultListFormat, new BaseUserProfileLookup() );
                    await rlif.InitializeAsync();

                    //For demo purposes, just show the top 3 participants.
                    rlif.ShowNumberOfBodyRows = 0;
                    rlif.ShowNumberOfChildRows = 0;
                    rlif.ShowRanks = 3;

                    //Pretend we are on a wide screen.
                    rlif.ResolutionWidth = 5000;

                    Console.WriteLine( $"Show results for {resultList.Name}" );

                    //Print the header row
                    foreach (var colIndex in rlif.GetShownColumnIndexes()) {
                        Console.Write( rlif.GetColumnHeaderCell( colIndex ).Text );
                        Console.Write( "  " );
                    }
                    Console.WriteLine();

                    //Print the results, one row at a time.
                    foreach (var row in rlif.ShownRows) {
                        foreach (var colIndex in rlif.GetShownColumnIndexes()) {
                            Console.Write( row.GetColumnBodyCell( colIndex ).Text );
                            Console.Write( "  " );
                        }
                        Console.WriteLine();
                    }
                }
                Console.WriteLine();

                /*
                    Show results for Individual - Sporter
                    Rank  Participant  Location  Kneeling  Prone  Standing  Aggregate  
                    1   TOLOSA, REEF  Aiea, HI  190.2  197.5  178.4  566.1  
                    2   Rosario, Joalis  Bartow, FL  184.4  196.7  182.3  563.4  
                    3   Algarin, Miniale  Bartow, FL  189.9  193.9  178.2  562.0  

                    Show results for Individual - Precision
                    Rank  Participant  Location  Kneeling  Prone  Standing  Aggregate  
                    1   Miller, Meredith  Green Springs, OH  207.3  210.2  201.9  619.4  
                    2   Miller, Lyla  Green Springs, OH  206.7  206.1  205.4  618.2  
                    3   Mix, Sarah  Green Springs, OH  203.8  208.6  204.9  617.3  

                    Show results for Team - Sporter
                    Rank  Participant  Location  Kneeling  Prone  Standing  Aggregate  
                    1   Summerlin Academy AJROTC  Bartow, FL  757.6  772.3  708.0  2237.7  
                    2   MSYESS  Booneville, MS  738.2  765.8  640.6  2126.0  
                    3   Bensalem MCJROTC - Sporter  Bensalem, PA  717.1  779.7  601.9  2071.8  

                    Show results for Team - Precision
                    Rank  Participant  Location  Kneeling  Prone  Standing  Aggregate  
                    1   American Legion Post 295 - Precision  Green Springs, OH  819.9  832.2  812.7  2462.4  
                    2   North Forsyth Raiders  Cumming, GA  774.2  813.1  724.8  2294.4  
                    3   Marcos de Niza AJROTC - Precision  Tempe, AZ  710.3  779.5  702.5  2192.3  
                 */
            }
        }
    }
}
```

### SCORE HISTORY
Demonstrates how to load historical publicly avaliable scores for a user.

```csharp
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;
using Scopos.BabelFish.Responses.ScoreHistoryAPI;
using Scopos.BabelFish.Runtime;

//You may use GyaHV300my60rs2ylKug5aUgFnYBj6GrU6V1WE33 as a x-api-key to start working with our API.
//However, this api key is limited in its use, and should not be used in any real application.
Initializer.Initialize( "GyaHV300my60rs2ylKug5aUgFnYBj6GrU6V1WE33", false );
DefinitionAPIClient.LocalStoreDirectory = new DirectoryInfo( @"C:\temp" ); 

var scoreHistoryClient = new ScoreHistoryAPIClient( );

//A ScoreHistory Public Requests returns all publicly avaliable scores for a user.
var scoreHistoryRequest = new GetScoreHistoryPublicRequest();
//Specify a date range
scoreHistoryRequest.StartDate = new DateTime( 2024, 01, 01 );
scoreHistoryRequest.EndDate = new DateTime( 2024, 12, 31 );
//Specify the user
scoreHistoryRequest.UserIds = new List<string>() { "26f32227-d428-41f6-b224-beed7b6e8850" };
//Specify the Event Style to lookup
var eventStyleDef = "v1.0:ntparc:Three-Position Sporter Air Rifle";
scoreHistoryRequest.EventStyleDef = SetName.Parse( eventStyleDef );

GetScoreHistoryPublicResponse scoreHistoryResponse;
do {
    //Make the request
    scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryPublicAsync( scoreHistoryRequest );

    if (scoreHistoryResponse.HasOkStatusCode) {
        foreach (var scoreHistoryBase in scoreHistoryResponse.ScoreHistoryList.Items) {
            //The response returns both ScoreHistoryEventStyleEntry and ScoreHistoryStageStyleEntry. We only want the event styles in this example.
            if (scoreHistoryBase is ScoreHistoryEventStyleEntry) {
                var scoreHistoryEventStyle = (ScoreHistoryEventStyleEntry)scoreHistoryBase;
                var cofSetName = SetName.Parse( scoreHistoryEventStyle.CourseOfFireDef );
                var cofDefinition = await DefinitionCache.GetCourseOfFireDefinitionAsync( cofSetName );
                //Print out the scores
                Console.WriteLine( $"{scoreHistoryEventStyle.MatchName}  {StringFormatting.SpanOfDates(scoreHistoryEventStyle.StartDate, scoreHistoryEventStyle.EndDate)}  {cofDefinition.CommonName}  {scoreHistoryEventStyle.ScoreFormatted}" );
            }
        }

        //Load more data if there is anymore to load.
        if (scoreHistoryResponse.HasMoreItems) {
            scoreHistoryRequest = (GetScoreHistoryPublicRequest)scoreHistoryResponse.GetNextRequest();
        }
    }

} while (scoreHistoryResponse.HasMoreItems);

/*
    Test Qualification  Thu, 19 Dec 2024  3x10 Air Rifle  281 - 8
    Test Qualification  Thu, 19 Dec 2024  3x10 Air Rifle  278 - 7
    Test 3x20 F  Thu, 07 Nov 2024  3x20 plus Final  678.7
    Test AR 3x10  Thu, 07 Nov 2024  3x10 Air Rifle  296.5
    Aggregate Finals Test  Thu, 07 Nov 2024  3x10 Plus Final  392.9
    Bristow Bombers at Baltimore Brewers  Mon, 23 Sep 2024  3x10 Air Rifle  278.8
    Annapolis Anchors at Manassas Maniacs  Mon, 23 Sep 2024  3x10 Air Rifle  290.1
    Baltimore Brewers at Annapolis Anchors  Mon, 23 Sep 2024  3x10 Air Rifle  185.8
    Annapolis Anchors at Bristow Bombers  Mon, 09 Sep 2024  3x10 Air Rifle  279.2
    Orion Scoring System Virtual Match 06 Sep 2024  Fri, 06 Sep 2024  3x10 Air Rifle  280 - 7
    Orion Scoring System Virtual Match 03 Sep 2024  Tue, 03 Sep 2024  3x10 Air Rifle  289.2
    Test 3x20 Decimal  Thu, 29 Aug 2024  3x20 Air Rifle  565.5
    Test Practice Match 3x20  Tue, 27 Aug 2024  3x20 Air Rifle  555 - 13
    Test Local Match  Mon, 26 Aug 2024  3x10 Air Rifle  274 - 8
    Test Match 27 Jun 2024  Thu, 27 Jun 2024  3x10 Air Rifle  274 - 6
    Test Match 6/12  Thu, 13 Jun 2024  3x10 Air Rifle  89 - 2
    The Results List Axiom  Wed, 12 Jun 2024  3x10 Air Rifle  174 - 4
    Test AR 3x10  Wed, 05 Jun 2024  3x10 Air Rifle  279 - 9
    Test VM  Mon, 03 Jun 2024  3x10 Air Rifle  180 - 4
    Test All of the Changes  Wed, 15 May 2024  3x10 Air Rifle  278 - 7
    Test Reduced Result List with VM  Sat, 04 May 2024  3x10 Air Rifle  278 - 8
    Test Result List Ref  Wed, 01 May 2024  3x10 Air Rifle  273 - 10
    Test Result List Ref  Wed, 01 May 2024  3x10 Air Rifle  280 - 11
    Test Rest API  Fri, 26 Apr 2024  3x10 Air Rifle  273 - 8
    Test AR 3x10  Wed, 06 Mar 2024  3x10 Air Rifle  282 - 11
    Test VM AR 3x10  Wed, 06 Mar 2024  3x10 Air Rifle  280 - 7
    Test Reduced Data Packet  Sun, 03 Mar 2024  3x10 Air Rifle  276 - 7
    Test Spectator Display  Fri, 01 Mar 2024  3x10 Air Rifle  270 - 5
    Test KPS VM  Wed, 28 Feb 2024  3x10 Air Rifle  277 - 7
    Match 08 Feb 2024  Thu, 08 Feb 2024  3x10 Air Rifle  278 - 9
    Test 3x10 PSK Plus Final  Wed, 17 Jan 2024  3x10 plus Final  376.2
    Test AR 3x20 PSK plus Final  Wed, 17 Jan 2024  3x20 plus Final  644.7
    Test 3x20 AR PSK  Tue, 16 Jan 2024  3x20  568 - 22
    Test Air Rifle 3x20 PSK  Sat, 13 Jan 2024  3x20  558 - 17
    Test Air Rifle 3x20  Fri, 12 Jan 2024  3x20 Air Rifle  558 - 13
*/

Console.WriteLine( "Press any key to close." );
Console.ReadKey();

```

### DEFINITIONS
Demonstrates how to load definition files, specifically COURSE OF FIRE, RESULT LIST FORMAT, and RANKING RULES, and learn stuff about the definition.

```csharp
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Runtime;


//You may use GyaHV300my60rs2ylKug5aUgFnYBj6GrU6V1WE33 as a x-api-key to start working with our API.
//However, this api key is limited in its use, and should not be used in any real application.
Initializer.Initialize( "GyaHV300my60rs2ylKug5aUgFnYBj6GrU6V1WE33", false );
DefinitionAPIClient.LocalStoreDirectory = new System.IO.DirectoryInfo( @"C:\temp" );

//A SetName uniquely ideentifies a definition
var threePositionCourseOfFireSetName = SetName.Parse( "v3.0:ntparc:Three-Position Air Rifle 3x20" );

//Retreives the COURSE OF FIRE definition.
var threePositionCourseOfFire = await DefinitionCache.GetCourseOfFireDefinitionAsync( threePositionCourseOfFireSetName );

//Print basic information about this definition
Console.WriteLine( threePositionCourseOfFire.CommonName );
Console.WriteLine( threePositionCourseOfFire.Description );
Console.WriteLine( threePositionCourseOfFire.Discipline );
Console.WriteLine();

/*
3x20 Air Rifle
Three-Position Air Rifle 3x20, position ordering for K-P-S
RIFLE
*/

//Build the Event Tree which is the structure of the course of fire.
var topLevelEvent = EventComposite.GrowEventTree( threePositionCourseOfFire );

//Print out the stages to the COF
foreach (var stage in topLevelEvent.GetEvents( EventtType.STAGE )) {
    Console.WriteLine( $"{stage.EventName} has {stage.GetAllSingulars().Count} number of shots." );

    //Load the recommended RESULT LIST FORMAT definition
    var resultListFormatSetName = SetName.Parse( stage.ResultListFormatDef );
    var resultListFormat = await DefinitionCache.GetResultListFormatDefinitionAsync( resultListFormatSetName );
    Console.WriteLine( $"The recommended RESULT LIST FORMAT is '{resultListFormat.CommonName}' which has a total of {resultListFormat.Format.Columns.Count} columns." );

    //Load the default RANKING RULE
    var rankingRuleSetName = SetName.Parse( stage.RankingRuleMapping["DefaultDef"] );
    var rankingRule = await DefinitionCache.GetRankingRuleDefinitionAsync( rankingRuleSetName );
    Console.WriteLine( $"The recommended RANKING RULE for this event is '{rankingRule.CommonName}' which defines {rankingRule.RankingRules[0].Rules.Count} rules." );
}

/*
    Kneeling has 20 number of shots.
    The recommended RESULT LIST FORMAT is 'Kneeling 20 Shots' which has a total of 5 columns.
    The recommended RANKING RULE for this event is 'Generic Decimal Kneeling' which defines 3 rules.
    Prone has 20 number of shots.
    The recommended RESULT LIST FORMAT is 'Prone 20 Shots' which has a total of 5 columns.
    The recommended RANKING RULE for this event is 'Generic Decimal Prone' which defines 3 rules.
    Standing has 20 number of shots.
    The recommended RESULT LIST FORMAT is 'Standing 20 Shots' which has a total of 5 columns.
    The recommended RANKING RULE for this event is 'Generic Decimal Standing' which defines 3 rules.
*/

Console.WriteLine();
//Print out each range command
foreach (var sg in threePositionCourseOfFire.RangeScripts[0].SegmentGroups) {
    foreach (var command in sg.Commands) {
        Console.WriteLine( command.Command );
    }
}

/*
    Welcome to the {MatchName} Three-Position Air Rifle Match
    Relay Number {RelayNumber} you may move your rifles and equipment to the firing line.
    You may uncase and handle your rifles.
    Take your positions.
    Your 8 minute preparation and sighting time for the kneeling position starts when your green signal light appears and ends when your red light reappears.
    Sighting shots ... START
    START
    30 seconds
    Sighting shots ... STOP
    Your 20 minute for 20 shots kneeling match firing time starts when your green signal light appears and ends when your red light reappears.
    Kneeling match firing ... START
    START
    Five minutes.
    Two minutes.
    STOP - UNLOAD
    Is the line clear?
    The line is clear
    Your 5 minute changeover time for the prone position begins now.
    Take your positions.
    Your 5 minute sighting time for the prone position starts when your green signal light appears and ends when your red light reappears.
    Sighting shots ... START
    START
    30 seconds
    Sighting shots ... STOP
    Your 20 minute for 20 shots prone match firing time starts when your green signal light appears and ends when your red light reappears.
    Prone match firing ... START
    START
    Five minutes.
    Two minutes.
    STOP - UNLOAD
    Is the line clear?
    The line is clear
    Your 5 minute changeover time for the Standing position begins now.
    Take your positions.
    Your 5 minute sighting time for the standing position starts when your green signal light appears and ends when your red light reappears.
    Sighting shots ... START
    START
    30 seconds
    Sighting shots ... STOP
    Your 25 minute for 20 shots standing match firing time starts when your green signal light appears and ends when your red light reappears.
    Standing match firing ... START
    START
    Five minutes.
    Two minutes.
    STOP - UNLOAD
    Is the line clear?
    The line is clear
    Athletes, you may remove your equipment from the firing line
    You may discharge air or gas downrange.
*/
```
## Recommended NLog Configuration

BabelFish uses NLog for internal diagnostics, request tracing, and error reporting. While BabelFish will run without any logging configuration, we recommend that applications include an NLog.config file so developers can monitor API calls, troubleshoot issues, and capture detailed runtime information.

The example configuration below provides:
- Console and rolling‑file logging
- Sensible defaults (Warn and above)
- Optional Debug‑level logging for troubleshooting
- Logger rules that match BabelFish’s internal namespaces

### Basic NLog.config Example
Create a file named NLog.config in your application project:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      autoReload="true"
      throwConfigExceptions="false">

  <!-- Define where logs are written -->
  <targets>
    <!-- Console output -->
    <target xsi:type="Console"
            name="console"
            layout="${longdate} | ${level} | ${logger} | ${message} ${exception}" />

    <!-- Rolling log file -->
    <target xsi:type="File"
            name="file"
            fileName="logs/babelfish.log"
            archiveEvery="Day"
            maxArchiveFiles="7"
            layout="${longdate} | ${level} | ${logger} | ${message} ${exception}" />
  </targets>

  <!-- Define logging rules -->
  <rules>
    <!-- Default: warnings and errors -->
    <logger name="Scopos.BabelFish.*"
            minlevel="Warn"
            writeTo="console,file" />

    <!-- Enable this rule for detailed debugging -->
    <!--
    <logger name="Scopos.BabelFish.*"
            minlevel="Debug"
            writeTo="console,file" />
    -->
  </rules>

</nlog>
```

### Enabling Debug Logging
If you need to troubleshoot API calls, data formatting, or definition loading, change:
minlevel="Warn"

to:
minlevel="Debug"


This enables detailed logs such as:
- REST API request URLs
- Response status codes
- Cache hits/misses
- Result list formatting steps
- Definition loading and parsing
Log Output Location
The example above writes logs to:
logs/babelfish.log


You may change this path to suit your application’s structure.
Why Logging Matters
BabelFish interacts with multiple Scopos services, caches definitions, formats complex result lists, and performs multi‑step calculations. Logging helps you:
- Diagnose API failures
- Understand result list formatting behavior
- Track down missing definitions
- Debug score history queries
- Capture unexpected exceptions
For production applications, we recommend enabling at most Warn‑level logging.

