using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class EventDerived : Event {

        public EventDerived() {
            Derivation = EventDerivationType.DERIVED;
        }

        /// <summary>
        /// String formatted as a Value Series.
        /// <para><see href="https://support.scopos.tech/index.html?string-formatting-value-series.html">Value Series documentation.</see></para>
        /// </summary>
        [JsonPropertyOrder( 12 )]
        [DefaultValue( "" )]
        public string ChildValues { get; set; } = string.Empty;

        /// <summary>
        /// Must include the place holder {}. For example "PR {}".
        /// </summary>
        [JsonPropertyOrder( 13 )]
        [DefaultValue( "" )]
        public string ChildEventName { get; set; } = string.Empty ;

        /// <inheritdoc />
        public override List<string> Children {
            get {

                if (string.IsNullOrEmpty( ChildEventName )
                    || !ChildEventName.Contains( "{}" ))
                    throw new ScoposException( "Can not calculate the list of Event Children. The value for ChildEventName is incorrectly formatted. It is empty, null, or does not contain a placeholder." );

                _children = new List<string>();
                ValueSeries vs = new ValueSeries( this.ChildValues );
                foreach (var eventName in vs.GetAsList( this.ChildEventName )) {
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
