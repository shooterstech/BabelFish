using System.Text.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using Scopos.BabelFish.Converters.Microsoft;

namespace Scopos.BabelFish.DataModel.Definitions {
	public class SparseDefinitionList : ITokenItems<SparseDefinitionSearchResult> {

		public SparseDefinitionList() {
			Items = new List<SparseDefinitionSearchResult>();
		}

		[OnDeserialized]
		internal void OnDeserialized( StreamingContext context ) {
			if (Items == null)
				Items = new List<SparseDefinitionSearchResult>();
		}

		/// <inheritdoc />
		public List<SparseDefinitionSearchResult> Items { get; set; }

		/// <inheritdoc />
		[JsonConverter(typeof(NextTokenConverter))]
		public string NextToken { get; set; } = string.Empty;

		/// <inheritdoc />
		public int Limit { get; set; } = 100;


		/// <inheritdoc />
		public bool HasMoreItems {
			get {
				return !string.IsNullOrEmpty( NextToken );
			}
		}

		public override string ToString() {
			return $"SparseDefinitionList with {Items.Count} items";
		}
	}

	public class SparseDefinitionSearchResult : SparseDefinition {

		private string _commonName = null;
		public string CommonName {
			get {
				if (string.IsNullOrEmpty( _commonName )) {
					SetName sn;
					if (Scopos.BabelFish.DataModel.Definitions.SetName.TryParse( this.SetName, out sn )) {
						return sn.ProperName;
					}
					//Shouldn't ever really get here, b/c every Definition should/better have a SetName.
					return "Unknown";
				} else {
					return _commonName;
				}
			}
			set {
				if (!string.IsNullOrEmpty( value )) {
					_commonName = value;
				}
			}
		}

		public string Description {  get; set; } = string.Empty;

		public string OwnerId { get; set; } = string.Empty;

		public string OwnerName { get; set; } = string.Empty;

		public string Discipline {  get; set; } = string.Empty;

		public List<string> Tags { get; set; } = new List<string>();

		public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// If the user specified a search term in the REST API call, SearchScore returns the relavenacy
        /// of this Definition compared to the search term. The higher the value the better the match.
        /// A value of -1 means no search term comparision was made.
        /// </summary>
        [DefaultValue( -1 )]
        public float SearchScore { get; set; } = -1;
    }
}
