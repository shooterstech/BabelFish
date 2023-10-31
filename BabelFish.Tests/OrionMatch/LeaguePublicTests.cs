﻿using System;
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

        /// <summary>
        /// Pass in a league id and check the response contains expected data.
        /// Uses a production league id, as these are more complete. 
        /// </summary>
        [TestMethod]
        public async Task GetLeagueDetailPublicTests() {

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );

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

            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );

            //Given these parameters, there whould only be one bye week returned.
            var request = new GetLeagueGameListPublicRequest( "1.1.2023091512010588.3" ) {
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

                Assert.AreEqual( LeagueTeam.ByeWeekTeamID, awayTeam.Team.TeamID );
            }
        }

        /// <summary>
        /// Checks that we can get a list of games, with the teams filtered by a specified Conference
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetLeagueGamesFilterByConference() {


            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );

            //Given these parameters, there whould only be one bye week returned.
            var request = new GetLeagueGameListPublicRequest( "1.1.2023091512010588.3" ) {
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


            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );

            //Given these parameters, there whould only be one bye week returned.
            var request = new GetLeagueGameListPublicRequest( "1.1.2023091512010588.3" ) {
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


            var client = new OrionMatchAPIClient( Constants.X_API_KEY, APIStage.PRODUCTION );

            //Given these parameters, there whould only be one bye week returned.
            var request = new GetLeagueGameListPublicRequest( "1.1.2023091512010588.3" );

            GetLeagueGameListPublicResponse response;
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
    }
}
