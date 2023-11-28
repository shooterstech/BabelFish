using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Responses.DefinitionAPI;

namespace Scopos.BabelFish.APIClients {
	/// <summary>
	/// The DefinitionFetcher is a Definition API Client, intended for internal use of BabelFish. 
	/// 
	/// this class is expected to be used within the instantiation of Attribute Values, which require the 
	/// knowledge of the attribute definition, and within Definiton classes themselves, to retreive
	/// other definitions that they link to.
	/// 
	/// There is no public access to his class, other than setting the X API Key. 
	/// 
	/// Protected internal access to the class is through the static class variable FETCHER
	/// 
	/// To use this class, the FETCHER's .XApiKey property must be set first.
	/// 
	/// NOTE: Choosing to override only the non-obsolte funciton sin the base DefinitionAPIClient
	/// </summary>
	public class DefinitionFetcher : DefinitionAPIClient  {


		protected internal static DefinitionFetcher FETCHER { get; private set; }

		private static string xApiKey = "";

		private static Logger logger = LogManager.GetCurrentClassLogger();

		private DefinitionFetcher(string apiKey) : base(apiKey) { }

		/// <summary>
		/// Sets the x-api-key to use when making calls to read new attribute definitions.
		/// Also reinstntiates the definition api client, using the new x-api-key.
		/// </summary>
		public static string XApiKey {
			get {
				return xApiKey;
			}
			set {
				if (xApiKey != value && !string.IsNullOrEmpty( value )) {
					xApiKey = value;
					FETCHER = new DefinitionFetcher( xApiKey );
				}
			}
		}

		/// <summary>
		/// Returns a boolean indicating if the X API Key has been set.
		/// This only validates that it's been set, not that the value is a good one.
		/// </summary>
		public static bool IsXApiKeySet {
			get {
				return !string.IsNullOrEmpty( xApiKey );
			}
		}
		
		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<DataModel.Definitions.Attribute>> GetAttributeDefinitionAsync( SetName setName ) {
			if (string.IsNullOrEmpty( xApiKey )) {
				throw new XApiKeyNotSetException();
			}

			return base.GetAttributeDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<CourseOfFire>> GetCourseOfFireDefinitionAsync( SetName setName ) {
			if (string.IsNullOrEmpty( xApiKey )) {
				throw new XApiKeyNotSetException();
			}

			return base.GetCourseOfFireDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<EventAndStageStyleMapping>> GetEventAndStageStyleMappingDefinitionAsync( SetName setName ) {
			if (string.IsNullOrEmpty( xApiKey )) {
				throw new XApiKeyNotSetException();
			}

			return base.GetEventAndStageStyleMappingDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<EventStyle>> GetEventStyleDefinitionAsync( SetName setName ) {
			if (string.IsNullOrEmpty( xApiKey )) {
				throw new XApiKeyNotSetException();
			}

			return base.GetEventStyleDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<RankingRule>> GetRankingRuleDefinitionAsync( SetName setName ) {
			if (string.IsNullOrEmpty( xApiKey )) {
				throw new XApiKeyNotSetException();
			}

			return base.GetRankingRuleDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<ResultListFormat>> GetResultListFormatDefinitionAsync( SetName setName ) {
			if (string.IsNullOrEmpty( xApiKey )) {
				throw new XApiKeyNotSetException();
			}

			return base.GetResultListFormatDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<ScoreFormatCollection>> GetScoreFormatCollectionDefinitionAsync( SetName setName ) {
			if (string.IsNullOrEmpty( xApiKey )) {
				throw new XApiKeyNotSetException();
			}

			return base.GetScoreFormatCollectionDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<StageStyle>> GetStageStyleDefinitionAsync( SetName setName ) {
			if (string.IsNullOrEmpty( xApiKey )) {
				throw new XApiKeyNotSetException();
			}

			return base.GetStageStyleDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<TargetCollectionDefinition>> GetTargetCollectionDefinitionAsync( SetName setName ) {
			if (string.IsNullOrEmpty( xApiKey )) {
				throw new XApiKeyNotSetException();
			}

			return base.GetTargetCollectionDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<Target>> GetTargetDefinitionAsync( SetName setName ) {
			if (string.IsNullOrEmpty( xApiKey )) {
				throw new XApiKeyNotSetException();
			}

			return base.GetTargetDefinitionAsync( setName );
		}
	}
}
