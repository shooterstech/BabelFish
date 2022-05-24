using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.Helpers;
using BabelFish.DataModel.Definitions;

namespace BabelFish.Requests.DefinitionAPI
{
    public class GetDefinitionRequest : Request
    {
        public GetDefinitionRequest(SetName? setName = null, string definitionType = "")
        {
            AttributeSetName = setName;
            AttributeDefinitionType = definitionType;
        }
        public SetName? AttributeSetName { get; set; } = null;

        public string AttributeDefinitionType { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/definition/{AttributeDefinitionType}/{AttributeSetName}"; }
        }
    }
}
