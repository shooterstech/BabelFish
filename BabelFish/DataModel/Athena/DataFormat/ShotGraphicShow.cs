using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.Athena.DataFormat
{


    public class ShotGraphicShow
    {

        /// <summary>
        /// Name given to this ShotGraphicShow object. Should be semi-human readable.
        /// </summary>
        public string ShowName { get; set; } = string.Empty;

        /// <summary>
        /// Show Competition shots or sighter shots.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CompetitionType Competition { get; set; } = CompetitionType.COMPETITION;

        /// <summary>
        /// If Competition == SIGHTER, then show all sighters that have a StageLabel equal to, well, this property.
        /// </summary>
        public List<string> StageLabel { get; set; } = new List<string> ();

        /// <summary>
        /// ALL => Display all competition shots
        /// EVENT => Display only the shots from the event with EventName (listed in the EventName property)
        /// CURRENT => Display the shots from the current SERIES, STAGE, STIRNG, etc. NOTE, only relavent during a live display of shots.
        /// PAST(n) => Display the last n number of shots
        /// </summary>
        public string ShotPresentation { get; set; } = "ALL";

        /// <summary>
        /// When ShotPresentation == "EVENT", thsi property holds the name of the Event to show shots from.
        /// </summary>
        public string EventName { get; set; } = string.Empty;

        /// <summary>
        /// When ShotPresentation == "CURRENT", this property holds the EventTyhpe to show. The current EventType is listed under LiveDispaly.CurrentEvents.
        /// NOTE, only relavent during a live display of shots.
        /// </summary>
        public Definitions.EventtType EventType { get; set; } = Definitions.EventtType.NONE;

        public override string ToString()
        {
            return ShowName;
        }
    }
}