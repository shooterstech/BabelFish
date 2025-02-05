﻿using Scopos.BabelFish.Runtime.Authentication;
using Scopos.BabelFish.APIClients;


namespace Scopos.BabelFish.Requests.ScoreHistoryAPI
{
    public class DeleteScoreHistoryRequest : Request
    {

        /// <inheritdoc />
        public DeleteScoreHistoryRequest(UserAuthentication credentials) : base("DeleteUserScore", credentials)
        {
            HttpMethod = HttpMethod.Delete;
            this.RequiresCredentials = true;
            this.SubDomain = APISubDomain.AUTHAPI;
        }

        public string ResultCOFID { get; set; } = "";

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/athlete/score/history"; }
        }

        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {
                if (ResultCOFID == "")
                    throw new RequestException("ResultCOFID is required and must not be empty");

                Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();
                parameterList.Add("result-cof-id", new List<string>() { ResultCOFID });
                

                return parameterList;
            }
        }



    }
}
