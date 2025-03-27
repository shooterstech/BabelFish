using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataActors.Specification.Definitions {
	public class IsRankingRulesValid : CompositeSpecification<RankingRule> {

		public override async Task<bool> IsSatisfiedByAsync( RankingRule candidate ) {

			var valid = true;
			Messages.Clear();

			//Common fields
			var hierarchicalName = new IsDefinitionHierarchicalNameValid();
			var commonName = new IsDefiniitonCommonNameValid();
			var description = new IsDefiniitonDescriptionValid();
			var subdiscipline = new IsDefiniitonSubdisciplineValid();
			var tags = new IsDefiniitonTagsValid();
			var comment = new IsCommentValid();
			var owner = new IsDefiniitonOwnerValid();
			var version = new IsDefiniitonVersionValid();

			if (!await hierarchicalName.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( hierarchicalName.Messages );
			} else {
				if (!await owner.IsSatisfiedByAsync( candidate )) {
					valid = false;
					Messages.AddRange( owner.Messages );
				}
			}

			if (!await version.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( version.Messages );
			}

			if (!await commonName.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( commonName.Messages );
			}

			if (!await description.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( description.Messages );
			}

			if (!await subdiscipline.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( subdiscipline.Messages );
			}

			if (!await tags.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( tags.Messages );
			}

			if (!await comment.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( comment.Messages );
			}

			//Attribute specific fields
			var firstRankingRule = new IsRankingRuleFirstRankingRuleValid();
			var source = new IsRankingRuleRulesSourceValid();


			if (!await firstRankingRule.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( firstRankingRule?.Messages );
			}

			if (!await source.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( source?.Messages );
			}

			return valid;
		}
	}

	public class IsRankingRuleFirstRankingRuleValid : CompositeSpecification<RankingRule> {

		public override async Task<bool> IsSatisfiedByAsync( RankingRule candidate ) {

			bool valid = true;
			Messages.Clear();

			//There must be at least one RankingRule
			if (candidate.RankingRules.Count == 0) {
				Messages.Add( "There must be at least one RankingRule" );
				return false;
			}

			//The first RankingRule must have "*" as the AppliesTo Value.
			if (candidate.RankingRules[0].AppliesTo != "*") {
				valid = false;
				Messages.Add( "The first ranking rule must have '*' as the value for AppliesTo." );
			}

			//Must have at least one Rule in the first RankingRule
			if (candidate.RankingRules[0].Rules.Count == 0) {
				valid = false;
				Messages.Add( "The first RankingRule must have at least one Rule." );
			}

			return valid;

		}
	}

	public class IsRankingRuleRulesSourceValid : CompositeSpecification<RankingRule> {

		public override async Task<bool> IsSatisfiedByAsync( RankingRule candidate ) {
			bool valid = true;

			var rankingRuleIndex = 0;
			foreach (var rankingRule in candidate.RankingRules) {

				var ruleIndex = 0;
				foreach (var rule in rankingRule.Rules) {

					if (rule is TieBreakingRuleAttribute) {
						var tbRule = rule as TieBreakingRuleAttribute;

						//Source must be a SetName to an Attribute
						var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( $"RankingRules[{rankingRuleIndex}].Rules[{ruleIndex}].Source",
							tbRule.Source,
							DefinitionType.ATTRIBUTE );

						if (!vm.Valid) {
							Messages.Add( vm.Message );
							valid = false;
						} else {
							//And the Attribute must be a simple attribute
							var sn = SetName.Parse( tbRule.Source );
							var attrDefinition = await DefinitionCache.GetAttributeDefinitionAsync( sn );

							if (!attrDefinition.SimpleAttribute) {
								valid = false;
								Messages.Add( $"RankingRules[{rankingRuleIndex}].Rules[{ruleIndex}].Source must be a SimpleAttribute." );
							}
						}

					} else if (rule is TieBreakingRuleCountOf) {
						var tbRule = rule as TieBreakingRuleCountOf;

						//Soucre must be an integer >= 0;
						if (tbRule.Source < 0) {
							valid = false;
							Messages.Add( $"RankingRules[{rankingRuleIndex}].Rules[{ruleIndex}].Source must be an integer equal to or greater than 0." );
						}

                        //EventName may not be empty
                        if (string.IsNullOrEmpty( tbRule.EventName )) {
							valid = false;
							Messages.Add( $"RankingRules[{rankingRuleIndex}].Rules[{ruleIndex}].EventName must not be an empty string." );
                        }

                        //If EventName has {} then Values needs to be a Values Series
                        //TODO Check that .Values is a ValueSeries
                        if (tbRule.EventName.Contains( "{}" ) && string.IsNullOrEmpty( tbRule.Values )) {
                            valid = false;
                            Messages.Add( $"RankingRules[{rankingRuleIndex}].Rules[{ruleIndex}].Values must not be an empty string when EventName contains the wildcard replacement characters '{{}}'." );

                        }

                    } else if (rule is TieBreakingRuleScore) {
                        var tbRule = rule as TieBreakingRuleScore;

						//EventName may not be empty
                        if (string.IsNullOrEmpty( tbRule.EventName )) {
                            valid = false;
                            Messages.Add( $"RankingRules[{rankingRuleIndex}].Rules[{ruleIndex}].EventName must not be an empty string." );
                        }

						//If EventName has {} then Values needs to be a Values Series
						//TODO Check that .Values is a ValueSeries
						if ( tbRule.EventName.Contains( "{}" ) && string.IsNullOrEmpty( tbRule.Values) ) {
                            valid = false;
                            Messages.Add( $"RankingRules[{rankingRuleIndex}].Rules[{ruleIndex}].Values must not be an empty string when EventName contains the wildcard replacement characters '{{}}'." );

                        }
                    }

					//Nothing to check for in TieBreakingRuleParticipantAttribute

					ruleIndex++;
				}

				rankingRuleIndex++;
			}

			return valid;
		}
	}
}
