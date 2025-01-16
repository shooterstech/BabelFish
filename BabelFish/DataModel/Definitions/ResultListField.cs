using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {
    /// <summary>
    /// A ResultListField describes data, called a field, that will be displayed in the Resut List Intermediate Format. 
    /// Including specifying where the data will come from. For example, the score the athlete shot in the Prone event.
    /// </summary>
    /// <remarks> The following are common fields that are always defined.
    /// <list type="bullet">
    /// <item>Rank</item> 
    /// <item>RankOrder</item>
    /// <item>Empty</item>
    /// <item>DisplayName</item> 
    /// <item>DisplayNameShort</item> 
    /// <item>DisplayNameAbbreviated</item>
    /// <item>FamilyName</item>
    /// <item>GivenName</item>
    /// <item>MiddleName</item>
    /// <item>HomeTown</item>
    /// <item>Country</item> 
    /// <item>Club</item>
    /// <item>CompetitorNumber</item> 
    /// <item>MatchLocation</item> 
    /// <item>MatchID</item> 
    /// <item>LocalDate</item> 
    /// <item>ResultCOFID</item> 
    /// <item>UserID</item>
    /// <item>Creator</item>
    /// <item>Owner</item>
    /// <item>Status</item>
    /// <item>TargetCollectionName</item>
    /// </list>"
    /// </remarks>
    public class ResultListField : IReconfigurableRulebookObject    {
        /// <summary>
        /// Public constructor
        /// </summary>
        public ResultListField() {
            Source = new FieldSource();
        }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {
            
            if (Source == null) 
                Source = new FieldSource();

        }

        /// <summary>
        /// The unique name for this ResultField. Must be unique within the Fields list
        /// of a RESULT LIST FORMAT, including the common (pre-defined) fields.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Defines the type of data to be displayed.
        /// </summary>
        public ResultFieldMethod Method { get; set; }

        /// <summary>
        /// With Method specifying the type of data, Source specified specifically where to pull the data. 
        /// The options for Source, are dependent on the value of Method.
        /// </summary>
        public FieldSource Source { get; set; }

        /// <inheritdoc/>
        public override string ToString() {
            return $"{FieldName} for {Method}";
        }

        /// <inheritdoc/>
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;
    }
}
