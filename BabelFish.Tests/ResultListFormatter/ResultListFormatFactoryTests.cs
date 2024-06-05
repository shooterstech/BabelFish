using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.APIClients;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Tests.ResultListFormatter {
    [TestClass]
    public class ResultListFormatFactoryTests {

        [TestInitialize]
        public void InitializeTest() {

            DefinitionFetcher.XApiKey = Constants.X_API_KEY;

        }

        [TestMethod]
        public async Task EriksPlayground() {
            var rlf = ResultListFormatFactory.FACTORY;

            var matchClient = new OrionMatchAPIClient( "wjM7eCb75aa3Okxj4FliXLY0VjHidoE2ei18pdg1", APIStage.PRODUCTION );

            var getResultListResponse = await matchClient.GetResultListPublicAsync( new MatchID( "1.2279.2024052710442834.0" ), "Individual - Sporter" );
            var resultList= getResultListResponse.ResultList;

            var rlfSetName = await rlf.GetResultListFormatSetNameAsync( resultList );
        }
    }
}
