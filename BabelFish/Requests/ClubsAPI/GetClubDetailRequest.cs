using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.BabelFish.Requests.ClubsAPI {

    public class GetClubDetailRequest : Request {

        public GetClubDetailRequest( string ownerId ) {

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
