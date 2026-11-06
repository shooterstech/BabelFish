using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Greengrass
{
	public class AthenaCoreDeployment
	{
		public string AthenaCoreDeploymentName { get; set; }

		public string DeploymentId { get; set; }

		/// <summary>
		/// Gets or sets whether another update is required after this deployment succeeds.
		/// </summary>
		public bool FurtherUpdatesRequired { get; set; } = false;

	}
}
