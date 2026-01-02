using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Athena.Shot;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Responses.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;
using SkiaSharp;

namespace Scopos.BabelFish.Tests.OrionMatch {
    [TestClass]
    public class PostShotDataTests : BaseTestClass {

        [TestMethod]
        public async Task EriksPlayground() {

            var matchClient = new OrionMatchAPIClient();
            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );

            await userAuthentication.InitializeAsync();
            var matchId = new MatchID( "1.1.2025121213504726.0" );
            var getMatchDetailResponse = await matchClient.GetMatchAuthenticatedAsync( matchId, userAuthentication );
            Assert.IsNotNull( getMatchDetailResponse );
            Assert.IsTrue( getMatchDetailResponse.HasOkStatusCode );

            var matchObj = getMatchDetailResponse.Match;
            var cofDefinition = await DefinitionCache.GetCourseOfFireDefinitionAsync( SetName.Parse( matchObj.CourseOfFireDef ) );
            var targetCollectionDefinition = await cofDefinition.GetTargetCollectionDefinitionAsync();
            var scoreFormatDefinition = await cofDefinition.GetScoreFormatCollectionDefinitionAsync();
            var randomNumber = new RandomGaussianNumberGenerator();

            //Retreive the Squadding for the match
            GetSquaddingListPublicRequest getSquaddingRequest = new GetSquaddingListPublicRequest(matchId, matchObj.SquaddingEvents[0].Name );
            GetSquaddingListPublicResponse getSquaddingResponse;
            SquaddingList? squaddingList = new SquaddingList();
            do {
                getSquaddingResponse = await matchClient.GetSquaddingListPublicAsync( getSquaddingRequest );
                if (getSquaddingResponse.HasOkStatusCode) {
                    if (squaddingList == null) {
                        squaddingList = getSquaddingResponse.SquaddingList;
                    } else {
                        squaddingList.Items.AddRange( getSquaddingResponse.SquaddingList.Items );
                    }
                }
            } while ( getSquaddingResponse.HasOkStatusCode && getSquaddingResponse.HasMoreItems );

            //Sort relay information by its name.
            CompareRelayInformation relayInfoComparer = new CompareRelayInformation( CompareRelayInformation.CompareMethod.RELAY_NAME, SortBy.ASCENDING );
            squaddingList.GenerateRelayInformation();
            squaddingList.RelayInformation.Sort( relayInfoComparer );
            
            var cofTree = EventComposite.GrowEventTree( cofDefinition );
            //Foreach relay in the match
            foreach (var relayInformation in squaddingList.RelayInformation) {
                //Foreach athlete on that relay
                foreach (var athlete in squaddingList.FilterByRelayInformation( relayInformation )) {
                    //Generate shots for that athlete
                    var sequence = 1;
                    foreach (var stage in cofTree.GetEvents( EventtType.STAGE )) {
                        var targetDefinition = await stage.GetTargetAsync( cofDefinition, matchObj.TargetCollectionName );
                        var tenRingDiameter = targetDefinition.ScoringRings[1].Dimension;
                        Console.WriteLine( $"Generating shots for {stage.EventName}. using {targetDefinition.CommonName}" );
                        PostShotDataAuthenticatedRequest request = new PostShotDataAuthenticatedRequest( userAuthentication, matchId );
                        foreach (var singular in stage.GetAllSingulars()) {
                            var x = (float)randomNumber.NextGaussian( 0, tenRingDiameter );
                            var y = (float)randomNumber.NextGaussian( 0, tenRingDiameter );
                            var score = targetDefinition.Score( x, y, cofDefinition.DefaultScoringDiameter );
                            Console.WriteLine( $"{x:F1} {y:F1} {score}" );

                            var shot = new Shot() {
                                Score = score,
                                TargetName = "EriksPlayground",
                                TargetSetName = targetDefinition.SetName,
                                TimeScored = DateTime.UtcNow,
                                BulletDiameter = cofDefinition.DefaultExpectedDiameter,
                                ScoringDiameter = cofDefinition.DefaultScoringDiameter,
                                Location = new Location() {
                                    X = x,
                                    Y = y
                                },
                                Sequence = sequence++,
                                ResultCOFID = ((Individual) athlete.Participant).ResultCOFID,
                                MatchID = matchId,
                                StageLabel = singular.StageLabel,
                                RangeTime = "0:00:00",
                                FiringPoint = ((SquaddingAssignmentFiringPoint) athlete.SquaddingAssignment).FiringPoint,
                                Privacy = matchObj.Visibility,
                                ScoreFormatted = StringFormatting.FormatScore( scoreFormatDefinition, matchObj.ScoreConfigName, singular.ScoreFormat, score )
                            };
                            shot.Meta = new System.Dynamic.ExpandoObject();
                            ((dynamic)shot.Meta).ESTSystem = "Bullseye";
                            request.Shots.Add( shot );
                        }
                        var response = await matchClient.PostShotDataAuthenticatedAsync( request );

                        Assert.IsNotNull( response );
                        Assert.IsTrue( response.HasOkStatusCode );

                        Console.WriteLine( $"{response.AcceptedShots.Count} shots acepted. {response.RejectedShots.Count} shots rejected." );
                        foreach (var rejectedShot in response.RejectedShots) {
                            var rejectedReasons = string.Join( ", ", rejectedShot.RejectionMessages );
                            Console.WriteLine( $"Shot sequence {rejectedShot.Sequence} rejected with reasones {rejectedReasons}" );
                        }
                        Console.WriteLine( $"OrionMatchAPI calls {OrionMatchAPIClient.Statistics.NumberOfApiCalls}." );
                        Console.WriteLine( $"DefinitionAPI calls {DefinitionAPIClient.Statistics.NumberOfApiCalls}." );
                    }
                }
            }
        }
    }
}
