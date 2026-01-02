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
	/// NOTE: Choosing to override only the non-obsolte funciton sin the base DefinitionAPIClient
	/// </summary>
	public class DefinitionFetcher : DefinitionAPIClient  {

		private static DefinitionFetcher _fetcher = null;

        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        protected internal static DefinitionFetcher FETCHER {
			get {
				if (_fetcher == null) {
					_fetcher = new DefinitionFetcher();
				}
				return _fetcher;
			}
		}

		private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <exception cref="XApiKeyNotSetException">Thrown if the Settings.XApiKey value has not been set.</exception>
        private DefinitionFetcher() : base() { }
		
		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<DataModel.Definitions.Attribute>> GetAttributeDefinitionAsync( SetName setName ) {

			return base.GetAttributeDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<CourseOfFire>> GetCourseOfFireDefinitionAsync( SetName setName ) {

			return base.GetCourseOfFireDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<EventAndStageStyleMapping>> GetEventAndStageStyleMappingDefinitionAsync( SetName setName ) {

			return base.GetEventAndStageStyleMappingDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<EventStyle>> GetEventStyleDefinitionAsync( SetName setName ) {

			return base.GetEventStyleDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<RankingRule>> GetRankingRuleDefinitionAsync( SetName setName ) {

			return base.GetRankingRuleDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<ResultListFormat>> GetResultListFormatDefinitionAsync( SetName setName ) {

			return base.GetResultListFormatDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<ScoreFormatCollection>> GetScoreFormatCollectionDefinitionAsync( SetName setName ) {

			return base.GetScoreFormatCollectionDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<StageStyle>> GetStageStyleDefinitionAsync( SetName setName ) {

			return base.GetStageStyleDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<TargetCollection>> GetTargetCollectionDefinitionAsync( SetName setName ) {

			return base.GetTargetCollectionDefinitionAsync( setName );
		}

		/// <inheritdoc/>
		/// <exception cref="XApiKeyNotSetException"></exception>
		public override Task<GetDefinitionPublicResponse<Target>> GetTargetDefinitionAsync( SetName setName ) {

			return base.GetTargetDefinitionAsync( setName );
		}
	}
}
