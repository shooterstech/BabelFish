using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Common;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;


namespace Scopos.BabelFish.Tests.OrionMatch
{
	[TestClass]
	public class LeagueAuthenticatedTests: BaseTestClass
	{
		[TestMethod]
		public async Task TestPressReleaseGeneration()
		{

			var client = new OrionMatchAPIClient(APIStage.PRODTEST);
			var userAuthentication = new UserAuthentication(
				Constants.TestDev7Credentials.Username,
				Constants.TestDev7Credentials.Password);
			await userAuthentication.InitializeAsync();

			var gameId = "1.1.2024092612083256.1";
			var leagueId = "1.1.2024072509092300.3";
			var request = new GetPressReleaseGenerationAuthenticatedRequest(userAuthentication);
			request.GameID = gameId;
			request.LeagueID = leagueId;
			request.Regenerate = true;

			var response = await client.GetPressReleaseGenerationAuthenticatedAsync(request);

			Assert.AreEqual(HttpStatusCode.OK, response.RestApiStatusCode);
			Assert.AreEqual(response.PressReleaseGeneration.S3Url, "https://s3.us-east-1.amazonaws.com/cdn.scopos.tech/matches/1.1.2024092612083256.1/pressrelease.html");
			Assert.IsTrue(response.PressReleaseGeneration.Regenerated);

		}
	}
}
