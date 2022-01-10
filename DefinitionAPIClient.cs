using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.DataModel.Definitions;

namespace BabelFish {
    public class DefinitionAPIClient : APIClient {

        public DefinitionAPIClient(string apiKey) : base(apiKey) { }

        public Definition GetDefinition( Definition.DefinitionType type, SetName setName) {
            throw new NotImplementedException();
        }

        public BabelFish.DataModel.Definitions.Attribute GetAttributeDefinition( SetName setName) {
            return (BabelFish.DataModel.Definitions.Attribute)GetDefinition( Definition.DefinitionType.ATTRIBUTE, setName );
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
