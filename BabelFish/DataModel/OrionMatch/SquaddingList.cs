﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using NLog;
using Newtonsoft.Json;
using Scopos.BabelFish.Converters;
using DateTimeConverter = Scopos.BabelFish.Converters.DateTimeConverter;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Response object for a request of Squadding Assignments for a specified match and squadding event name.
    /// </summary>
    public class SquaddingList : ITokenItems<SquaddingAssignment>, IPublishTransactions {

        private Logger logger = LogManager.GetCurrentClassLogger();

        public SquaddingList() {
            Items = new List<SquaddingAssignment>();
        }

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (Items == null)
                Items = new List<SquaddingAssignment>();
        }

        /// <summary>
        /// The name of the squadding event, that this Squadding List is for.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// Formatted as a string, the date and time this squadding list was last updated.
        /// Use GetLastUpdated() to return this value as a DateTime object.
        /// </summary>
        [Obsolete("LastUpdated will soon be a property on each seperate SquaddingAssignment, instead of the list as a whole.")]
        [JsonConverter( typeof( DateTimeConverter ) )]
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Start date for the ResultList of the Match. Used to guage what the Status of the Result list is.
        /// need defaults?
        /// </summary>
        [JsonConverter( typeof( DateConverter ) )]
        public DateTime StartDate { get; set; } = DateTime.Today;

        /// <summary>
        /// End date for the ResultList of the Match. Used to guage what the Status of the ResultList is.
        /// need defaults?
        /// </summary>
        [JsonConverter( typeof( DateConverter ) )]
        public DateTime EndDate { get; set; } = DateTime.Today;

        /// <summary>
        /// Formatted as a string, the Match ID that this squadding list is from.
        /// Use GetMatchID() to return the value as a MatchID object.
        /// </summary>
        public string MatchID { get; set; }

        /// <summary>
        /// The Match ID that this squadding list is from.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FormatException">Thrown if unable to parse the property MatchID into a MatchID object.</exception>
        public MatchID GetMatchID() {
            try {
                return new MatchID( MatchID );
            } catch (Exception ex) {
                //Probable either a FormatException or a NullValueException
                var msg = $"Can not parse MatchID values of '{MatchID}'. Received error {ex}.";
                logger.Error( msg, ex );
                throw new FormatException( msg );
            }
        }

        /// <summary>
        /// If this squadding list a Virtual Match, this is the Parent ID of the match. If this is a local match, then this 
        /// value will be the same as MatchID.
        /// </summary>
        public string ParentID { get; set; }

        public MatchID GetParentID() {
            //NOTE that I am using the GetMatchID value to calculate the value for ParentID. This *should* be the
            //same as the ParentID property.
            return GetMatchID().GetParentMatchID();
        }

        /// <summary>
        /// The name of the match that this squadding list is from.
        /// </summary>
        public string MatchName { get; set; }

        /// <summary>
        /// List of SquaddingAssignments (e.g. Individuals and where and when they will shoot). 
        /// </summary>
        public List<SquaddingAssignment> Items { get; set; }

        /// <inheritdoc />
        [DefaultValue( "" )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        [DefaultValue( 0 )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public int Limit { get; set; } = 0;

        /// <inheritdoc />
        [DefaultValue( false )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public bool HasMoreItems {
            get {
                return !string.IsNullOrEmpty( NextToken );
            }
        }

        /// <inheritdoc />
        [DefaultValue( "" )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public string PublishTransactionId { get; set; } = string.Empty;

        /// <inheritdoc />
        [DefaultValue( 0 )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public int TransactionSequence { get; set; } = 0;

        /// <inheritdoc />
        [DefaultValue( 1 )]
        [JsonProperty( DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate )]
        public int TransactionCount { get; set; } = 1;

        public override string ToString() {
            return $"SquaddingList with {Items.Count} items";
        }
    }
}
