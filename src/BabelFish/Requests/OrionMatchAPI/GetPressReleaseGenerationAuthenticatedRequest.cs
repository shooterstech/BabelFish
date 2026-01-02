using Scopos.BabelFish.DataModel.OrionMatch;
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

		public MatchID ? GameID { get; set; }

		public MatchID ? LeagueID { get; set; } = null;

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

				if (GameID is not null)
				{
					parameterList.Add("game-id", new List<string> { GameID.ToString() });
				}

				parameterList.Add("regenerate", new List<string> { Regenerate.ToString() });



				return parameterList;
			}
		}


	}
}
