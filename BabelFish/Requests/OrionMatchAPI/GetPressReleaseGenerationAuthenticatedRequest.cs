using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Runtime.Authentication;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QuestPDF.Helpers.Colors;
using ZXing.Aztec.Internal;

namespace Scopos.BabelFish.Requests.OrionMatchAPI
{
	public class GetPressReleaseGenerationAuthenticatedRequest: Request
	{

		public GetPressReleaseGenerationAuthenticatedRequest(UserAuthentication credentials) : base("PressReleaseGenerator_v2", credentials)
		{
		}

		public string GameID { get; set; }

		public string LeagueID { get; set; } = "";

		public bool Regenerate { get; set; } = false;

		/// <inheritdoc />
		public override string RelativePath
		{
			get { return $"/league/{LeagueID}/press-release"; }
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
