using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// Within a ResultListField the Method property describes the type of data, and the Source property describes where the data is coming from.
    /// </summary>
    public class FieldSource : IReconfigurableRulebookObject {

        /// <summary>
        /// When the ResultField.Method == Score, Name is the name 
        /// of the Event to pull the participant's score from.
        /// </summary>
        public string Name { get; set; } = string.Empty;


        /// <summary>
        /// When the ResultField.Method == Score, ScoreFormat is the name 
        /// of the Score Format to use. The list of optional Score Format names 
        /// is defined in the Score Format Definition (e.g. v1.0:orion:Standard Score Formats)
        /// </summary>
        public string ScoreFormat { get; set; } = string.Empty;

        /// <summary>
        /// When the ResultField.Method == Gap, Value is the rank of the participant
        /// to comare agaisnt. -1 means the participant directly ahead, VAlues > 0
        /// mean that specific spot (1 being the leader, or 8 being cut to make Final).
        /// </summary>        
        [DefaultValue( 0 )]
        public int Value { get; set; } = 0;

        /// <summary>
        /// If the length of the returned string is longer than .TruncateAt, then the string 
        /// is truncated at this length and has a set of ellipsis ("...") added to the end.
        /// A value of < 3, means never to truncate.
        /// </summary>
        [DefaultValue( 0 )]
        public int TruncateAt { get; set; } = 0;

        /// <inheritdoc/>
        [JsonPropertyOrder( 99 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
    }
}
