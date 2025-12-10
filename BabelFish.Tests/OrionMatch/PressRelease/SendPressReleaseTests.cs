using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.Requests.OrionMatchAPI;
using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Tests.OrionMatch.PressRelease {
    [TestClass]
    public class SendPressReleaseTests : BaseTestClass {

        [TestMethod]
        public async Task SendPressReleaseEmailTest() {

            var userAuthentication = new UserAuthentication(
                Constants.TestDev7Credentials.Username,
                Constants.TestDev7Credentials.Password );
            await userAuthentication.InitializeAsync();

            PostSendPressReleaseEmailAuthenticatedRequest request = new PostSendPressReleaseEmailAuthenticatedRequest( userAuthentication ) {
                TestOnly = true,
                SendTo = new List<string>() { "erik@scopos.tech" },
                LeagueID = "1.1.2024072509092300.3",
                GameID = "1.1.2024092612083260.1"
            };

            var client = new OrionMatchAPIClient();

            var response = await client.PostSendPressReleaseEmailAsync( request );

            Assert.IsTrue( response.HasOkStatusCode );
            Console.WriteLine( response.MessageResponse );
        }
    }
}
