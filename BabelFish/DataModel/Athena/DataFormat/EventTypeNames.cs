using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.BabelFish.DataModel.Athena.DataFormat
{
    /// <summary>
    /// Object that holds the EventNames of a specific shot, for each of the EventTypes.
    /// Since EventTypes are not required, values could be an empty string.
    /// Property names are purposefully in all caps, to match the EventType enum values.
    /// </summary>
    public class EventTypeNames
    {


        public string EVENT { get; set; } = string.Empty;
        public string STAGE { get; set; } = string.Empty;
        public string SERIES { get; set; } = string.Empty;
        public string STRING { get; set; } = string.Empty;
        public string SINGULAR { get; set; } = string.Empty;
    }
}