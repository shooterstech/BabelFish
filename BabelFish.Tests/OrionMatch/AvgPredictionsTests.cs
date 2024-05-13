using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataActors.OrionMatch;
using Amazon.DynamoDBv2.Model;


namespace Scopos.BabelFish.Tests.OrionMatch
{

    [TestClass]
    public class AvgPredictionsTests
    {

        // might should just use an API call, might be easier....
        [TestMethod]
        public async Task ProjectedAvgScoresMakerTests()
        {
            var client = new OrionMatchAPIClient(Constants.X_API_KEY, APIStage.BETA);

            string resultCofId = "4608b306-8b6d-40c2-b608-e5375d05bd12";

            var response = await client.GetResultCourseOfFireDetailPublicAsync(resultCofId);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            //This result cof should of 30 shots, and 8 event scores
            ResultCOF resultCof = response.ResultCOF;

            object test = ProjectScoresByAverageShotFired();
        }
    }
}
