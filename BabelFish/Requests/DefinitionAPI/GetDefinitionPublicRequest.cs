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
    public class GetDefinitionPublicRequest : Request {
        public GetDefinitionPublicRequest( SetName setName, DefinitionType definitionType ) : base( "GetDefinition" ) {
            if (setName == null)
                throw new ArgumentNullException( nameof( setName ) );

            SetName = setName;
            DefinitionType = definitionType;
        }

        public GetDefinitionPublicRequest( SetName setName, DefinitionType definitionType, UserAuthentication credentials ) : base( "GetDefinition", credentials ) {
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

        /// <summary>
        /// Indicates if the local cache should be ignored and always pull the definition from the Rest API.
        /// The default value is false, meaning to use the local cache.
        /// The option to ignore local cache can either be wet at the API Client level, or on a per request level.
        /// </summary>
        public bool IgnoreLocalCache { get; set; } = false;
    }
}
