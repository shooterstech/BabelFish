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

        public Definition GetDefinition(Definition.DefinitionType type, SetName setName)
        {
            throw new NotImplementedException();
        }

        //public async Task<T> GetDefinition<T>(Definition.DefinitionType type, SetName setName, Response<T> response)
        //{
        //    //throw new NotImplementedException();
        //    GetDefinitionRequest requestParameters = new GetDefinitionRequest(setName, EnumHelper.GetAttributeOfType<EnumMemberAttribute>(type).Value);

        //    await this.CallAPI(requestParameters, response).ConfigureAwait(false);

        //    return response;
        //}

        public async Task<GetAttributeResponse> GetAttributeDefinitionAsync(string version, string nameSpace, string properName)
        {
            //orig - return await (BabelFish.DataModel.Definitions.Attribute)GetDefinition( Definition.DefinitionType.ATTRIBUTE, setName ).ConfigureAwait(false);
            //TODO: Get this working then fix error with Generics in GetDefinition()

            GetAttributeResponse response = new GetAttributeResponse();

            SetName setName = new SetName(nameSpace,properName,version);
//            return await GetDefinition(Definition.DefinitionType.ATTRIBUTE, setName, response).ConfigureAwait(false);
            
            GetDefinitionRequest requestParameters = new GetDefinitionRequest(setName, EnumHelper.GetAttributeOfType<EnumMemberAttribute>(Definition.DefinitionType.ATTRIBUTE).Value);
            await this.CallAPI(requestParameters, response).ConfigureAwait(false);
            return response;
        }

        public CourseOfFire GetCourseOfFireDefinition( SetName setName ) {
            return (CourseOfFire)GetDefinition( Definition.DefinitionType.COURSEOFFIRE, setName );
        }

        public EventStyle GetEventStyleDefinition( SetName setName ) {
            return (EventStyle)GetDefinition( Definition.DefinitionType.EVENTSTYLE, setName );
        }

        public RankingRule GetRankingRuleDefinition( SetName setName ) {
            return (RankingRule)GetDefinition( Definition.DefinitionType.RANKINGRULES, setName );
        }

        public StageStyle GetStageStyleDefinition( SetName setName ) {
            return (StageStyle)GetDefinition( Definition.DefinitionType.STAGESTYLE, setName );
        }

        public TargetCollection GetTargetCollectionDefinition( SetName setName ) {
            return (TargetCollection)GetDefinition( Definition.DefinitionType.TARGETCOLLECTION, setName );
        }

        public Target GetTargetDefinition( SetName setName ) {
            return (Target)GetDefinition( Definition.DefinitionType.TARGET, setName );
        }
    }
}
