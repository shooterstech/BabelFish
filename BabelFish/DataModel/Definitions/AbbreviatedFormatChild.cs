using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataActors.OrionMatch;
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
	}


	/// <summary>
	/// The .EventName is the explicit Event Name to return, as is the Display Event Name.
	/// </summary>
	/// <remarks>
	/// JSON Example
	/// <code>
	/// {
	///    "EventName": "ST 1",
	///    “DERIVATION” : “EXPLICIT”
	/// }
	/// </code>
	/// </remarks>
	public class AbbreviatedFormatChildExplicit : AbbreviatedFormatChild {

		public AbbreviatedFormatChildExplicit() : base() {
			this.Derivation = EventDerivationType.EXPLICIT;
		}

		/// <summary>
		/// Helper constructor, intended to work with the method GetCompiledAbbreviatedFormatChildren().
		/// </summary>
		/// <param name="copyFrom"></param>
		/// <param name="eventName"></param>
		/// <param name="eventDisplayName"></param>
		public AbbreviatedFormatChildExplicit( AbbreviatedFormatChild copyFrom, string eventName, string eventDisplayName ) {
			this.Derivation = EventDerivationType.EXPLICIT;
			this.EventName = EventName;
			this.EventDisplayName = eventDisplayName;
			this.Comment = copyFrom.Comment;
		}

		/// <inheritdoc />
		public override List<AbbreviatedFormatChild> GetCompiledAbbreviatedFormatChildren( IEventScoreProjection re ) {
			List<AbbreviatedFormatChild> list = new List<AbbreviatedFormatChild>();
			list.Add( this.Clone() );
			return list;
		}
	}

	/// <summary>
	/// The .EventName should have a wild card character {}. Optionally, but probable a good idea
	/// so should the EventDisplayName property. 
	/// Values are derived using the method specified in .Values.	/// 
	/// </summary>
	/// <remarks>
	/// JSON Example
	/// <code>
	/// {
	///	   "Values": "LAST(3)",
	///    "EventName": "ST {}",
	///    “DERIVATION” : “DERIVED”
	/// }
	/// </code>
	/// </remarks>
	public class AbbreviatedFormatChildDerived : AbbreviatedFormatChild {

		public AbbreviatedFormatChildDerived() : base() {
			this.Derivation = EventDerivationType.DERIVED;
		}

		/// <summary>
		/// The method to use to return appropriate Event Names.
		/// </summary>
		[G_NS.JsonProperty( Order = 16 )]
		public AbbreviatedFormatDerivedOptions Values { get; set; } = AbbreviatedFormatDerivedOptions.LAST_1;

		/// <inheritdoc />
		public override List<AbbreviatedFormatChild> GetCompiledAbbreviatedFormatChildren( IEventScoreProjection re ) {
			List<AbbreviatedFormatChild> list = new List<AbbreviatedFormatChild>();
			ValueSeries vs = new ValueSeries( "100..1,-1" );
			var eventNameList = vs.GetAsList( this.EventName );
			var displayNameList = string.IsNullOrEmpty( this.EventDisplayName ) ? vs.GetAsList( this.EventName ) : vs.GetAsList( this.EventDisplayName );
            var count = eventNameList.Count;

			var numberToInclude = 0;
			switch (this.Values) {
				default:
				case AbbreviatedFormatDerivedOptions.LAST_1:
					numberToInclude = 1;
					break;
				case AbbreviatedFormatDerivedOptions.LAST_2:
					numberToInclude = 2;
					break;
				case AbbreviatedFormatDerivedOptions.LAST_3:
					numberToInclude = 3;
					break;
				case AbbreviatedFormatDerivedOptions.LAST_4:
					numberToInclude = 4;
					break;
				case AbbreviatedFormatDerivedOptions.LAST_5:
					numberToInclude = 5;
					break;
				case AbbreviatedFormatDerivedOptions.LAST_6:
					numberToInclude = 6;
					break;
			}

			for (int i = 0; i < count; i++) {
				if (re.EventScores.TryGetValue( eventNameList[i], out EventScore es ) && es.NumShotsFired > 0) {
					list.Add( new AbbreviatedFormatChildExplicit( this, eventNameList[i], displayNameList[i] ) );
				}

				if (list.Count >= numberToInclude)
					break;
			}

			return list;

		}
	}

	/// <summary>
	/// The .EventName should have a wild card character {}. Optionally, but probable a good idea
	/// so should the EventDisplayName property. 
	/// Values are derived using the Value Series specified in .Values.
	/// </summary>
	/// <remarks>
	/// JSON Example
	/// <code>
	/// {
	///	   "Values": "1..4",
	///    "EventName": "ST {}",
	///    “DERIVATION” : “EXPAND”
	/// }
	/// </code>
	/// </remarks>
	public class AbbreviatedFormatChildExpand : AbbreviatedFormatChild {

		public AbbreviatedFormatChildExpand() : base() {
			this.Derivation = EventDerivationType.EXPAND;
		}

		/// <summary>
		/// Formatted as a Value Series
		/// </summary>
		[G_NS.JsonProperty( Order = 16 )]
		public string Values { get; set; } = string.Empty;

		/// <inheritdoc />
		public override List<AbbreviatedFormatChild> GetCompiledAbbreviatedFormatChildren( IEventScoreProjection re ) {
			List<AbbreviatedFormatChild> list = new List<AbbreviatedFormatChild>();
			ValueSeries vs = new ValueSeries( this.Values );
			var eventNameList = vs.GetAsList( this.EventName );
			var displayNameList = string.IsNullOrEmpty( this.EventDisplayName ) ? vs.GetAsList( this.EventName ) : vs.GetAsList( this.EventDisplayName );
			var count = eventNameList.Count;

			for (int i = 0; i < count; i++) {
				list.Add( new AbbreviatedFormatChildExplicit( this, eventNameList[i], displayNameList[i] ) );
			}

			return list;
		}
	}
}
