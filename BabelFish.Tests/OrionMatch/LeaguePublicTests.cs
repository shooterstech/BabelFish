using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Responses.OrionMatchAPI;

namespace Scopos.BabelFish.Tests.OrionMatch {
    [TestClass]
    public class LeaguePublicTests {

        [TestInitialize]
        public void InitializeTest() {
            Scopos.BabelFish.Runtime.Settings.XApiKey = Constants.X_API_KEY;
        }

        /// <summary>
        /// Pass in a league id and check the response contains expected data.
        /// Uses a production league id, as these are more complete. 
        /// </summary>
        [TestMethod]
        public async Task GetLeagueDetailPublicTests() {

            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );

            //Pass in a fake match id
            var leagueDetailResponse = await client.GetLeagueDetailPublicAsync( "1.1.2023091512010588.3" );

            Assert.AreEqual( HttpStatusCode.OK, leagueDetailResponse.StatusCode );

            var league = leagueDetailResponse.League;
            Assert.IsNotNull( league );

            Assert.AreEqual( "National Precision Air Rifle League", league.LeagueNetworkName );
            Assert.AreEqual( "2023", league.SeasonName );
            Assert.AreEqual( "2023 National Precision Air Rifle League", league.LeagueName );
            Assert.AreEqual( "League", league.MatchType );
            Assert.AreEqual( LeagueSeasonType.REGULAR, league.SeasonType );
            Assert.AreEqual( new DateTime( 2023, 9, 25 ), league.StartDate );
            Assert.AreEqual( new DateTime( 2023, 11, 19 ), league.EndDate );
        }

        /// <summary>
        /// Searches for all the bye weeks a team in the league has. 
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetByeWeeksForLeagueTeam() {

            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );

            //Given these parameters, there whould only be one bye week returned.
            var request = new GetLeagueGamesPublicRequest( "1.1.2023091512010588.3" ) {
                ByeWeeks = true,
                TeamId = 2208
            };

            var response = await client.GetLeagueGamesPublicAsync ( request );

            Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

            var leagueGameList = response.LeagueGames;

            Assert.AreEqual( "National Precision Air Rifle League", leagueGameList.LeagueNetworkName );
            Assert.AreEqual( "2023", leagueGameList.SeasonName );
            Assert.AreEqual( "2023 National Precision Air Rifle League", leagueGameList.LeagueName );

            foreach ( var game in leagueGameList.Items ) {
                Assert.AreEqual( "1.1.2023100215285812.1", game.GameID );
                Assert.AreEqual( LeagueVirtualType.BYE_WEEK, game.Virtual );
                Assert.AreEqual( new DateTime( 2023, 10, 09 ), game.StartDate );
                Assert.AreEqual( new DateTime( 2023, 10, 15 ), game.EndDate );

                var homeTeam = game.HomeTeam;
                var awayTeam = game.AwayTeam; //As this is a bye week, the Away Team should exist, but largley empty.

                Assert.AreEqual( 2208, homeTeam.Team.TeamID );
                Assert.AreEqual( "American Legion Post 295", homeTeam.Team.TeamName );
            }
        }

        /// <summary>
        /// Checks that we can get a list of games, with the teams filtered by a specified Conference
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetLeagueGamesFilterByConference() {


            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );

            //Given these parameters, there whould only be one bye week returned.
            var request = new GetLeagueGamesPublicRequest( "1.1.2023091512010588.3" ) {
                Conference = "Junior Rifle Club"
            };

            var response = await client.GetLeagueGamesPublicAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

            var leagueGameList = response.LeagueGames;

            Assert.AreEqual( "National Precision Air Rifle League", leagueGameList.LeagueNetworkName );
            Assert.AreEqual( "2023", leagueGameList.SeasonName );
            Assert.AreEqual( "2023 National Precision Air Rifle League", leagueGameList.LeagueName );

            //Check that at least one team in each game is from the specified Conference
            foreach( var game in leagueGameList.Items ) {
                bool jrc = false;
                jrc |= game.HomeTeam.Team.Conference == "Junior Rifle Club";
                jrc |= game.AwayTeam.Team.Conference == "Junior Rifle Club";
                Assert.IsTrue( jrc );  
            }
        }

        /// <summary>
        /// Checks that we can get a list of games, with the teams filtered by a specified Division
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetLeagueGamesFilterByDivision() {


            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );

            //Given these parameters, there whould only be one bye week returned.
            var request = new GetLeagueGamesPublicRequest( "1.1.2023091512010588.3" ) {
                Division = "Champions"
            };

            var response = await client.GetLeagueGamesPublicAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

            var leagueGameList = response.LeagueGames;

            Assert.AreEqual( "National Precision Air Rifle League", leagueGameList.LeagueNetworkName );
            Assert.AreEqual( "2023", leagueGameList.SeasonName );
            Assert.AreEqual( "2023 National Precision Air Rifle League", leagueGameList.LeagueName );

            //Check that at least one team in each game is from the specified Division
            foreach (var game in leagueGameList.Items) {
                bool jrc = false;
                jrc |= game.HomeTeam.Team.Division == "Champions";
                jrc |= game.AwayTeam.Team.Division == "Champions";
                Assert.IsTrue( jrc );
            }
        }


        /// <summary>
        /// Checks that the tokenization works as expected
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetLeagueGamesTokenization() {


            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );

            var request = new GetLeagueGamesPublicRequest( "1.1.2023091512052862.3" ); // 1.1.2023091512010588.3" );

            GetLeagueGamesPublicResponse response;
            List<LeagueGame> allTheGames = new List<LeagueGame>();
            string lastToken = "";

            do {
                response = await client.GetLeagueGamesPublicAsync( request );

                //Expect a OK response
                Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

                allTheGames.AddRange( response.LeagueGames.Items );

                //Expect this token to be different from the last token
                Assert.AreNotEqual( lastToken, response.LeagueGames.NextToken );
                lastToken = response.LeagueGames.NextToken;

                //Check if there is more data to return, if so prepare the next request.
                if (response.LeagueGames.HasMoreItems) {
                    request = response.GetNextRequest();
                }
            } while (response.LeagueGames.HasMoreItems);

            //Now go through and check that each returned game is unique.
            Dictionary<string, bool> seenGames = new Dictionary<string, bool>();
            foreach( var game in allTheGames ) {
                Assert.IsFalse( seenGames.ContainsKey( game.GameID ) );
                seenGames[game.GameID] = true;
            }

        }

        /// <summary>
        /// Basic test for retreiving a list of teams from a league.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetLeagueTeamsBasicTest() {

            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );

            var request = new GetLeagueTeamsPublicRequest( "1.1.2023091512010588.3" );

            var response = await client.GetLeagueTeamsPublicAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

            var leagueTeamList = response.LeagueTeams;

            Assert.AreEqual( "National Precision Air Rifle League", leagueTeamList.LeagueNetworkName );
            Assert.AreEqual( "2023", leagueTeamList.SeasonName );
            Assert.AreEqual( "2023 National Precision Air Rifle League", leagueTeamList.LeagueName );

            foreach (var team in leagueTeamList.Items) {
                Assert.AreNotEqual( 0, team.TeamID );
                Assert.AreNotEqual( "", team.TeamName );
                Assert.AreNotEqual( "", team.Hometown );

                Assert.AreEqual( 0, team.Schedule.Count );
            }
        }


        /// <summary>
        /// Checks that the tokenization works as expected
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetLeagueTeamsTokenization() {


            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );

            var request = new GetLeagueTeamsPublicRequest( "1.1.2023091512010588.3" ) {
                Limit = 10 //Set a small limit so there will be a token on the first call.
            };

            GetLeagueTeamsPublicResponse response;
            List<LeagueTeam> allTheTeams = new List<LeagueTeam>();
            string lastToken = "";

            do {
                response = await client.GetLeagueTeamsPublicAsync( request );

                //Expect a OK response
                Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

                allTheTeams.AddRange( response.LeagueTeams.Items );

                //Expect this token to be different from the last token
                Assert.AreNotEqual( lastToken, response.LeagueTeams.NextToken );
                lastToken = response.LeagueTeams.NextToken;

                //Check if there is more data to return, if so prepare the next request.
                if (response.LeagueTeams.HasMoreItems) {
                    request = response.GetNextRequest();
                }
            } while (response.LeagueTeams.HasMoreItems);

            //Now go through and check that each returned team is unique.
            Dictionary<int, bool> seenTeams = new Dictionary<int, bool>();
            foreach (var team in allTheTeams) {
                Assert.IsFalse( seenTeams.ContainsKey( team.TeamID ) );
                seenTeams[team.TeamID] = true;
            }

        }

        /// <summary>
        /// Basic test for retreiving a League Team Detail
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetLeagueTeamDetailBasicTest() {

            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );

            var request = new GetLeagueTeamDetailPublicRequest( "1.1.2023091512010588.3", 2213 );

            var response = await client.GetLeagueTeamDetailPublicAsync( request );

            Assert.AreEqual( HttpStatusCode.OK, response.StatusCode );

            var leagueTeamDetail = response.LeagueTeamDetail;

            Assert.AreEqual( "National Precision Air Rifle League", leagueTeamDetail.LeagueNetworkName );
            Assert.AreEqual( "2023", leagueTeamDetail.SeasonName );
            Assert.AreEqual( "2023 National Precision Air Rifle League", leagueTeamDetail.LeagueName );

            Assert.AreEqual( 2213, leagueTeamDetail.LeagueTeam.TeamID );
            Assert.AreEqual( "Hereford MCJROTC", leagueTeamDetail.LeagueTeam.TeamName );
            Assert.AreEqual( 9, leagueTeamDetail.LeagueTeam.Schedule.Count );

            foreach( var game in leagueTeamDetail.LeagueTeam.Schedule ) {
                Assert.AreNotEqual( "", game.GameID );
                Assert.AreNotEqual( "", game.GameName );
            }
        }

        /// <summary>
        /// Basic test for retreiving a League Team Detail
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetLeagueConfAndDivLists()
        {

            var client = new OrionMatchAPIClient(APIStage.PRODUCTION);

            var requestTeamDetail = new GetLeagueTeamDetailPublicRequest("1.1.2023091512010588.3", 2213);
            var requestGames = new GetLeagueGamesPublicRequest("1.1.2023091512010588.3");

            var responseTeamDetail = await client.GetLeagueTeamDetailPublicAsync(requestTeamDetail);
            var responseGames = await client.GetLeagueGamesPublicAsync(requestGames);
            var leagueDetailResponse = await client.GetLeagueDetailPublicAsync("1.1.2023091512010588.3");

            Assert.AreEqual(HttpStatusCode.OK, responseTeamDetail.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, responseGames.StatusCode);
            Assert.AreEqual(HttpStatusCode.OK, leagueDetailResponse.StatusCode);

            var leagueTeamDetail = responseTeamDetail.LeagueTeamDetail;
            var leagueGame = responseGames.LeagueGames;
            var leagueDetail = leagueDetailResponse.League;
            Assert.IsNotNull( leagueTeamDetail );
            Assert.IsNotNull( leagueGame );
            Assert.IsNotNull( leagueDetail );

            List<string> confList = new string[] { "Junior Rifle Club", "Jrotc" }.ToList();
            /*
            confList[0] = "Junior Rifle Club";
            confList[1] = "Jrotc";
            */
            //USE CollectionAssert when comparing lists. 
            //not sure why that ISNT part of the Assert class, but it does not differentiate at all.... but hey this works lol
            CollectionAssert.AreEqual(confList, leagueTeamDetail.ConferenceList);
            CollectionAssert.AreEqual(confList, leagueGame.ConferenceList);
            CollectionAssert.AreEqual(confList, leagueDetail.ConferenceList);

            List<string> divList = new string[] { "Champions", "Distinguished" }.ToList();
            /*
            divList[0] = "Champions";
            divList[1] = "Distinguished";
            */

            CollectionAssert.AreEqual(divList, leagueTeamDetail.DivisionList);
            CollectionAssert.AreEqual(divList, leagueGame.DivisionList);
            CollectionAssert.AreEqual(divList, leagueDetail.DivisionList);
        }

        [TestMethod]
        public async Task GenerateLeagueWeeksTest() {


            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );
            //2024 National Air Pistol League
            var response = await client.GetLeagueDetailPublicAsync( "1.1.2023122017132108.3" );
            var league = response.League;

            var leagueWeeks = league.LeagueWeeks;

            //Should have 10 weeks
            Assert.IsTrue( leagueWeeks.Count == 10 );

            int weekNumber = 1;
            foreach( var lw in leagueWeeks ) {
                Assert.AreEqual( $"Week {weekNumber}", lw.Name );
                weekNumber++;
            }
        }

        [TestMethod]
        public async Task DefaultValuesForListOfLeagueWeeksTest( ) {


            var client = new OrionMatchAPIClient( APIStage.PRODUCTION );
            //2024 National Air Pistol League
            var response = await client.GetLeagueDetailPublicAsync( "1.1.2023122017132108.3" );
            var league = response.League;

            var leagueWeeks = league.LeagueWeeks;

            //Today should be well after week 10
            Assert.AreEqual( "Week 10", leagueWeeks.Default().Name );

            //A date before the league
            Assert.AreEqual( "Week 1", leagueWeeks.Default( new DateTime(2020, 1, 1 ) ).Name );

            //Dates within the first and second week
            Assert.AreEqual( "Week 1", leagueWeeks.Default( new DateTime( 2024, 1, 29 ) ).Name );
            Assert.AreEqual( "Week 1", leagueWeeks.Default( new DateTime( 2024, 2, 3 ) ).Name );
            Assert.AreEqual( "Week 1", leagueWeeks.Default( new DateTime( 2024, 2, 4 ) ).Name );
            Assert.AreEqual( "Week 2", leagueWeeks.Default( new DateTime( 2024, 2, 5 ) ).Name );
            Assert.AreEqual( "Week 2", leagueWeeks.Default( new DateTime( 2024, 2, 6 ) ).Name );
            Assert.AreEqual( "Week 2", leagueWeeks.Default( new DateTime( 2024, 2, 11 ) ).Name );

        }

        [TestMethod]
        public async Task TestShowLeaderboardLink()
        {
            var league = new LeagueGame();
            league.Virtual = LeagueVirtualType.VIRTUAL;
            league.HomeTeam = new LeagueTeamResult();
            league.AwayTeam = new LeagueTeamResult();
            league.HomeTeam.Result = "";
            league.AwayTeam.Result = "";
            Assert.AreEqual(true, league.ShowLeaderboard);

            league.Virtual = LeagueVirtualType.LOCAL;
            league.HomeTeam.Result = "";
            league.AwayTeam.Result = "";
            Assert.AreEqual(true, league.ShowLeaderboard);

            league.Virtual = LeagueVirtualType.BYE_WEEK;
            league.HomeTeam.Result = "";
            league.AwayTeam.Result = "";
            Assert.AreEqual(false, league.ShowLeaderboard);

            league.Virtual = LeagueVirtualType.VIRTUAL;
            league.HomeTeam.Result = "WIN";
            league.AwayTeam.Result = "LOSE";
            Assert.AreEqual(false, league.ShowLeaderboard);

            league.Virtual = LeagueVirtualType.LOCAL;
            league.HomeTeam.Result = "WIN";
            league.AwayTeam.Result = "LOSE";
            Assert.AreEqual(false, league.ShowLeaderboard);

            league.Virtual = LeagueVirtualType.LOCAL;
            league.HomeTeam.Result = "CANCELLED";
            league.AwayTeam.Result = "CANCELLED";
            Assert.AreEqual(false, league.ShowLeaderboard);

            league.Virtual = LeagueVirtualType.LOCAL;
            league.HomeTeam.Result = "DNS";
            league.AwayTeam.Result = "WIN";
            Assert.AreEqual(false, league.ShowLeaderboard);
        }
    }
}
