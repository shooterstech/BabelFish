using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.Athena;
using Scopos.BabelFish.Requests.ScoreHistoryAPI;
using Scopos.BabelFish.Responses.ScoreHistoryAPI;
using Scopos.BabelFish.DataModel.ScoreHistory;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.ScoreHistory {
	[TestClass]
	public class AuthenticatedScoreHistoryTests {

		[TestMethod]
		public async Task GetEventStyleScoreHistory() {

			var scoreHistoryClient = new ScoreHistoryAPIClient( Constants.X_API_KEY, APIStage.BETA );

			var userAuthentication = new UserAuthentication(
				Constants.TestDev7Credentials.Username,
				Constants.TestDev7Credentials.Password );
			await userAuthentication.InitializeAsync();

			var scoreHistoryRequest = new GetScoreHistoryAuthenticatedRequest(userAuthentication);
			scoreHistoryRequest.StartDate = new DateTime( 2023, 04, 15 );
			scoreHistoryRequest.EndDate = new DateTime( 2023, 04, 22 );
			//scoreHistoryRequest.UserIds = new List<string>() { Constants.TestDev7UserId };
			
			//var eventStyleDef = "v1.0:ntparc:Three-Position Sporter Air Rifle";
			//scoreHistoryRequest.EventStyleDef = SetName.Parse( eventStyleDef );

			var scoreHistoryResponse = await scoreHistoryClient.GetScoreHistoryAuthenticatedAsync( scoreHistoryRequest );

			Assert.AreEqual( System.Net.HttpStatusCode.OK, scoreHistoryResponse.StatusCode );

			bool hasAtLeastOneEventStyleEntry = false;

			foreach (var scoreHistoryBase in scoreHistoryResponse.ScoreHistory.Items) {
				Assert.IsTrue( scoreHistoryBase.NumberOfShots > 0 );
				if (scoreHistoryBase is ScoreHistoryEventStyleEntry) {
					hasAtLeastOneEventStyleEntry |= true;
					var scoreHistoryEventStyle = (ScoreHistoryEventStyleEntry)scoreHistoryBase;
				}
			}

			Assert.IsTrue( hasAtLeastOneEventStyleEntry );
		}
	}
}
