using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Defines how to compare two IEventScores using the number of shots, from an Event, with a specific integer score.
    /// </summary>
    public class TieBreakingRuleCountOf : TieBreakingRuleBase, IEquatable<TieBreakingRuleCountOf>, IEqualityComparer<TieBreakingRuleCountOf> {

        /// <summary>
        /// Public constructor
        /// </summary>
        public TieBreakingRuleCountOf() {
            this.Method = TieBreakingRuleMethod.COUNT_OF;
        }

        /// <summary>
        /// The EventName to apply this rule to that is defined by the Course of Fire and found in the participant’s ResultCOF. 
        /// 
        /// The result engine must use this rule if the EventName is found in the participant’s ResultCOF. If the EventName is not found this TieBreakingRule is skipped.
        /// 
        /// May contain a place holder "{}". If used, ValueSeries must be included to compile the list of EventNames to check.
        /// </summary>
        [G_NS.JsonProperty( Order = 2 )]
        public string EventName { get; set; } = string.Empty;

        /// <summary>
        /// When EventName contains a place holder "{}", the ValueSeries are used to compile the actual list of EventNames to check against.
        /// 
        /// Required when EventName has a placeholder, ignored otherwise.
        /// </summary>
        [G_NS.JsonProperty( Order = 3 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.ValueSeriesConverter ) )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ValueSeriesConverter ) )]
        public ValueSeries Values { get; set; } = new ValueSeries( "1" );

        /// <summary>
        /// Newtonsoft.json helper method to determine if .Values should be serialized.
        /// If .EventName contains the interpolation variable "{}" then .Values will be serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeValues() {
            return EventName?.Contains( "{}" ) ?? false;
        }

        /// <summary>
        /// The integer score value to use to count.
        /// </summary>
        /// <remarks>Example 10, 9, 8, etc</remarks>
        [G_NS.JsonProperty( Order = 4, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( 0 )]
        public int Source { get; set; } = 10;

        /// <inheritdoc/>
        public override string ToString() {
            return $"{Method} {SortOrder} {Source}";
        }

        /// <inheritdoc/>
        public override List<TieBreakingRuleBase> GetCompiledTieBreakingRules() {
            if (string.IsNullOrEmpty( EventName ) || !EventName.Contains( "{}" )) {
                return new List<TieBreakingRuleBase>() { this };
            } else {
                List<TieBreakingRuleBase> list = new List<TieBreakingRuleBase>();
                foreach (var eventName in Values.GetAsList( this.EventName )) {
                    var newTieBreakingRule = this.Clone();
                    newTieBreakingRule.EventName = eventName;
                    newTieBreakingRule.Values = new ValueSeries( "1" );
                    list.Add( newTieBreakingRule );
                }
                return list;
            }
        }

        #region Equal Operators

        /// <inheritdoc/>
        public bool Equals( TieBreakingRuleCountOf x, TieBreakingRuleCountOf y ) {
            return x.Source == y.Source
                && x.SortOrder == y.SortOrder
                && x.EventName == y.EventName
                && x.Values == y.Values;
        }

        /// <inheritdoc/>
        public int GetHashCode( TieBreakingRuleCountOf obj ) {
            return $"{obj.Method} {obj.SortOrder} {obj.Source} {obj.EventName} {obj.Values}".GetHashCode();
        }

        /// <inheritdoc/>
        public bool Equals( TieBreakingRuleCountOf other ) {
            return Equals( this, other );
        }

        /// <inheritdoc/>
        public override bool Equals( object obj ) {
            if (obj == null || obj is not TieBreakingRuleCountOf)
                return false;

            return Equals( (TieBreakingRuleCountOf)obj );
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return GetHashCode( this );
        }

        #endregion
    }
}
