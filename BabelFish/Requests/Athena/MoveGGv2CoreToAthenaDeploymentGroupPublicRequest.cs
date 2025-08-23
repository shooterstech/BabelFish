using Scopos.BabelFish.APIClients;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Requests.Athena
{
	public class MoveGGv2CoreToAthenaDeploymentGroupPublicRequest: Request
	{
		public MoveGGv2CoreToAthenaDeploymentGroupPublicRequest(): base("MoveGGv2CoreToAthenaDeploymentGroup"){
			SubDomain = APISubDomain.INTERNAL;
			HttpMethod = HttpMethod.Post;
		}

		public string OwnerID { get; set; } = "";

		public string CoreName { get; set; } = "";

		public string AthenaDeploymentName { get; set; } = "";

		public bool Redeploy { get; set; } = false;

		/// <inheritdoc />
		public override string RelativePath
		{
			get { return $"/orion/update-core"; }
		}

		public override Dictionary<string, List<string>> QueryParameters
		{
			get
			{

				Dictionary<string, List<string>> parameterList = new Dictionary<string, List<string>>();

				parameterList.Add("redeploy", new List<string>() { Redeploy.ToString() });

				parameterList.Add("athena-deployment-name", new List<string>() { AthenaDeploymentName.ToString() });

				if (!string.IsNullOrEmpty(this.OwnerID))
				{
					parameterList.Add("owner-id", new List<string>() { OwnerID.ToString() });
				}

				if (!string.IsNullOrEmpty(this.CoreName))
				{
					parameterList.Add("core-name", new List<string>() { CoreName.ToString() });
				}

				return parameterList;
			}
		}


	}
}
