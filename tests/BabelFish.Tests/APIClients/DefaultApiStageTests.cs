using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Tests.APIClients {

    [TestClass]
    [DoNotParallelize]
    public class DefaultApiStageTests {
        private APIClientEnums _originalStage;
        private string _originalApiKey = string.Empty;

        [TestInitialize]
        public void Initialize() {
            _originalStage = Settings.DefaultApiStage;
            _originalApiKey = Settings.XApiKey;
            Settings.XApiKey = "stage-tests-no-network";
        }

        [TestCleanup]
        public void Cleanup() {
            Settings.DefaultApiStage = _originalStage;
            Settings.XApiKey = _originalApiKey;
        }

        [TestMethod]
        [DataRow(APIClientEnums.PRODUCTION)]
        [DataRow(APIClientEnums.PRODTEST)]
        [DataRow(APIClientEnums.ALPHA)]
        [DataRow(APIClientEnums.BETA)]
        public void ParameterlessClientsUseConfiguredDefault(APIClientEnums stage) {
            Settings.DefaultApiStage = stage;

            APIClientEnums[] stages = {
                new AthenaAPIClient().ApiStage,
                new AttributeValueAPIClient().ApiStage,
                new ClubsAPIClient().ApiStage,
                new DefinitionAPIClient().ApiStage,
                new ImageClient().ApiStage,
                new OrionMatchAPIClient().ApiStage,
                new ScoposDataClient().ApiStage,
                new ScoreHistoryAPIClient().ApiStage,
                new SocialNetworkAPIClient().ApiStage
            };

            foreach (var actual in stages) {
                Assert.AreEqual(stage, actual);
            }
        }

        [TestMethod]
        public void ExplicitStagesOverrideConfiguredDefault() {
            Settings.DefaultApiStage = APIClientEnums.PRODTEST;

            APIClientEnums[] stages = {
                new AthenaAPIClient(APIClientEnums.PRODUCTION).ApiStage,
                new AttributeValueAPIClient(APIClientEnums.PRODUCTION).ApiStage,
                new ClubsAPIClient(APIClientEnums.PRODUCTION).ApiStage,
                new DefinitionAPIClient(APIClientEnums.PRODUCTION).ApiStage,
                new ImageClient(APIClientEnums.PRODUCTION).ApiStage,
                new OrionMatchAPIClient(APIClientEnums.PRODUCTION).ApiStage,
                new ScoposDataClient(APIClientEnums.PRODUCTION).ApiStage,
                new ScoreHistoryAPIClient(APIClientEnums.PRODUCTION).ApiStage,
                new SocialNetworkAPIClient(APIClientEnums.PRODUCTION).ApiStage
            };

            foreach (var actual in stages) {
                Assert.AreEqual(APIClientEnums.PRODUCTION, actual);
            }
        }

        [TestMethod]
        public void ChangingDefaultOnlyAffectsNewClients() {
            Settings.DefaultApiStage = APIClientEnums.PRODUCTION;
            var existing = new ClubsAPIClient();

            Settings.DefaultApiStage = APIClientEnums.PRODTEST;

            Assert.AreEqual(APIClientEnums.PRODUCTION, existing.ApiStage);
            Assert.AreEqual(APIClientEnums.PRODTEST, new ClubsAPIClient().ApiStage);
        }
    }
}
