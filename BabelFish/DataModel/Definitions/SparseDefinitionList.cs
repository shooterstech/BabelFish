using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
	public class SparseDefinitionList : ITokenItems<SparseDefinition> {

		public SparseDefinitionList() {
			Items = new List<SparseDefinition>();
		}

		[OnDeserialized]
		internal void OnDeserialized( StreamingContext context ) {
			if (Items == null)
				Items = new List<SparseDefinition>();
		}

		/// <inheritdoc />
		public List<SparseDefinition> Items { get; set; }

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
}
