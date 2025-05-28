using System;
using System.Collections.Generic;
using System.ComponentModel;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.DataModel.Definitions {

	/// <summary>
	/// An EventExpand is a concrete child of abstract class Event.
	/// It represents a series of EventExplicits (but defined more compactly). The values of EventName and Children (and the expanded list of 
	/// Event Explicits), are encodeed within this EventExpand. The Event Name of the expanded list, is based on the value 
	/// of EventName and Values. The Children are based on the values of ChildEventName and ChildStringSize.
	/// </summary>
	/// <remarks>
	/// <![CDATA[
	/// {
	/// "EventName": "PR {}",
	/// "EventType": "STRING",
	/// "Derivation": "EXPAND",
	/// "Values": "1..50",
	/// "ChildEventName": "P{}",
    /// "ChildStringSize" : 10,
	/// "Calculation": "SUM",
	/// "ScoreFormat": "Events"
	/// }
	/// ]]>
	/// </remarks>
	public class EventExpand : Event {

        public EventExpand() {
            Derivation = EventDerivationType.EXPAND;
        }

        /// <summary>
        /// String formatted as a Value Series.
        /// <para><see href="https://support.scopos.tech/index.html?string-formatting-value-series.html">Value Series documentation.</see></para>
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        [DefaultValue( "" )]
        public string Values { get; set; } = string.Empty;

        [G_STJ_SER.JsonPropertyOrder( 7 )]
        [G_NS.JsonProperty( Order = 7, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
        [DefaultValue( 10 )]
        public int ChildStringSize { get; set; } = 10;

        /// <summary>
        /// Must include the place holder {}. For example "PR {}".
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 6 )]
        [G_NS.JsonProperty( Order = 6 )]
        [DefaultValue( "" )]
        public string ChildEventName { get; set; } = string.Empty;

        /// <inheritdoc />
        public override List<EventExplicit> GetCompiledEvents() {
            if (string.IsNullOrEmpty( Values )
                || !this.EventName.Contains( "{}" ))
                throw new ScoposException( $"Can not compile the list of expanded Events. The value for EventName is incorrectly formatted, '{this.EventName}'. It is empty, null, or does not contain a placeholder." );

            List<EventExplicit> events = new List<EventExplicit>();

            int childIndex = 1;
            ValueSeries vs = new ValueSeries( this.Values );
            foreach (var eventName in vs.GetAsList( this.EventName )) {
                var eventExplicit = new EventExplicit();
                eventExplicit.EventName = eventName;
                eventExplicit.Calculation = this.Calculation;
                eventExplicit.CalculationVariables = this.CalculationVariables;
                eventExplicit.ScoreFormat = this.ScoreFormat;
                eventExplicit.EventType = this.EventType;
                eventExplicit.ResultListFormatDef = this.ResultListFormatDef;
                if ( this.StageStyleMapping != null )
                    eventExplicit.StageStyleMapping = this.StageStyleMapping.Clone();
                if (this.EventStyleMapping != null)
                    eventExplicit.EventStyleMapping = this.EventStyleMapping.Clone();
                if (this.RankingRuleMapping != null )
                    eventExplicit.RankingRuleMapping = this.RankingRuleMapping.Clone();
                eventExplicit.ExternalToEventTree = this.ExternalToEventTree;
                eventExplicit.Comment = $"Compiled EventExplicit based on the EventExpand named '{this.EventName}'.";

                for (int i = childIndex; i < childIndex + this.ChildStringSize; i++) {
                    var childName = this.ChildEventName.Replace( "{}", i.ToString() );
                    eventExplicit.Children.Add( childName );
                }
                childIndex += ChildStringSize;

                events.Add( eventExplicit );
            }

            return events;
        }


		/// <inheritdoc />
		/// <remarks>
		/// Overriding the property here, specifically to apply the JsonIgnore attribute. As the json definition
        /// has no use for Children, there is no need to serialize or deserize it. 
		/// </remarks>
		[G_STJ_SER.JsonIgnore]
		[G_NS.JsonIgnore]
		public override List<string> Children {
            get { return _children; } 
            set { ; } 
        }
    }
}
