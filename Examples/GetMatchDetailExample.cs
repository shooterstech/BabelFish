using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BabelFish.Requests;

namespace BabelFish.Examples {
    public class GetMatchDetailExample {

        public void Run() {

            //Create a client
            var client = new OrionMatchAPIClient( "myAPIKey" );

            //Generate a request to retreive a match
            GetMatchRequest matchRequest = new GetMatchRequest() { MatchID = "1.2332.2022012505350587.0" };

            //Call Get Match Detail, which returns a GetMatchResponse
            GetMatchResponse matchResponse = client.GetMatchDetail(matchRequest);

            //Get the returned Match object
            var match = matchResponse.Value; // or matchResponse.Match

            //Now inspect the returned value
            //Get the name
            var MatchName = match.Name;

            //Get a list of event names for results.
            List<string> events = new List<string>();
            foreach (var eventName in match.ResultEvents)
                events.Add(eventName.DisplayName);
        }
    }
}
