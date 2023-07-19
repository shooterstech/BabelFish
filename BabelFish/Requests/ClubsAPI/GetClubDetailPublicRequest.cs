using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Requests.ClubsAPI {

    public class GetClubDetailPublicRequest : Request {


        public GetClubDetailPublicRequest( string ownerId) : base( "GetClubDetail" ) {
            this.SubDomain = APISubDomain.API;

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
