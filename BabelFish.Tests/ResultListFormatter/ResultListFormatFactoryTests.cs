using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataActors.ResultListFormatter;
using Scopos.BabelFish.APIClients;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataActors.ResultListFormatter.UserProfile;

namespace Scopos.BabelFish.Tests.ResultListFormatter {
    [TestClass]
    public class ResultListFormatFactoryTests : BaseTestClass {

        private OrionMatchAPIClient matchClient;
        private DefinitionAPIClient definitionClient;
        private IUserProfileLookup userProfileLookup;

        [TestInitialize]
        public override void InitializeTest() {
            base.InitializeTest();

            matchClient = new OrionMatchAPIClient( );
            definitionClient = new DefinitionAPIClient();

            userProfileLookup = new BaseUserProfileLookup();

        }

        [TestMethod]
        [Ignore]
        public async Task EriksPlayground() {
            var rlf = ResultListFormatFactory.FACTORY;

            var matchClient = new OrionMatchAPIClient( APIStage.PRODUCTION );

            var getResultListResponse = await matchClient.GetResultListPublicAsync( new MatchID( "1.3313.2024060212062194.0" ), "Individual - All" );
            var resultList= getResultListResponse.ResultList;

            var rlfSetName = await rlf.GetResultListFormatSetNameAsync( resultList );
            var rlfDefinition = await DefinitionCache.GetResultListFormatDefinitionAsync( rlfSetName );

            ResultListIntermediateFormatted rlif = new ResultListIntermediateFormatted( resultList, rlfDefinition, userProfileLookup );
            await rlif.InitializeAsync();
        }
    }
}
