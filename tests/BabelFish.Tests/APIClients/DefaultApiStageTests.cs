using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Tests.APIClients {

    [TestClass]
    [DoNotParallelize]
    public class DefaultApiStageTests {
        private APIStage _originalStage;
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
        [DataRow(APIStage.PRODUCTION)]
        [DataRow(APIStage.PRODTEST)]
        [DataRow(APIStage.ALPHA)]
        [DataRow(APIStage.BETA)]
        public void ParameterlessClientsUseConfiguredDefault(APIStage stage) {
            Settings.DefaultApiStage = stage;

            APIStage[] stages = {
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
            Settings.DefaultApiStage = APIStage.PRODTEST;

            APIStage[] stages = {
                new AthenaAPIClient(APIStage.PRODUCTION).ApiStage,
                new AttributeValueAPIClient(APIStage.PRODUCTION).ApiStage,
                new ClubsAPIClient(APIStage.PRODUCTION).ApiStage,
                new DefinitionAPIClient(APIStage.PRODUCTION).ApiStage,
                new ImageClient(APIStage.PRODUCTION).ApiStage,
                new OrionMatchAPIClient(APIStage.PRODUCTION).ApiStage,
                new ScoposDataClient(APIStage.PRODUCTION).ApiStage,
                new ScoreHistoryAPIClient(APIStage.PRODUCTION).ApiStage,
                new SocialNetworkAPIClient(APIStage.PRODUCTION).ApiStage
            };

            foreach (var actual in stages) {
                Assert.AreEqual(APIStage.PRODUCTION, actual);
            }
        }

        [TestMethod]
        public void ChangingDefaultOnlyAffectsNewClients() {
            Settings.DefaultApiStage = APIStage.PRODUCTION;
            var existing = new ClubsAPIClient();

            Settings.DefaultApiStage = APIStage.PRODTEST;

            Assert.AreEqual(APIStage.PRODUCTION, existing.ApiStage);
            Assert.AreEqual(APIStage.PRODTEST, new ClubsAPIClient().ApiStage);
        }
    }
}
