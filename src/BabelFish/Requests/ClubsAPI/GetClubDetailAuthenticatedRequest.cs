using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Requests.ClubsAPI {

    public class GetClubDetailAuthenticatedRequest : Request {


        public GetClubDetailAuthenticatedRequest( string ownerId, UserAuthentication credentials ) : base( "GetClubDetail", credentials ) {
            //NOTE: Because this request requires user credentials, we're only writing one constructor that includes parameters for crendentials.
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;

            if (string.IsNullOrEmpty( ownerId ))
                throw new ArgumentNullException( "The parameter ownerId may not be null or an empty string." );

            OwnerId = ownerId;
        }

        /// <summary>
        /// The Orion Account Owner Id to look up. 
        /// </summary>
        /// <example>OrionAcct001234</example>
        public string OwnerId { get; set; }


        /// <inheritdoc />
        public override string RelativePath {

            get { return $"/clubs/{OwnerId}"; }
        }
    }
}
