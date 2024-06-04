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
    public class GetDefinitionListPublicRequest : Request, ITokenRequest {

        public GetDefinitionListPublicRequest( DefinitionType definitionType ) : base( "GetDefinitionList" ) {

            DefinitionType = definitionType;
            IgnoreFileSystemCache = true;
            IgnoreInMemoryCache = false;
        }

        public DefinitionType DefinitionType { get; set; } = DefinitionType.ATTRIBUTE;

        /// <inheritdoc />
        public override string RelativePath {
            get { return $"/definition/{EnumHelper.GetAttributeOfType<EnumMemberAttribute>( DefinitionType ).Value}"; }
		}

		/// <inheritdoc />
		public override Dictionary<string, List<string>> QueryParameters {
			get {

				Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

				if (!string.IsNullOrEmpty( Token )) {
					parameterList.Add( "token", new List<string> { Token } );
				}

				if (Limit > 0)
					parameterList.Add( "limit", new List<string> { Limit.ToString() } );

				if (!string.IsNullOrEmpty( Search ))
					parameterList.Add( "search", new List<string> { Search } );


				return parameterList;
			}
		}

		/// <inheritdoc />
		public string Token { get; set; }

		/// <inheritdoc />
		public int Limit { get; set; } = 0;

        /// <summary>
        /// When Search term is includeed, the REST API returns a definition list in order of relavance matching 
        /// of the Search value, and will include a SearchScore for each Definition.
        /// </summary>
        public string Search { get; set; } = string.Empty;

		/// <inheritdoc />
		public override Request Copy() {
			var newRequest = new GetDefinitionListPublicRequest( this.DefinitionType );
			newRequest.Token = this.Token;

			return newRequest;
		}

		public override string ToString() {
            return $"{OperationId} request for {DefinitionType.Description()}";
        }
    }
}
