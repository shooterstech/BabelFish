using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

	/// <summary> 
	/// An EventExplicit is a concrete child of abstract class Event.
	/// The Children of the Event are listed explicitly. 
	/// </summary>
	/// <remarks>
	/// <![CDATA[
	/// {
	/// "EventName": "Prone",
	/// "EventType": "STAGE",
	/// "Derivation": "EXPLICIT",
	/// "Calculation": "SUM",
	/// "ScoreFormat": "Events",
	/// "Children" : [
	///   "PR 1",
	///   "PR 2",
	///   "PR 3"
	///   "PR 4"
	/// }
	/// ]]>
	/// </remarks>
	public class EventExplicit : Event {

        public EventExplicit() {
            Derivation = EventDerivationType.EXPLICIT;
        }

        /// <inheritdoc />
        [G_STJ_SER.JsonPropertyOrder( 4 )]
        [G_NS.JsonProperty( Order = 4 )]
        public override List<string> Children { 
            get {
                return _children;
            }
            set { 
                _children = value; 
            }
        }
    }
}
