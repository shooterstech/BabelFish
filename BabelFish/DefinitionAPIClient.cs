using System;
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

        //public async Task<T> GetDefinition<T>(Definition.DefinitionType type, SetName setName, Response<T> response)
        //{
        //    //throw new NotImplementedException();
        //    GetDefinitionRequest requestParameters = new GetDefinitionRequest(setName, EnumHelper.GetAttributeOfType<EnumMemberAttribute>(type).Value);

        //    await this.CallAPI(requestParameters, response).ConfigureAwait(false);

        //    return response;
        //}

        public async Task<GetDefinitionResponse<BabelFish.DataModel.Definitions.Attribute>> GetAttributeDefinitionAsync( SetName setName ) {

            var definitionType = Definition.DefinitionType.ATTRIBUTE;

            GetDefinitionResponse<BabelFish.DataModel.Definitions.Attribute> response = new Responses.DefinitionAPI.GetDefinitionResponse<BabelFish.DataModel.Definitions.Attribute>( setName, definitionType );
            GetDefinitionRequest request = new GetDefinitionRequest( setName, definitionType.Description() );
            
            await this.CallAPI(request, response).ConfigureAwait(false);

            return response;
        }

        public CourseOfFire GetCourseOfFireDefinition( SetName setName ) {
            throw new NotImplementedException();
        }

        public EventStyle GetEventStyleDefinition( SetName setName ) {
            throw new NotImplementedException();
        }

        public RankingRule GetRankingRuleDefinition( SetName setName ) {
            throw new NotImplementedException();
        }

        public StageStyle GetStageStyleDefinition( SetName setName ) {
            throw new NotImplementedException();
        }

        public TargetCollection GetTargetCollectionDefinition( SetName setName ) {
            throw new NotImplementedException();
        }

        public Target GetTargetDefinition( SetName setName ) {
            throw new NotImplementedException();
        }

        public ScoreFormatCollection GetScoreFormatCollection(SetName setName) {
            throw new NotImplementedException();
        }

        public ResultListFormat GetResultListFormatCollection( SetName setName ) {
            throw new NotImplementedException();
        }
    }
}
