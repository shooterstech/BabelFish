using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        public Scopos.BabelFish.DataModel.Definitions.ShowInSegment.CompetitionType Competition { get; set; } = Scopos.BabelFish.DataModel.Definitions.ShowInSegment.CompetitionType.COMPETITION;

        /// <summary>
        /// If Competition == SIGHTER, then show all sighters that have a StageLabel equal to, well, this property.
        /// </summary>
        public string StageLabel { get; set; } = string.Empty;

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
        public Scopos.BabelFish.DataModel.Definitions.Event.EventtType EventType { get; set; } = Scopos.BabelFish.DataModel.Definitions.Event.EventtType.NONE;

        public override string ToString()
        {
            return ShowName;
        }
    }
}