using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    [Serializable]
    public class RankingDirective : IReconfigurableRulebookObject, ICopy<RankingDirective>
    {

        private enum STATE { NOTHING_IS_SET, START_IS_SET, END_IS_SET };

        public RankingDirective() {
        }

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {

            if (Rules == null)
                this.Rules = new List<TieBreakingRule>();

            if (ListOnly == null)
                this.ListOnly = new List<TieBreakingRule>();
        }

        /// <summary>
        /// A string indicating who to apply this RankingRules to. The first, and only the first, RankingRule must have a value “*”.
        /// The value must be one of the following:
        /// “*” to indicate to apply the rule to all participants in the event.
        /// “n..m” where n and m are integer values, representing the rank of the participants after the last RankingRule was applied. 
        /// For example, “1..8” means to apply this RankingRule to participants currently in first through eighth place.
        /// Not required, assumed to be '*' if missing.
        /// </summary>
        [DefaultValue( "*" )]
        public string AppliesTo { get; set; } = "*";

        /// <summary>
        /// Parses the AppliesTo value and returns the start index (item 1)
        /// and the count (item 2) to use to slice the array. 
        /// </summary>
        /// <param name="sizeOfList"></param>
        /// <returns></returns>
        public Tuple<int, int> GetAppliesToStartAndCount( int sizeOfList ) {

            if (string.IsNullOrEmpty( AppliesTo ) || AppliesTo == "*") {
                return new Tuple<int, int>( 0, sizeOfList );
            }

            try {
                var split = AppliesTo.Split( new char[] { '.', ',', ' ' } );
                int start = 0, end = sizeOfList;
                STATE state = STATE.NOTHING_IS_SET;

                foreach ( string foo in split ) {
                    //the -1 is to translate the rank, which has an start index of 1, to CS index that starts at 0
                    if (state == STATE.NOTHING_IS_SET && int.TryParse( foo, out start )) {
                        start--;
                        state = STATE.START_IS_SET;
                    } else if (state == STATE.START_IS_SET && int.TryParse( foo, out end )) {
                        end--;
                        state = STATE.END_IS_SET;
                    } else if (state == STATE.END_IS_SET) {
                        break;
                    }
                }

                //Provide default values if we couldn't read two different start and end values
                if (state == STATE.NOTHING_IS_SET) {
                    start = 0;
                    end = sizeOfList - 1;
                } else if (state == STATE.START_IS_SET) {
                    end = sizeOfList - 1;
                }

                if (start < 0 || start >= sizeOfList)
                    start = sizeOfList;

                if (end >= sizeOfList) 
                    end = sizeOfList-1;

                int count = end - start + 1;

                if (count == 0)
                    start = 0;

                return new Tuple<int, int>( start, count );
            } catch (Exception ex) {

                return new Tuple<int, int>( 0, sizeOfList );
            }

            /*
             * 
            if (string.IsNullOrEmpty( AppliesTo ) || AppliesTo == "*") {
                return new Tuple<int, int>( 0, sizeOfList );
            }

            ValueSeries vs = new ValueSeries( AppliesTo );
            if (vs.StartValue < vs.EndValue ) {
                return new Tuple<int, int>( vs.StartValue-1, vs.EndValue-vs.StartValue );
            } else {
                return new Tuple<int, int>( 0, sizeOfList );
            }
            */
        }

        /// <summary>
        /// An ordered list of TieBreakingRules to follow to sort two participants in an event. 
        /// 
        /// The result engine must use the list of Rule in order until the tie is broken. Once the tie is broken the remaining 
        /// Rules are ignored. If the tie can not be broken given the set of Rules the two competitors are given the same rank. 
        /// 
        /// This attribute is required and must have one or more elements in the list.
        /// </summary>
        public List<TieBreakingRule> Rules { get; set; } = new List<TieBreakingRule>();

        /// <summary>
        /// In the event that tie between two participants can not be broken, the TieBreakingRules in ListOnly are used to sort participants for display purposes only.
        /// 
        /// The result engine must use the list of TieBreakingRules in the order listed. Once the tie is broke the remaining rules may be ignored. 
        /// These rules do not affect a participants rank, only the order they are listed.
        /// 
        /// This attribute is not required.
        /// </summary>
        public List<TieBreakingRule> ListOnly { get; set; } = new List<TieBreakingRule>();

        /// <summary>
        /// Generates a default RankingDirective based on the passed in top level event name.
        /// If tied, participants are sorted using their DisplayName
        /// </summary>
        /// <param name="topLevelEventName"></param>
        /// <returns></returns>
        public static RankingDirective GetDefault( string topLevelEventName, string scoreConfigName = "Decimal" ) {
            var directive = new RankingDirective();

            directive.AppliesTo = "*";

            //Try and predict a set of tie breaking rules to use, based on the passed in score config name. 
            //Note we are largely assuming the standard set of score config names from v1.0:orion:Standard Score Formats
            switch (scoreConfigName.ToUpper()) {
                case "DECIMAL":
                case "DEC":
                case "D":
                default:
                    directive.Rules.Add( new TieBreakingRule() {
                        EventName = topLevelEventName,
                        SortOrder = Helpers.SortBy.DESCENDING,
                        Method = TieBreakingRuleMethod.SCORE,
                        Source = "D"
                    } );
                    break;

                case "INTEGER":
                case "INT":
                case "I":
                case "CONVENTIONAL":
                case "CONV":
                case "C":
                    directive.Rules.Add( new TieBreakingRule() {
                        EventName = topLevelEventName,
                        SortOrder = Helpers.SortBy.DESCENDING,
                        Method = TieBreakingRuleMethod.SCORE,
                        Source = "I"
                    } );

                    directive.Rules.Add( new TieBreakingRule() {
                        EventName = topLevelEventName,
                        SortOrder = Helpers.SortBy.DESCENDING,
                        Method = TieBreakingRuleMethod.SCORE,
                        Source = "X"
                    } );
                    break;
            }

            directive.ListOnly.Add( new TieBreakingRule() {
                Method = TieBreakingRuleMethod.PARTICIPANT_ATTRIBUTE,
                Source = "DisplayName",
                SortOrder = Helpers.SortBy.ASCENDING
            } );

            return directive;
        }

        public RankingDirective Copy()
        {
            RankingDirective rd = new RankingDirective();
            rd.AppliesTo = this.AppliesTo;
            if (this.Rules != null)
            {
                foreach (var ori in this.Rules)
                {
                    rd.Rules.Add(ori.Copy());
                }
            }
            if (this.ListOnly != null)
            {
                foreach (var ori in this.ListOnly)
                {
                    rd.ListOnly.Add(ori.Copy());
                }
            }
            return rd;
        }

        /// <inheritdoc/>
        [JsonProperty(Order = 99, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
