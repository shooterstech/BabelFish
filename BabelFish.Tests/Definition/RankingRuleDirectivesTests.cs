using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataActors.OrionMatch;

namespace Scopos.BabelFish.Tests.Definition {

    [TestClass]
    public class RankingRuleDirectivesTests {

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

            var integerDescending = new TieBreakingRule() {
                SortOrder = SortBy.DESCENDING,
                EventName = "Qualification",
                Method = TieBreakingRuleMethod.SCORE,
                Source = "I"
            };

            var integerAscending = new TieBreakingRule() {
                SortOrder = SortBy.ASCENDING,
                EventName = "Qualification",
                Method = TieBreakingRuleMethod.SCORE,
                Source = "I"
            };

            var decimalDescending = new TieBreakingRule() {
                SortOrder = SortBy.DESCENDING,
                EventName = "Qualification",
                Method = TieBreakingRuleMethod.SCORE,
                Source = "D"
            };

            var decimalAscending = new TieBreakingRule() {
                SortOrder = SortBy.ASCENDING,
                EventName = "Qualification",
                Method = TieBreakingRuleMethod.SCORE,
                Source = "D"
            };

            var xDescending = new TieBreakingRule() {
                SortOrder = SortBy.DESCENDING,
                EventName = "Qualification",
                Method = TieBreakingRuleMethod.SCORE,
                Source = "X"
            };

            var xAscending = new TieBreakingRule() {
                SortOrder = SortBy.ASCENDING,
                EventName = "Qualification",
                Method = TieBreakingRuleMethod.SCORE,
                Source = "X"
            };

            var directive = new RankingDirective();
            var comparer = new CompareByRankingDirective( cof, directive );

            //Integer Descending, X's integer score is < Y's, Z's score is equal to Y's score
            directive.Rules.Clear();
            directive.Rules.Add( integerDescending );
            Assert.IsTrue( comparer.Compare( x, y ) < 0 );
            Assert.IsTrue( comparer.Compare( y, x ) > 0 );
            Assert.IsTrue( comparer.Compare( z, y ) == 0 );

            //Integer Ascending, X's integer score is < Y's
            directive.Rules.Clear();
            directive.Rules.Add( integerAscending );
            Assert.IsTrue( comparer.Compare( x, y ) > 0 );
            Assert.IsTrue( comparer.Compare( y, x ) < 0 );
            Assert.IsTrue( comparer.Compare( z, y ) == 0 );

            //Decimal Descending, X's decimal score is < Y's
            directive.Rules.Clear();
            directive.Rules.Add( decimalDescending );
            Assert.IsTrue( comparer.Compare( x, y ) < 0 );
            Assert.IsTrue( comparer.Compare( y, x ) > 0 );
            Assert.IsTrue( comparer.Compare( z, y ) == 0 );

            //Decimal Ascending, X's decimal score is < Y's
            directive.Rules.Clear();
            directive.Rules.Add( decimalAscending );
            Assert.IsTrue( comparer.Compare( x, y ) > 0 );
            Assert.IsTrue( comparer.Compare( y, x ) < 0 );
            Assert.IsTrue( comparer.Compare( z, y ) == 0 );

            //Inner Tens Descending, X's inner ten count is > Y's
            directive.Rules.Clear();
            directive.Rules.Add( xDescending );
            Assert.IsTrue( comparer.Compare( x, y ) > 0 );
            Assert.IsTrue( comparer.Compare( y, x ) < 0 );
            Assert.IsTrue( comparer.Compare( z, y ) == 0 );

            //Inner Tens Ascending, X's inner ten count is > Y's
            directive.Rules.Clear();
            directive.Rules.Add( xAscending );
            Assert.IsTrue( comparer.Compare( x, y ) < 0 );
            Assert.IsTrue( comparer.Compare( y, x ) > 0 );
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

            var familyName = new TieBreakingRule() {
                SortOrder = SortBy.ASCENDING,
                Method = TieBreakingRuleMethod.PARTICIPANT_ATTRIBUTE,
                Source = "FamilyName"
            };

            var givenName = new TieBreakingRule() {
                SortOrder = SortBy.ASCENDING,
                Method = TieBreakingRuleMethod.PARTICIPANT_ATTRIBUTE,
                Source = "GivenName"
            };

            var familyNameDesc = new TieBreakingRule() {
                SortOrder = SortBy.DESCENDING,
                Method = TieBreakingRuleMethod.PARTICIPANT_ATTRIBUTE,
                Source = "FamilyName"
            };

            var compeNumber = new TieBreakingRule() {
                SortOrder = SortBy.ASCENDING,
                Method = TieBreakingRuleMethod.PARTICIPANT_ATTRIBUTE,
                Source = "CompetitorNumber"
            };

            var country = new TieBreakingRule() {
                SortOrder = SortBy.ASCENDING,
                Method = TieBreakingRuleMethod.PARTICIPANT_ATTRIBUTE,
                Source = "Country"
            };

            var directive = new RankingDirective();
            var comparer = new CompareByRankingDirective( cof, directive );

            //Integer Descending, X's integer score is < Y's, Z's score is equal to Y's score
            directive.Rules.Clear();
            directive.Rules.Add( familyName );
            Assert.IsTrue( comparer.Compare( johnSmith, lucyJones ) < 0 );
            Assert.IsTrue( comparer.Compare( lucyJones, craigJones ) == 0 );
            Assert.IsTrue( comparer.Compare( johnSmith, craigJones ) < 0 );

            directive.Rules.Clear();
            directive.Rules.Add( givenName );
            Assert.IsTrue( comparer.Compare( johnSmith, lucyJones ) > 0 );
            Assert.IsTrue( comparer.Compare( lucyJones, craigJones ) < 0 );
            Assert.IsTrue( comparer.Compare( johnSmith, craigJones ) < 0 );

            directive.Rules.Clear();
            directive.Rules.Add( familyNameDesc );
            Assert.IsTrue( comparer.Compare( johnSmith, lucyJones ) > 0 );
            Assert.IsTrue( comparer.Compare( lucyJones, craigJones ) == 0 );
            Assert.IsTrue( comparer.Compare( johnSmith, craigJones ) > 0 );

            directive.Rules.Clear();
            directive.Rules.Add( compeNumber );
            Assert.IsTrue( comparer.Compare( johnSmith, lucyJones ) > 0 );
            Assert.IsTrue( comparer.Compare( lucyJones, craigJones ) > 0 );
            Assert.IsTrue( comparer.Compare( johnSmith, craigJones ) > 0 );

            directive.Rules.Clear();
            directive.Rules.Add( country );
            Assert.IsTrue( comparer.Compare( johnSmith, lucyJones ) > 0 );
            Assert.IsTrue( comparer.Compare( lucyJones, craigJones ) < 0 );
            Assert.IsTrue( comparer.Compare( johnSmith, craigJones ) < 0 );
        }
    }
}
