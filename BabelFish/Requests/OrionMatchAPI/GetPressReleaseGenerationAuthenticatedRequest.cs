using Scopos.BabelFish.Runtime.Authentication;

namespace Scopos.BabelFish.Requests.OrionMatchAPI
{
	public class GetPressReleaseGenerationAuthenticatedRequest: Request
	{

		public GetPressReleaseGenerationAuthenticatedRequest(UserAuthentication credentials) : base("PressReleaseGenerator_v2", credentials)
		{
			//Press release generator is known to take a long time. Increasing the timeout to 3 minutes. 
			Timeout = 3 * 60;
		}

		public string GameID { get; set; }

		public string LeagueID { get; set; } = "";

		public bool Regenerate { get; set; } = false;

		/// <inheritdoc />
		public override string RelativePath
		{
			get { 
				return $"/league/{LeagueID}/press-release"; 
			}
		}

		public override Dictionary<string, List<string>> QueryParameters
		{
			get
			{

				Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

				if (!string.IsNullOrEmpty(GameID))
				{
					parameterList.Add("game-id", new List<string> { GameID });
				}

				parameterList.Add("regenerate", new List<string> { Regenerate.ToString() });



				return parameterList;
			}
		}


	}
}
