using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
	public class ResultEngineDirectives : IReconfigurableRulebookObject {

		public bool RecordCompareResultListNow { get; set; } = false;

		public bool DisableScoreProjection { get; set; } = false;

		/// <inheritdoc />
		[DefaultValue( "" )]
		[G_NS.JsonProperty( Order = 100 )]
		public string Comment { get; set; } = string.Empty;
	}
}
