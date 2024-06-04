using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.ClubsAPI
{
    public class GetCoachAssignmentAuthenticatedRequest: CoachAssignmentCRUDBaseRequest
    {
        public GetCoachAssignmentAuthenticatedRequest(UserAuthentication credentials) : base("CoachAssignmentR", credentials)
        {
            HttpMethod = HttpMethod.Get;

        }

        public GetCoachAssignmentAuthenticatedRequest(int licenseNumber, UserAuthentication credentials) : base("CoachAssignmentR", credentials)
        {
            HttpMethod = HttpMethod.Get;
            LicenseNumber = licenseNumber;

        }
    }
}
