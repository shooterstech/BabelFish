using System.Diagnostics;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.Definition {

    [TestClass]
    public class RankingRuleTests : BaseTestClass {

        /// <summary>
        /// Basic Unit Tests for when TieBreakingRules.Method is "Score".
        /// </summary>
        [TestMethod]
        public void CompareScore() {
            var cof = CourseOfFireHelper.Get_3x20_KPS_Cof();

            var x = new ResultEvent() {
                Participant = new Individual() {
                    DisplayName = "John"
                },
                EventScores = new Dictionary<string, EventScore>()
            };
            x.EventScores.Add( "Qualification", new EventScore() {
                EventName = "Qualification",
                Score = new DataModel.Athena.Score() {
                    I = 94,
                    D = 98.6f,
                    X = 5,
                    S = 98.6f
                }
            } );

            var y = new ResultEvent() {
                Participant = new Individual() {
                    DisplayName = "Lucy"
                },
                EventScores = new Dictionary<string, EventScore>()
            };
            y.EventScores.Add( "Qualification", new EventScore() {
                EventName = "Qualification",
                Score = new DataModel.Athena.Score() {
                    I = 98,
                    D = 102.2f,
                    X = 4,
                    S = 102.2f
                }
            } );

            var z = new ResultEvent() {
                Participant = new Individual() {
                    DisplayName = "LucyClone"
                },
                EventScores = new Dictionary<string, EventScore>()
            };
            z.EventScores.Add( "Qualification", new EventScore() {
                EventName = "Qualification",
                Score = new DataModel.Athena.Score() {
                    I = 98,
                    D = 102.2f,
                    X = 4,
                    S = 102.2f
                }
            } );

            var integerDescending = new TieBreakingRuleScore() {
                SortOrder = SortBy.DESCENDING,
                EventName = "Qualification",
                Source = TieBreakingRuleScoreSource.I
            };

            var integerAscending = new TieBreakingRuleScore() {
                SortOrder = SortBy.ASCENDING,
                EventName = "Qualification",
                Source = TieBreakingRuleScoreSource.I
            };

            var decimalDescending = new TieBreakingRuleScore() {
                SortOrder = SortBy.DESCENDING,
                EventName = "Qualification",
                Source = TieBreakingRuleScoreSource.D
            };

            var decimalAscending = new TieBreakingRuleScore() {
                SortOrder = SortBy.ASCENDING,
                EventName = "Qualification",
                Source = TieBreakingRuleScoreSource.D
            };

            var xDescending = new TieBreakingRuleScore() {
                SortOrder = SortBy.DESCENDING,
                EventName = "Qualification",
                Source = TieBreakingRuleScoreSource.X
            };

            var xAscending = new TieBreakingRuleScore() {
                SortOrder = SortBy.ASCENDING,
                EventName = "Qualification",
                Source = TieBreakingRuleScoreSource.X
            };

            var directive = new RankingDirective();
            var comparer = new CompareByRankingDirective( cof, directive );

            //Integer Descending, X's integer score is < Y's, Z's score is equal to Y's score
            directive.Rules.Clear();
            directive.Rules.Add( integerDescending );
            Assert.IsTrue( comparer.Compare( x, y ) > 0 );
            Assert.IsTrue( comparer.Compare( y, x ) < 0 );
            Assert.IsTrue( comparer.Compare( z, y ) == 0 );

            //Integer Ascending, X's integer score is < Y's
            directive.Rules.Clear();
            directive.Rules.Add( integerAscending );
            Assert.IsTrue( comparer.Compare( x, y ) < 0 );
            Assert.IsTrue( comparer.Compare( y, x ) > 0 );
            Assert.IsTrue( comparer.Compare( z, y ) == 0 );

            //Decimal Descending, X's decimal score is < Y's
            directive.Rules.Clear();
            directive.Rules.Add( decimalDescending );
            Assert.IsTrue( comparer.Compare( x, y ) > 0 );
            Assert.IsTrue( comparer.Compare( y, x ) < 0 );
            Assert.IsTrue( comparer.Compare( z, y ) == 0 );

            //Decimal Ascending, X's decimal score is < Y's
            directive.Rules.Clear();
            directive.Rules.Add( decimalAscending );
            Assert.IsTrue( comparer.Compare( x, y ) < 0 );
            Assert.IsTrue( comparer.Compare( y, x ) > 0 );
            Assert.IsTrue( comparer.Compare( z, y ) == 0 );

            //Inner Tens Descending, X's inner ten count is > Y's
            directive.Rules.Clear();
            directive.Rules.Add( xDescending );
            Assert.IsTrue( comparer.Compare( x, y ) < 0 );
            Assert.IsTrue( comparer.Compare( y, x ) > 0 );
            Assert.IsTrue( comparer.Compare( z, y ) == 0 );

            //Inner Tens Ascending, X's inner ten count is > Y's
            directive.Rules.Clear();
            directive.Rules.Add( xAscending );
            Assert.IsTrue( comparer.Compare( x, y ) > 0 );
            Assert.IsTrue( comparer.Compare( y, x ) < 0 );
            Assert.IsTrue( comparer.Compare( z, y ) == 0 );
        }
        /// <summary>
        /// Basic Unit Tests for when TieBreakingRules.Method is "Participant Attribute".
        /// </summary>
        [TestMethod]
        public void CompareParticipant() {
            var cof = CourseOfFireHelper.Get_3x20_KPS_Cof();

            var johnSmith = new ResultEvent() {
                Participant = new Individual() {
                    DisplayName = "Smith, John",
                    FirstName = "John",
                    LastName = "Smith",
                    DisplayNameShort = "Smith, J",
                    CompetitorNumber = "101",
                    Country = "JJS"
                },
                EventScores = new Dictionary<string, EventScore>()
            };

            var lucyJones = new ResultEvent() {
                Participant = new Individual() {
                    DisplayName = "Jones, Lucy",
                    FirstName = "Lucy",
                    LastName = "Jones",
                    DisplayNameShort = "Jones, L",
                    CompetitorNumber = "102",
                    Country = "LLJ"
                },
                EventScores = new Dictionary<string, EventScore>()
            };

            var craigJones = new ResultEvent() {
                Participant = new Individual() {
                    DisplayName = "Jones, Craig",
                    FirstName = "Craig",
                    LastName = "Jones",
                    DisplayNameShort = "Jones, C",
                    CompetitorNumber = "103",
                    Country = "CCJ"
                },
                EventScores = new Dictionary<string, EventScore>()
            };

            var familyName = new TieBreakingRuleParticipantAttribute() {
                SortOrder = SortBy.ASCENDING,
                Source = TieBreakingRuleParticipantAttributeSource.FamilyName
            };

            var givenName = new TieBreakingRuleParticipantAttribute() {
                SortOrder = SortBy.ASCENDING,
                Source = TieBreakingRuleParticipantAttributeSource.GivenName
            };

            var familyNameDesc = new TieBreakingRuleParticipantAttribute() {
                SortOrder = SortBy.DESCENDING,
                Source = TieBreakingRuleParticipantAttributeSource.FamilyName
            };

            var compeNumber = new TieBreakingRuleParticipantAttribute() {
                SortOrder = SortBy.ASCENDING,
                Source = TieBreakingRuleParticipantAttributeSource.CompetitorNumber
            };

            var country = new TieBreakingRuleParticipantAttribute() {
                SortOrder = SortBy.ASCENDING,
                Source = TieBreakingRuleParticipantAttributeSource.Country
            };

            var directive = new RankingDirective();
            var comparer = new CompareByRankingDirective( cof, directive );

            directive.Rules.Clear();
            directive.Rules.Add( familyName );
            Assert.IsTrue( comparer.Compare( johnSmith, lucyJones ) > 0 );
            Assert.IsTrue( comparer.Compare( lucyJones, craigJones ) == 0 );
            Assert.IsTrue( comparer.Compare( johnSmith, craigJones ) > 0 );

            directive.Rules.Clear();
            directive.Rules.Add( givenName );
            Assert.IsTrue( comparer.Compare( johnSmith, lucyJones ) < 0 );
            Assert.IsTrue( comparer.Compare( lucyJones, craigJones ) > 0 );
            Assert.IsTrue( comparer.Compare( johnSmith, craigJones ) > 0 );

            directive.Rules.Clear();
            directive.Rules.Add( familyNameDesc );
            Assert.IsTrue( comparer.Compare( johnSmith, lucyJones ) < 0 );
            Assert.IsTrue( comparer.Compare( lucyJones, craigJones ) == 0 );
            Assert.IsTrue( comparer.Compare( johnSmith, craigJones ) < 0 );

            directive.Rules.Clear();
            directive.Rules.Add( compeNumber );
            Assert.IsTrue( comparer.Compare( johnSmith, lucyJones ) < 0 );
            Assert.IsTrue( comparer.Compare( lucyJones, craigJones ) < 0 );
            Assert.IsTrue( comparer.Compare( johnSmith, craigJones ) < 0 );

            directive.Rules.Clear();
            directive.Rules.Add( country );
            Assert.IsTrue( comparer.Compare( johnSmith, lucyJones ) < 0 );
            Assert.IsTrue( comparer.Compare( lucyJones, craigJones ) > 0 );
            Assert.IsTrue( comparer.Compare( johnSmith, craigJones ) > 0 );
        }

        /// <summary>
        /// Tests the parsing of the .Applies to Value
        /// </summary>
        [TestMethod]
        public void AppliesToTests() {

            var directive = new RankingDirective();

            directive.AppliesTo = "*";
            Assert.AreEqual( new Tuple<int, int>( 0, 20 ), directive.GetAppliesToStartAndCount( 20 ) );
            Assert.AreEqual( new Tuple<int, int>( 0, 0 ), directive.GetAppliesToStartAndCount( 0 ) );
            Assert.AreEqual( new Tuple<int, int>( 0, 4 ), directive.GetAppliesToStartAndCount( 4 ) );

            directive.AppliesTo = "1..8";
            Assert.AreEqual( new Tuple<int, int>( 0, 8 ), directive.GetAppliesToStartAndCount( 20 ) );
            Assert.AreEqual( new Tuple<int, int>( 0, 0 ), directive.GetAppliesToStartAndCount( 0 ) );
            Assert.AreEqual( new Tuple<int, int>( 0, 4 ), directive.GetAppliesToStartAndCount( 4 ) );

            //From rank 9 to the end of the list
            directive.AppliesTo = "9..";
            Assert.AreEqual( new Tuple<int, int>( 8, 12 ), directive.GetAppliesToStartAndCount( 20 ) );
            Assert.AreEqual( new Tuple<int, int>( 0, 0 ), directive.GetAppliesToStartAndCount( 0 ) );
            Assert.AreEqual( new Tuple<int, int>( 0, 0 ), directive.GetAppliesToStartAndCount( 4 ) );

            //Unparseable value
            directive.AppliesTo = "FU";
            Assert.AreEqual( new Tuple<int, int>( 0, 20 ), directive.GetAppliesToStartAndCount( 20 ) );
            Assert.AreEqual( new Tuple<int, int>( 0, 0 ), directive.GetAppliesToStartAndCount( 0 ) );
            Assert.AreEqual( new Tuple<int, int>( 0, 4 ), directive.GetAppliesToStartAndCount( 4 ) );
        }

        [TestMethod]
        public void ValueSeriesTests() {

            var vs1 = new ValueSeries( "1..10" );
            Assert.AreEqual( 1, vs1.StartValue );
            Assert.AreEqual( 10, vs1.EndValue );
            Assert.AreEqual( 1, vs1.Step );
            var intList = vs1.GetAsList();
            var strList = vs1.GetAsList( "P{}" );
            int index = 0;
            for (int i = 1; i <= 10; i++ ) {
                Assert.AreEqual( i, intList[index] );
                Assert.AreEqual( $"P{i}", strList[index] );
                index++;
            }

            var vs2 = new ValueSeries( "10..1" );
            Assert.AreEqual( 10, vs2.StartValue );
            Assert.AreEqual( 1, vs2.EndValue );
            Assert.AreEqual( 1, vs2.Step );
            intList = vs2.GetAsList();
            strList = vs2.GetAsList( "K{}" );
            index = 0;
            for (int i = 10; i > 0; i--) {
                Assert.AreEqual( i, intList[index] );
                Assert.AreEqual( $"K{i}", strList[index] );
                index++;
            }

            var vs3 = new ValueSeries( "1..10,2" );
            Assert.AreEqual( 1, vs3.StartValue );
            Assert.AreEqual( 10, vs3.EndValue );
            Assert.AreEqual( 2, vs3.Step );
            intList = vs3.GetAsList();
            strList = vs3.GetAsList( "S{}" );
            index = 0;
            for (int i = 1; i <= 10; i+=2) {
                Assert.AreEqual( i, intList[index] );
                Assert.AreEqual( $"S{i}", strList[index] );
                index++;
            }

            //The negative, part of the step value is ignored.
            var vs4 = new ValueSeries( "1..10,-2" );
            Assert.AreEqual( 1, vs4.StartValue );
            Assert.AreEqual( 10, vs4.EndValue );
            Assert.AreEqual( 2, vs4.Step );
            intList = vs4.GetAsList();
            strList = vs4.GetAsList( "PR {}" );
            index = 0;
            for (int i = 1; i <= 10; i += 2) {
                Assert.AreEqual( i, intList[index] );
                Assert.AreEqual( $"PR {i}", strList[index] );
                index++;
            }

            var vs5 = new ValueSeries( "10..1,2" );
            Assert.AreEqual( 10, vs5.StartValue );
            Assert.AreEqual( 1, vs5.EndValue );
            Assert.AreEqual( 2, vs5.Step );
            intList = vs5.GetAsList();
            strList = vs5.GetAsList( "ST {}" );
            index = 0;
            for (int i = 10; i > 0; i-=2) {
                Assert.AreEqual( i, intList[index] );
                Assert.AreEqual( $"ST {i}", strList[index] );
                index++;
            }
        }

        [TestMethod]
        public async Task EriksPlayground() {

            OrionMatchAPIClient matchClient = new OrionMatchAPIClient( APIStage.PRODUCTION );
            DefinitionAPIClient definitionClient = new DefinitionAPIClient();

            var resultListResponse = await matchClient.GetResultListPublicAsync( new MatchID( "1.1.2025043016073045.0" ), "Individual - Sporter" ); //
            var resultList = resultListResponse.ResultList;
            var rankingRuleDef = await resultList.GetRankingRuleDefinitionAsync();
            var courseOfFireDef = await resultList.GetCourseOfFireDefinitionAsync();

            //Clear all the existing Projected Scores out
            foreach( var part in resultList.Items) {
                foreach( var eventScore in part.EventScores.Values ) {
                    eventScore.Projected = null;
                }
            }

            ResultEngine resultEngine = new ResultEngine( resultList, rankingRuleDef );

            //ProjectScoresByScoreHistory ps = new ProjectScoresByScoreHistory(courseOfFire);
            //new ProjectScoresByAverageShotFired( courseOfFire );
            //ps.APIStage = APIStage.BETA;
            //ps.XAPIKey = Constants.X_API_KEY;

            var teamMemberComparer = new CompareByRankingDirective( courseOfFireDef, RankingDirective.GetDefault( resultList.EventName, resultList.ScoreConfigName ) );
            teamMemberComparer.ResultStatus = resultList.Status;
            teamMemberComparer.Projected = resultList.Projected;


            //ProjectorOfScores ps = new ProjectScoresByAverageShotFired( courseOfFire );
            ProjectorOfScores ps = ProjectorOfScoresFactory.Create( ProjectorOfScoresType.AVERAGE_SHOT_FIRED, courseOfFireDef, teamMemberComparer );
            ps.NumberOfTeamMembers = 3;

            await resultEngine.SortAsync( ps, true );

            foreach ( var re in resultList.Items ) {
                Debug.Write( $"{re.Rank} {re.Participant.DisplayName}  " );
                Debug.Write( $"{re.EventScores["Standing"].Projected.D}  {re.EventScores["Standing"].Score.D}  " );
                //Debug.Write( $"{re.EventScores["Qualification"].Score.I}  {re.EventScores["Final"].Score.D}  {re.EventScores["Individual"].Score.S}" );
                Debug.WriteLine("\n");
            }

        }
    }
}
