using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.Definitions;
using System.Runtime.Serialization;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.DefinitionAPI {

    /*
     * A GetDefinitionVersion request is nearly identical to a GetDefinition request. The 
     * difference is a QueryParameter (version=true) that is included, and the response
     * is also different (and much smaller). As only the SetName, Type, and Version are
     * returned
     */

    public class GetDefinitionVersionPublicRequest : Request {

        public GetDefinitionVersionPublicRequest( SetName setName, DefinitionType definitionType ) : base( "GetDefinitionVersion" ) {


            if (setName == null)
                throw new ArgumentNullException( nameof( setName ) );

            SetName = setName;
            DefinitionType = definitionType;
            IgnoreFileSystemCache = true;
            //NOTE: The InMemoryCache timeout is set to a relatively small 5 minutes.
            IgnoreInMemoryCache = false;
        }

        public SetName? SetName { get; set; } = null;

        public DefinitionType DefinitionType { get; set; } = DefinitionType.ATTRIBUTE;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/definition/{EnumHelper.GetAttributeOfType<EnumMemberAttribute>( DefinitionType ).Value}/{SetName}"; }
        }

        /// <inheritdoc />
        public override Dictionary<string, List<string>> QueryParameters {
            get {
                var queryParameters = base.QueryParameters;
                //version = true, is the query parameter that makes this a get version request.
                queryParameters.Add( "version", new List<string> { "true" } );
                //Choosing to always ask for the current value
                queryParameters.Add( "disable-cache", new List<string> { "true" } );

                return queryParameters;
            }
        }

        public override string ToString() {
            return $"{OperationId} request for {DefinitionType.Description()} {SetName}";
        }
    }
}
