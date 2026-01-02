using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using NLog;
using System.Text.Json;
using Scopos.BabelFish.Converters.Microsoft;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// Response object for a request of Squadding Assignments for a specified match and squadding event name.
    /// </summary>
    public class SquaddingList : ITokenItems<Squadding>, IRLIFList, IPublishTransactions {

        private Logger logger = LogManager.GetCurrentClassLogger();

        public SquaddingList() {
            Items = new List<Squadding>();
        }

        [OnDeserialized]
        internal void OnDeserialized( StreamingContext context ) {
            if (Items == null)
                Items = new List<Squadding>();
        }

		/// <summary>
		/// The name of the squadding event, that this Squadding List is for.
		/// </summary>
		[G_NS.JsonProperty( Order = 1 )]
		public string EventName { get; set; }

		/// <summary>
		/// The name of the match that this squadding list is from.
		/// </summary>
		[G_NS.JsonProperty( Order = 2 )]
		public string MatchName { get; set; }

		/// <summary>
		/// The Owner of this data. e.g. OrionAcct001234
		/// </summary>
		[G_NS.JsonProperty( Order = 3 )]
		public string OwnerId { get; set; } = string.Empty;

		/// <summary>
		/// Start date for Match.
		/// </summary>
		[G_STJ_SER.JsonConverter( typeof( Scopos.BabelFish.Converters.Microsoft.ScoposDateOnlyConverter ) )]
		[G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
		[G_NS.JsonProperty( Order = 4 )]
		public DateTime StartDate { get; set; } = DateTime.Today;

		/// <summary>
		/// End date for the Match.
		/// </summary>
		[G_STJ_SER.JsonConverter( typeof( ScoposDateOnlyConverter ) )]
		[G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateConverter ) )]
		[G_NS.JsonProperty( Order = 5 )]
		public DateTime EndDate { get; set; } = DateTime.Today;

		/// <summary>
		/// Formatted as a string, the date and time this squadding list was last updated.
		/// Use GetLastUpdated() to return this value as a DateTime object.
		/// </summary>
		[G_STJ_SER.JsonConverter( typeof( Scopos.BabelFish.Converters.Microsoft.ScoposDateTimeConverter ) )]
        [G_NS.JsonConverter( typeof( G_BF_NS_CONV.DateTimeConverter ) )]
		[G_NS.JsonProperty( Order = 6 )]
		public DateTime LastUpdated { get; set; }

		/// <summary>
		/// Formatted as a string, the Match ID that this squadding list is from.
		/// Use GetMatchID() to return the value as a MatchID object.
		/// </summary>
		[G_NS.JsonProperty( Order = 7 )]
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
		[G_NS.JsonProperty( Order = 8 )]
		public string ParentID { get; set; }

        public MatchID GetParentID() {
            //NOTE that I am using the GetMatchID value to calculate the value for ParentID. This *should* be the
            //same as the ParentID property.
            return GetMatchID().GetParentMatchID();
		}

		/// <summary>
		/// String holding the software (Orion Scoring System) and Version number of the software.
		/// </summary>
		[G_NS.JsonProperty( Order = 9 )]
		public string Creator { get; set; } = string.Empty;

		/// <summary>
		/// Set name of the Result List Format definition to use when displaying this squadding list.
		/// </summary>
		[JsonPropertyOrder( 10 )]
		public string ResultListFormatDef { get; set; } = string.Empty;

		/// <summary>
		/// List of SquaddingAssignments (e.g. Individuals and where and when they will shoot). 
		/// </summary>
		[G_NS.JsonProperty( Order = 20 )]
		public List<Squadding> Items { get; set; }

		/// <inheritdoc />
		public List<IRLIFItem> GetAsIRLItemsList() {
			return Items.ToList<IRLIFItem>();
		}

		/// <inheritdoc />
		[DefaultValue( "" )]
        [JsonConverter( typeof( NextTokenConverter ) )]
		[G_NS.JsonProperty( Order = 21 )]
		public string NextToken { get; set; } = string.Empty;

        /// <inheritdoc />
        [DefaultValue( 0 )]
		[G_NS.JsonProperty( Order = 22 )]
		public int Limit { get; set; } = 0;

        /// <inheritdoc />
        [DefaultValue( false )]
		[G_NS.JsonProperty( Order = 23 )]
		public bool HasMoreItems {
            get {
                return !string.IsNullOrEmpty( NextToken );
            }
        }

		/// <summary>
		/// Call to line and start times for each relay. 
		/// </summary>
		public List<RelayInformation> RelayInformation { get; set; }

		/// <summary>
		/// Helper method to generate a list of Relays for RelayInformation.
		/// <para>If .RelayInformation alreayd has values, then this method does not change anything.</para>
		/// <para>Currently only works with FiringPoints.</para>
		/// </summary>
		public void GenerateRelayInformation() {
			if (!( RelayInformation is null || RelayInformation.Count == 0 ))
				return;

			HashSet<string> seenRelays = new HashSet<string>();
			RelayInformation = new List<RelayInformation>();
			foreach( var item in Items ) {
				
				if ( item.SquaddingAssignment is SquaddingAssignmentFiringPoint squaddingFiringPoint 
					&& ! seenRelays.Contains( squaddingFiringPoint.Relay ) ) {
					this.RelayInformation.Add( new RelayInformationFiringPoint() { RelayName = squaddingFiringPoint.Relay } );
					seenRelays.Add( squaddingFiringPoint.Relay );
				}

			}
		}

        /// <summary>
        /// Returns a subset of .Items, specically Squadding instances that are both SquaddingAssignmentFiringPoint
        /// and has the same Relay name as the passed in RelayInformation.
        /// </summary>
        /// <param name="relayInformation"></param>
        /// <returns></returns>
        public List<Squadding> FilterByRelayInformation( RelayInformationFiringPoint relayInformation ) {
			List<Squadding> list = new List<Squadding>();

			foreach( var squadding in this.Items ) {
				if ( squadding.SquaddingAssignment is SquaddingAssignmentFiringPoint sa
					&& sa.Relay == relayInformation.RelayName ) {
					list.Add( squadding );
				}
			}

			return list;
        }

        /// <summary>
        /// Returns a subset of .Items, specically Squadding instances that are both SquaddingAssignmentBank
        /// and has the same Relay name as the passed in RelayInformation.
        /// </summary>
        /// <param name="relayInformation"></param>
        /// <returns></returns>
        public List<Squadding> FilterByRelayInformation( RelayInformationBank relayInformation ) {
            List<Squadding> list = new List<Squadding>();

            foreach (var squadding in this.Items) {
                if (squadding.SquaddingAssignment is SquaddingAssignmentBank sa
                    && sa.Relay == relayInformation.RelayName) {
                    list.Add( squadding );
                }
            }

            return list;
        }

		public List<Squadding> FilterByRelayInformation( RelayInformation relayInformation ) {
			if ( relayInformation is RelayInformationFiringPoint )
				return FilterByRelayInformation((RelayInformationFiringPoint) relayInformation );

			if ( relayInformation is RelayInformationBank )
				return FilterByRelayInformation((RelayInformationBank) relayInformation );

			if ( relayInformation is RelayInformationSquad)
				return FilterByRelayInformation((RelayInformationSquad) relayInformation );

			throw new NotImplementedException( $"Unable to identify the concrete class for {relayInformation}.");
		}

		/// <summary>
		/// Returns a subset of .Items, specically Squadding instances that are both SquaddingAssignmentSquad
		/// and has the same Squad name as the passed in RelayInformation.
		/// </summary>
		/// <param name="relayInformation"></param>
		/// <returns></returns>
        public List<Squadding> FilterByRelayInformation( RelayInformationSquad relayInformation ) {
            List<Squadding> list = new List<Squadding>();

            foreach (var squadding in this.Items) {
                if (squadding.SquaddingAssignment is SquaddingAssignmentSquad sa
                    && sa.Squad == relayInformation.RelayName) {
                    list.Add( squadding );
                }
            }

            return list;
        }

        /// <summary>
        /// NewtonSoft helper method to determine if .RelayInformation should be serialized.
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRelayInformation() {
			return RelayInformation != null && RelayInformation.Count > 0;
		}

        /// <inheritdoc />
        [DefaultValue( "" )]
		[G_NS.JsonProperty( Order = 44 )]
		public string PublishTransactionId { get; set; } = string.Empty;

        /// <inheritdoc />
        [DefaultValue( 0 )]

		[G_NS.JsonProperty( Order = 45 )]
		public int TransactionSequence { get; set; } = 0;

        /// <inheritdoc />
        [DefaultValue( 1 )]
		[G_NS.JsonProperty( Order = 46 )]
		public int TransactionCount { get; set; } = 1;

		/// <inheritdoc />
		[G_NS.JsonIgnore]
		public string Name {
			get {
				return this.EventName;
			}
		}

		/// <inheritdoc />
		[G_NS.JsonIgnore]
		public ResultStatus Status {
			get {
				return ResultStatus.FUTURE;
			}
		}

		public override string ToString() {
            return $"SquaddingList with {Items.Count} items";
        }
	}
}
