using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Henchmen.ResultListFormatter {

    /// <summary>
    /// MatchDataCache is a facade class for the OrionMatchAPIClient, that also adds in caching for the Orion Match
    /// Data. Orion Match Data is refreshed from the API based on the likelyhood the data needs to be refreshed.  
    /// 
    /// </summary>
    public class OrionMatchDataCache {

        private OrionMatchAPIClient orionMatchClient;
        private DefinitionAPIClient definitionClient;

        private Dictionary<MatchID, Match> CacheMatches = new Dictionary<MatchID, Match>();

        public OrionMatchDataCache( string xApiKey ) {
            this.XApiKey = xApiKey;

            this.orionMatchClient = new OrionMatchAPIClient( xApiKey );
            this.definitionClient = new DefinitionAPIClient( xApiKey );
        }

        public OrionMatchDataCache( string xApiKey, APIStage apiStage ) {
            this.XApiKey = xApiKey;

            this.orionMatchClient = new OrionMatchAPIClient( xApiKey, apiStage );
            this.definitionClient = new DefinitionAPIClient( xApiKey, apiStage );
        }

        public async Match GetMatchDetail( MatchID matchId ) {

        }

        public string XApiKey { get; private set; }
    }
}
