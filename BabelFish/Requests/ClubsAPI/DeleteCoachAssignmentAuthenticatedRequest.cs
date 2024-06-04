using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.ClubsAPI
{
    public class DeleteCoachAssignmentAuthenticatedRequest : CoachAssignmentCRUDBaseRequest
    {
        public DeleteCoachAssignmentAuthenticatedRequest(UserAuthentication credentials) : base("CoachAssignmentD", credentials)
        {
            HttpMethod = HttpMethod.Delete;

        }
    }
}

