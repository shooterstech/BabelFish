using System.Text.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

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


        /// <summary>
        /// If the user specified a search term in the REST API call, SearchScore returns the relavenacy
        /// of this Definition compared to the search term. The higher the value the better the match.
        /// A value of -1 means no search term comparision was made.
        /// </summary>
        [DefaultValue( -1 )]
        [JsonIgnore]
        public int SearchScore { get; set; } = -1;
    }
}
