using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Scopos.BabelFish.Requests.ClubsAPI
{
    public class CoachAssignmentCRUDBaseRequest : Request
    {
        public CoachAssignmentCRUDBaseRequest(string operationId, UserAuthentication credentials) : base(operationId, credentials)
        {
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;
        }

        public int LicenseNumber { get; set; }

        public List<string> UserId { get; set; } = new List<string>();

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/clubs/coach"; }
        }

        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                parameterList.Add("license-number", new List<string>() { LicenseNumber.ToString() });
                
                if (UserId.Count > 0)
                    parameterList.Add("user-id", UserId);

                return parameterList;
            }
        }

    }
}
