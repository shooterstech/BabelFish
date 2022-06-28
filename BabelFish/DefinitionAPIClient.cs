using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using ShootersTech.Helpers;
using ShootersTech.Responses;
using ShootersTech.DataModel.Definitions;
using ShootersTech.Requests.DefinitionAPI;
using ShootersTech.Responses.DefinitionAPI;

namespace ShootersTech {
    public class DefinitionAPIClient : APIClient {

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        public DefinitionAPIClient(string apiKey) : base(apiKey) { }

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey">Your assigned XApiKey</param>
        /// <param name="CustomUserSettings">Dictionary<string,string> of Allowed User Settings</param>
        public DefinitionAPIClient(string xapikey, Dictionary<string, string> CustomUserSettings) : base(xapikey, CustomUserSettings) { }

        public async Task<GetDefinitionResponse<T>> GetDefinition<T>(GetDefinitionRequest request, GetDefinitionResponse<T> response) where T : new() 
        {
            /////Logic checking for caching function
            /// Reference https://github.com/shooterstech/BabelFish/issues/9
            // CHECK IF USER SETTINGS SAY TO ALWAYS GET DEFINITION FRESH'
            //if (!SettingsHelper.SettingIsNullOrEmpty("Definitions_CacheAlwaysNew") && SettingsHelper.UserSettings["Definitions_CacheAlwaysNew"])
            //1. Check for object in mem
            //1a. if exists, check timestamp
            //1a1. Return if within acceptable timeframe (never check for new Version??)
            //1a2. Step2 if outside timeframe
            //1b. if !exists, continue Step2
            //2. Check for local file
            //2a. Continue API retrieval if user pref is to always pull new API
            //2b. If !exists local file, continue API retrieval
            //2b. If exists local file, check local file timestamp
            //2b1. Return if within acceptable timeframe (never check for new Version??)
            //2b2. Step3 if outside timeframe
            //3. Exists local file + Outside Timeframe
            //3a. API call for Version
            //3a1. No Version returned, (ERROR instance: Return local file or continue API retrieval??)
            //3a2. Version matches local file, return local file
            //3a3. Version returned ! match local file, continue API retrieval
            //if (CheckForLocalFile(setName))
            //    response = GetLocalFileObject();

            await this.CallAPI(request, response).ConfigureAwait(false);

            //Preloading: set-and-forget async thread for objects => written file -> add to mem
            //  maybe check if user has AlwaysFetchDefinition=true and don't waste write/mem???

            return response;
        }

        /*
        public async Task<GetDefinitionResponse<ShootersTech.DataModel.Definitions.Attribute>> GetAttributeDefinitionAsync( SetName setName ) {

            //OLD METHOD FOR COMPARISON
            //var definitionType = Definition.DefinitionType.ATTRIBUTE;

            //GetDefinitionResponse<BabelFish.DataModel.Definitions.Attribute> response = new Responses.DefinitionAPI.GetDefinitionResponse<BabelFish.DataModel.Definitions.Attribute>( setName, definitionType );
            //GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType.Description() );

            //await this.CallAPI(request, response).ConfigureAwait(false);

            //return response;
            var definitionType = DefinitionType.ATTRIBUTE;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<BabelFish.DataModel.Definitions.Attribute> response = new GetDefinitionResponse<ShootersTech.DataModel.Definitions.Attribute>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }
        */

        public async Task<GetDefinitionResponse<CourseOfFire>> GetCourseOfFireDefinition( SetName setName ) {

            var definitionType = DefinitionType.COURSEOFFIRE;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<CourseOfFire> response = new GetDefinitionResponse<CourseOfFire>( request );

            return await GetDefinition( request, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<EventStyle>> GetEventStyleDefinition( SetName setName ) {

            var definitionType = DefinitionType.EVENTSTYLE;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<EventStyle> response = new GetDefinitionResponse<EventStyle>( request );

            return await GetDefinition( request, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<RankingRule>> GetRankingRuleDefinition( SetName setName ) {

            var definitionType = DefinitionType.RANKINGRULES;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<RankingRule> response = new GetDefinitionResponse<RankingRule>( request );

            return await GetDefinition( request, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<StageStyle>> GetStageStyleDefinition( SetName setName ) {

            var definitionType = DefinitionType.STAGESTYLE;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<StageStyle> response = new GetDefinitionResponse<StageStyle>( request );

            return await GetDefinition( request, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<TargetCollection>> GetTargetCollectionDefinition( SetName setName ) {

            var definitionType = DefinitionType.TARGETCOLLECTION;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<TargetCollection> response = new GetDefinitionResponse<TargetCollection>( request );

            return await GetDefinition( request, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<Target>> GetTargetDefinition( SetName setName ) {

            var definitionType = DefinitionType.TARGET;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<Target> response = new GetDefinitionResponse<Target>( request );

            return await GetDefinition( request, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<ScoreFormatCollection>> GetScoreFormatCollectionDefinition (SetName setName) {

            var definitionType = DefinitionType.SCOREFORMATCOLLECTION;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<ScoreFormatCollection> response = new GetDefinitionResponse<ScoreFormatCollection>(request);

            return await GetDefinition( request, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<ResultListFormat>> GetResultListFormatCollection( SetName setName ) {

            var definitionType = DefinitionType.RESULTLISTFORMAT;

            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType );

            GetDefinitionResponse<ResultListFormat> response = new GetDefinitionResponse<ResultListFormat>( request );

            return await GetDefinition( request, response ).ConfigureAwait( false );
        }
    }
}
