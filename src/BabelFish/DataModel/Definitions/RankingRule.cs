using System.ComponentModel;
using System.Runtime.Serialization;
using Scopos.BabelFish.DataActors.Specification.Definitions;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A RANKING RULE precisely defines how a list of participants competing in an event should be sorted to create a result list.
    /// </summary>
    /// <remarks>
    /// RANKING RULE is one of the top level Reconfigurable Rulebook definitions.</remarks>
    [Serializable]
    public class RankingRule : Definition {

        public RankingRule() : base() {
            Type = DefinitionType.RANKINGRULES;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod( StreamingContext context ) {
            base.OnDeserializedMethod( context );
        }

        /// <summary>
        /// An ordered list of RankingDirectives to follow to sort the participants in a Result List.
        /// <para>When the Result List status is FUTURE or INTERMEDIATE only the first RankingDirective
        /// is used.When the status if UNOFFICIAL or OFFICIAL all RankingDirectives are used to sort a result list.</para>
        /// <para>Typically there will only be one RankingDirective in the list.</para>
        /// <para>This attribute is required.There must be one or more RankingDirective listed.</para>
        /// </summary>
        [G_STJ_SER.JsonPropertyOrder( 1 )]
        [G_NS.JsonProperty( Order = 20 )]
        [DefaultValue( null )]
        public List<RankingDirective> RankingRules { get; set; } = new List<RankingDirective>();

        /// <summary>
        /// Generates a default ranking rule definition based on the passed in Event Name and ScoreConfigName
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="scoreConfigName"></param>
        /// <returns></returns>
        public static RankingRule GetDefault( string eventName, string scoreConfigName ) {

            var rankingRule = new RankingRule();
            rankingRule.RankingRules.Add( RankingDirective.GetDefault( eventName, scoreConfigName ) );

            return rankingRule;
        }

        /// <inheritdoc />
        public override async Task<bool> GetMeetsSpecificationAsync() {
            var validation = new IsRankingRulesValid();

            var meetsSpecification = await validation.IsSatisfiedByAsync( this );
            SpecificationMessages = validation.Messages;

            return meetsSpecification;
        }

        /// <inheritdoc />
        public override bool SetDefaultValues() {
            base.SetDefaultValues();

            RankingDirective rd = new RankingDirective();
            rd.AppliesTo = "*";

            var aggregateRule = new TieBreakingRuleScore() {
                EventName = "Aggregate",
                SortOrder = SortBy.DESCENDING,
                Source = TieBreakingRuleScoreSource.I,
                Comment = "Auto generated default Tie Breaking Rule"
            };
            rd.Rules.Add( aggregateRule );

            var familyName = new TieBreakingRuleParticipantAttribute() {
                SortOrder = SortBy.ASCENDING,
                Source = TieBreakingRuleParticipantAttributeSource.FamilyName
            };
            var givenName = new TieBreakingRuleParticipantAttribute() {
                SortOrder = SortBy.ASCENDING,
                Source = TieBreakingRuleParticipantAttributeSource.GivenName
            };
            var middleName = new TieBreakingRuleParticipantAttribute() {
                SortOrder = SortBy.ASCENDING,
                Source = TieBreakingRuleParticipantAttributeSource.MiddleName
            };
            var compNumber = new TieBreakingRuleParticipantAttribute() {
                SortOrder = SortBy.ASCENDING,
                Source = TieBreakingRuleParticipantAttributeSource.CompetitorNumber
            };
            rd.ListOnly.Add( familyName );
            rd.ListOnly.Add( givenName );
            rd.ListOnly.Add( middleName );
            rd.ListOnly.Add( compNumber );

            this.RankingRules.Add( rd );

            return true;
        }
    }
}
