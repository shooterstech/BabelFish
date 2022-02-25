﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using BabelFish.Helpers;
using BabelFish.Responses;
using BabelFish.DataModel.Definitions;
using BabelFish.Requests.DefinitionAPI;
using BabelFish.Responses.DefinitionAPI;

namespace BabelFish {
    public class DefinitionAPIClient : APIClient {

        public DefinitionAPIClient(string apiKey) : base(apiKey) { }

        public async Task<GetDefinitionResponse<T>> GetDefinition<T>(Definition.DefinitionType type, SetName setName, GetDefinitionResponse<T> response)
        {
            /////Logic checking for caching function
            //1. Check for local file
            //1a. Continue API retrieval if user pref is to always pull new API
            //1b. If !exists local file, continue API retrieval
            //1b. If exists local file, check local file timestamp
            //1b1. Return if within acceptable timeframe (never check for new Version??)
            //1b2. Step2 if outside timeframe
            //2. Exists local file + Outside Timeframe
            //2a. API call for Version
            //2a1. No Version returned, (ERROR instance: Return local file or continue API retrieval??)
            //2a2. Version matches local file, return local file
            //2a3. Version returned ! match local file, continue API retrieval
            //if (CheckForLocalFile(setName))
            //    response = GetLocalFileObject();
            
            GetDefinitionRequest requestParameters = new GetDefinitionRequest(setName, type.Description());

            await this.CallAPI(requestParameters, response).ConfigureAwait(false);

            return response;
        }

        public async Task<GetDefinitionResponse<BabelFish.DataModel.Definitions.Attribute>> GetAttributeDefinitionAsync( SetName setName ) {

            //OLD METHOD FOR COMPARISON
            //var definitionType = Definition.DefinitionType.ATTRIBUTE;

            //GetDefinitionResponse<BabelFish.DataModel.Definitions.Attribute> response = new Responses.DefinitionAPI.GetDefinitionResponse<BabelFish.DataModel.Definitions.Attribute>( setName, definitionType );
            //GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType.Description() );

            //await this.CallAPI(request, response).ConfigureAwait(false);

            //return response;
            var definitionType = Definition.DefinitionType.ATTRIBUTE;

            GetDefinitionResponse<BabelFish.DataModel.Definitions.Attribute> response = new GetDefinitionResponse<BabelFish.DataModel.Definitions.Attribute>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<CourseOfFire>> GetCourseOfFireDefinition( SetName setName ) {

            var definitionType = Definition.DefinitionType.COURSEOFFIRE;

            GetDefinitionResponse<CourseOfFire> response = new GetDefinitionResponse<CourseOfFire>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<EventStyle>> GetEventStyleDefinition( SetName setName ) {

            var definitionType = Definition.DefinitionType.EVENTSTYLE;

            GetDefinitionResponse<EventStyle> response = new GetDefinitionResponse<EventStyle>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<RankingRule>> GetRankingRuleDefinition( SetName setName ) {

            var definitionType = Definition.DefinitionType.RANKINGRULES;

            GetDefinitionResponse<RankingRule> response = new GetDefinitionResponse<RankingRule>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<StageStyle>> GetStageStyleDefinition( SetName setName ) {

            var definitionType = Definition.DefinitionType.STAGESTYLE;

            GetDefinitionResponse<StageStyle> response = new GetDefinitionResponse<StageStyle>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<TargetCollection>> GetTargetCollectionDefinition( SetName setName ) {

            var definitionType = Definition.DefinitionType.TARGETCOLLECTION;

            GetDefinitionResponse<TargetCollection> response = new GetDefinitionResponse<TargetCollection>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<Target>> GetTargetDefinition( SetName setName ) {

            var definitionType = Definition.DefinitionType.TARGET;

            GetDefinitionResponse<Target> response = new GetDefinitionResponse<Target>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public async Task<GetDefinitionResponse<ScoreFormatCollection>> GetScoreFormatCollectionDefinition (SetName setName) {

            var definitionType = Definition.DefinitionType.SCOREFORMATCOLLECTION;

            GetDefinitionResponse<ScoreFormatCollection> response = new GetDefinitionResponse<ScoreFormatCollection>(setName, definitionType);

            return await GetDefinition(definitionType, setName, response).ConfigureAwait(false);
        }

        public ResultListFormat GetResultListFormatCollection( SetName setName ) {
            throw new NotImplementedException();
        }
    }
}