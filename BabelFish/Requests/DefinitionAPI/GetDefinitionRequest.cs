using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.Definitions;
using System.Runtime.Serialization;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.DefinitionAPI {
    public class GetDefinitionRequest : Request {
        public GetDefinitionRequest( SetName setName, DefinitionType definitionType ) : base( "GetDefinition" ) {
            if (setName == null)
                throw new ArgumentNullException( nameof( setName ) );

            SetName = setName;
            DefinitionType = definitionType;
        }

        public GetDefinitionRequest( SetName setName, DefinitionType definitionType, UserAuthentication credentials ) : base( "GetDefinition", credentials ) {
            if (setName == null)
                throw new ArgumentNullException( nameof( setName ) );

            SetName = setName;
            DefinitionType = definitionType;
        }

        public SetName? SetName { get; set; } = null;

        public DefinitionType DefinitionType { get; set; } = DefinitionType.ATTRIBUTE;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/definition/{EnumHelper.GetAttributeOfType<EnumMemberAttribute>( DefinitionType ).Value}/{SetName}"; }
        }
    }
}
