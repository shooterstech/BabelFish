using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.Athena;
using Scopos.BabelFish.Runtime.Authentication;


namespace Scopos.BabelFish.Tests.Athena
{


	[TestClass]
	public class GreengrassTests : BaseTestClass
	{
		[TestMethod]
		public async Task MoveGGv2CoreToAthenaDeploymentGroupTest()
		{
			var client = new AthenaAPIClient(APIStage.ALPHA);
			var request = new MoveGGv2CoreToAthenaDeploymentGroupPublicRequest();
			request.OwnerID = "OrionAcct000007"; 
			request.CoreName = "core-01"; //test core in us-east-2, may not exist in the future
			request.AthenaDeploymentName = "";
			request.Redeploy = false;

			var response = await client.MoveGGv2CoreToAthenaDeploymentGroupPublicAsync(request);
			Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
			Assert.AreEqual("AthenaRangeDeployment_1-0-0-0", response.AthenaCoreDeployment.AthenaCoreDeploymentName); //may be in different deployment group in the future
			
		}
	}
}
