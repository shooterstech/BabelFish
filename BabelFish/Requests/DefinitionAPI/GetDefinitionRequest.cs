using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootersTech.BabelFish.Helpers;
using ShootersTech.BabelFish.DataModel.Definitions;
using System.Runtime.Serialization;

namespace ShootersTech.BabelFish.Requests.DefinitionAPI
{
    public class GetDefinitionRequest : Request
    {
        public GetDefinitionRequest(SetName? setName = null, DefinitionType definitionType = DefinitionType.ATTRIBUTE )
        {
            SetName = setName;
            DefinitionType = definitionType;
        }

        public SetName? SetName { get; set; } = null;

        public DefinitionType DefinitionType { get; set; } = DefinitionType.ATTRIBUTE;

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/definition/{EnumHelper.GetAttributeOfType<EnumMemberAttribute>(DefinitionType).Value}/{SetName}"; }
        }
    }
}
