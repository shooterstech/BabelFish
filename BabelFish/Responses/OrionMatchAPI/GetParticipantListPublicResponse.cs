using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
    public class GetParticipantListPublicResponse : Response<MatchParticipantList>
    {

        public GetParticipantListPublicResponse( GetParticipantListPublicRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public List<MatchParticipant> ParticipantList
        {
            get { return Value.ParticipantList; }
        }
    }
}