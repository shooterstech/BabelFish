using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.ClubsAPI
{
    public class CreateCoachAssignmentAuthenticatedRequest : CoachAssignmentCRUDBaseRequest
    {
        public CreateCoachAssignmentAuthenticatedRequest(UserAuthentication credentials) : base("CoachAssignmentC", credentials)
        {
            HttpMethod = HttpMethod.Post;

        }
    }
}
