# BabelFish
Dot Net Library that provides a façade for Shooter's Tech REST API interface.

While the code is stable and used in Scopos' production environment, the accompnaing documentation is not ready yet.

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

The following examples are part of our [scopos-labs]([url](https://github.com/shooterstech/scopos-labs/tree/master/csharp/Command%20Line%20Examples)) open source example repository.

### ORION MATCH API

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

### DEFINITIONS

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
