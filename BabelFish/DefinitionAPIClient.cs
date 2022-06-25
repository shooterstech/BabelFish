using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using BabelFish.Helpers;
using BabelFish.Responses;
using BabelFish.DataModel.Definitions;
using BabelFish.Requests.DefinitionAPI;
using BabelFish.Responses.DefinitionAPI;

namespace BabelFish
{
    public class DefinitionAPIClient : APIClient
    {
        DefinitionCacheHelper definitionCacheHelper;
        private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="apiKey"></param>
        public DefinitionAPIClient(string apiKey) : base(apiKey) 
        {
            definitionCacheHelper = new DefinitionCacheHelper();
        }

        /// <summary>
        /// Instantiate client
        /// </summary>
        /// <param name="xapikey">Your assigned XApiKey</param>
        /// <param name="CustomUserSettings">Dictionary<string,string> of Allowed User Settings</param>
        public DefinitionAPIClient(string xapikey, Dictionary<string, string> CustomUserSettings) : base(xapikey, CustomUserSettings) 
        {
            definitionCacheHelper = new DefinitionCacheHelper();
        }

        public async Task<GetDefinitionResponse<T>> GetDefinition<T>(Definition.DefinitionType type, SetName setName, GetDefinitionResponse<T> response)
        {
            try{
                // TimeToRun if we use cache
                DateTime startTime = DateTime.Now;

                // Check cache (memory then file system) for existing, non-expired Definition
                DefinitionCache? cachedDefinition = definitionCacheHelper.GetCachedDefinition(type, setName);

                if (cachedDefinition != null)
                {
                    if (definitionCacheHelper.LastException != "")
                    {
                        logger.Error($"Definition Cache Error: {definitionCacheHelper.LastException}");
                        response.MessageResponse.Message.Add($"Definition Cache Error: {definitionCacheHelper.LastException}");
                        response.MessageResponse.ResponseCodes.Add("Definition Cache Error");
                    }

                    //Craft response object
                    response.Body = JToken.Parse(cachedDefinition.DefinitionJSON);
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.TimeToRun = DateTime.Now - startTime;
                }
                else
                {
                    GetDefinitionRequest requestParameters = new GetDefinitionRequest(setName, type.Description());

                    await this.CallAPI(requestParameters, response).ConfigureAwait(false);

                    // Save returned definition to cache
                    if ( response.Body != null )
                        definitionCacheHelper.SaveDefinitionToCache(response.Body.ToString(), type, setName);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"GetDefinition Error: {ex.Message}");
                response.MessageResponse.Message.Add($"GetDefinition Error: {ex.Message}");
                response.MessageResponse.ResponseCodes.Add("GetDefinition Error");
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }

            return response;
        }

        public async Task<GetDefinitionResponse<BabelFish.DataModel.Definitions.Attribute>> GetAttributeDefinitionAsync(SetName setName)
        {
            var definitionType = Definition.DefinitionType.ATTRIBUTE;

            GetDefinitionResponse<BabelFish.DataModel.Definitions.Attribute> response = new GetDefinitionResponse<BabelFish.DataModel.Definitions.Attribute>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<CourseOfFire>> GetCourseOfFireDefinition(SetName setName)
        {

            var definitionType = Definition.DefinitionType.COURSEOFFIRE;

            GetDefinitionResponse<CourseOfFire> response = new GetDefinitionResponse<CourseOfFire>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<EventStyle>> GetEventStyleDefinition(SetName setName)
        {

            var definitionType = Definition.DefinitionType.EVENTSTYLE;

            GetDefinitionResponse<EventStyle> response = new GetDefinitionResponse<EventStyle>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<RankingRule>> GetRankingRuleDefinition(SetName setName)
        {

            var definitionType = Definition.DefinitionType.RANKINGRULES;

            GetDefinitionResponse<RankingRule> response = new GetDefinitionResponse<RankingRule>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<StageStyle>> GetStageStyleDefinition(SetName setName)
        {

            var definitionType = Definition.DefinitionType.STAGESTYLE;

            GetDefinitionResponse<StageStyle> response = new GetDefinitionResponse<StageStyle>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<TargetCollection>> GetTargetCollectionDefinition(SetName setName)
        {

            var definitionType = Definition.DefinitionType.TARGETCOLLECTION;

            GetDefinitionResponse<TargetCollection> response = new GetDefinitionResponse<TargetCollection>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<Target>> GetTargetDefinition(SetName setName)
        {

            var definitionType = Definition.DefinitionType.TARGET;

            GetDefinitionResponse<Target> response = new GetDefinitionResponse<Target>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<ScoreFormatCollection>> GetScoreFormatCollectionDefinition(SetName setName)
        {

            var definitionType = Definition.DefinitionType.SCOREFORMATCOLLECTION;

            GetDefinitionResponse<ScoreFormatCollection> response = new GetDefinitionResponse<ScoreFormatCollection>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<ResultListFormat>> GetResultListFormatCollection(SetName setName)
        {

            var definitionType = Definition.DefinitionType.RESULTLISTFORMAT;

            GetDefinitionResponse<ResultListFormat> response = new GetDefinitionResponse<ResultListFormat>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }
    }
}