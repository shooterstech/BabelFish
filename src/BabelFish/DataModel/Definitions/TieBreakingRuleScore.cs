using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Defines how to compare two IEventScores using the Score object from an Event.
    /// </summary>
    public class TieBreakingRuleScore : TieBreakingRuleBase, IEquatable<TieBreakingRuleScore>, IEqualityComparer<TieBreakingRuleScore> {

        public TieBreakingRuleScore() {
            this.Method = TieBreakingRuleMethod.SCORE;
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
        /// The Score dictionary property to use to compare.
        /// </summary>
        /// <remarks>
        /// Optional values are:
        /// <list type="bullet">
        /// <item>I => Use the integer score.</item>
        /// <item>D => Use the decimal score.</item>
        /// <item>S => Use the special sum rulebook score.</item>
        /// <item>X => Use the inner ten score.</item>
        /// <item>IX => Use the integer socre, and if still tied then use the inner ten score.</item>
        /// <item>J => Use the special use case J score.</item>
        /// <item>K => Use the special use case K score.</item>
        /// <item>L => Use the special use case L score.</item>
        /// </list>
        /// </remarks>
        [G_NS.JsonProperty( Order = 4, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( TieBreakingRuleScoreSource.IX )]
        public TieBreakingRuleScoreSource Source { get; set; } = TieBreakingRuleScoreSource.IX;

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
        public bool Equals( TieBreakingRuleScore x, TieBreakingRuleScore y ) {
            return x.Source == y.Source
                && x.SortOrder == y.SortOrder
                && x.EventName == y.EventName
                && x.Values == y.Values;
        }

        /// <inheritdoc/>
        public int GetHashCode( TieBreakingRuleScore obj ) {
            return $"{obj.Method} {obj.SortOrder} {obj.Source} {obj.EventName} {obj.Values}".GetHashCode();
        }

        /// <inheritdoc/>
        public bool Equals( TieBreakingRuleScore other ) {
            return Equals( this, other );
        }

        /// <inheritdoc/>
        public override bool Equals( object obj ) {
            if (obj == null || obj is not TieBreakingRuleScore) 
                return false;

            return Equals( (TieBreakingRuleScore) obj );
        }

        /// <inheritdoc/>
        public override int GetHashCode() {
            return GetHashCode( this );
        }

        #endregion
    }
}
