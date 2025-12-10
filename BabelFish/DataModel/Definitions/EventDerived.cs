using System;
using System.Collections.Generic;
using System.ComponentModel;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.DataModel.Definitions {

	/// <summary>
	/// An EventDerived is a concrete child of abstract class Event.
	/// It represents a single EventExplicit, but one where the Children are encoded. 
    /// The Children are based on the values of ChildEventName and ChildValues.
	/// </summary>
	/// <remarks>
	/// <![CDATA[
	/// {
	/// "EventName": "Prone",
	/// "EventType": "STAGE",
	/// "Derivation": "DERIVED",
	/// "ChildValues": "1..50",
	/// "ChildEventName": "PR {}",
	/// "Calculation": "SUM",
	/// "ScoreFormat": "Events"
	/// }
	/// ]]>
	/// </remarks>
	public class EventDerived : Event {

        public EventDerived() {
            Derivation = EventDerivationType.DERIVED;
        }


        /// <summary>
        /// Numberic values to use to interpolation .ChildEventName
        /// </summary>
        /// <remarks>Default value is "1"</remarks>
		[G_STJ_SER.JsonPropertyOrder( 5 )]
        [G_NS.JsonProperty( Order = 5 )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.ValueSeriesConverter ) )]
        [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.ValueSeriesConverter ) )]
        public ValueSeries ChildValues { get; set; } = new ValueSeries( "1" );

        /// <summary>
        /// Newtonsoft.json helper method to determine if .Values should be serialized.
        /// If .ChildEventName contains the interpolation variable "{}" then .ChildValues will be serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeChildValues() {
            return ChildEventName?.Contains( "{}" ) ?? false;
        }

        /// <summary>
        /// Must include the place holder {}. For example "PR {}".
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        [DefaultValue( "" )]
        public string ChildEventName { get; set; } = string.Empty ;

        /// <inheritdoc />
        [G_STJ_SER.JsonIgnore]
        [G_NS.JsonIgnore]
        public override List<string> Children {
            get {

                if (string.IsNullOrEmpty( ChildEventName )
                    || !ChildEventName.Contains( "{}" ))
                    throw new ScoposException( "Can not calculate the list of Event Children. The value for ChildEventName is incorrectly formatted. It is empty, null, or does not contain a placeholder." );

                _children = new List<string>();
                foreach (var eventName in ChildValues.GetAsList( this.ChildEventName )) {
                    _children.Add( eventName );
                }

                return _children;
            }

            set {
                throw new ScoposException( "Can not set the Children (Events) of a EventDerived, as they are derived and not set explicity." );
            }
        }
    }
}
