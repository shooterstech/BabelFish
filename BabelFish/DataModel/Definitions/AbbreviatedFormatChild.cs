using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataModel.Definitions {
	public abstract class AbbreviatedFormatChild : AbbreviatedFormatBase {

		public AbbreviatedFormatChild() : base() { }

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>Acts as the concrete class identifier.</remarks>
		[G_NS.JsonProperty( Order = 15, DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include )]
		public EventDerivationType Derivation { get; protected set; } = EventDerivationType.EXPLICIT;

		public abstract List<AbbreviatedFormatChild> GetCompiledAbbreviatedFormatChildren(ResultEvent re);
	}

	public class AbbreviatedFormatChildExplicit : AbbreviatedFormatChild {

		public AbbreviatedFormatChildExplicit() : base() {
			this.Derivation = EventDerivationType.EXPLICIT;
		}

		public override List<AbbreviatedFormatChild> GetCompiledAbbreviatedFormatChildren( ResultEvent re ) {
			throw new NotImplementedException();
		}
	}

	public class AbbreviatedFormatChildDerived : AbbreviatedFormatChild {

		public AbbreviatedFormatChildDerived() : base() {
			this.Derivation = EventDerivationType.DERIVED;
		}

		/// <summary>
		/// 
		/// </summary>
		[G_NS.JsonProperty( Order = 16 )]
		public AbbreviatedFormatDerivedOptions Values { get; set; } = AbbreviatedFormatDerivedOptions.LAST_1;

		public override List<AbbreviatedFormatChild> GetCompiledAbbreviatedFormatChildren( ResultEvent re ) {
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// The .EventName should have a wild card character {}. Optionally, but probable a good idea
	/// so should the EventDisplayName property.
	/// </summary>
	public class AbbreviatedFormatChildExpand : AbbreviatedFormatChild {

		public AbbreviatedFormatChildExpand() : base() {
			this.Derivation = EventDerivationType.EXPAND;
		}

		/// <summary>
		/// Formatted as a Value Series
		/// </summary>
		[G_NS.JsonProperty( Order = 16 )]
		public string Values { get; set; } = string.Empty;

		public override List<AbbreviatedFormatChild> GetCompiledAbbreviatedFormatChildren( ResultEvent re ) {
			throw new NotImplementedException();
		}
	}
}
