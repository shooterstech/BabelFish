using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.Requests.ClubsAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Responses.ClubsAPI
{
    public class CoachAssignmentCRUDAuthenticatedResponse: Response<CoachAssignmentWrapper>
    {
        public CoachAssignmentCRUDAuthenticatedResponse(CoachAssignmentCRUDBaseRequest request) : base()
        {
            Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public CoachAssignmentList CoachAssignmentList
        {
            get { return Value.CoachAssignmentList; }
        }
    }
}
