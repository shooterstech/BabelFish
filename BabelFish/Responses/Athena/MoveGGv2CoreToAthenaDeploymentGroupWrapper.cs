using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.Greengrass;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Responses.Athena
{
	public class MoveGGv2CoreToAthenaDeploymentGroupWrapper: BaseClass
	{
		public AthenaCoreDeployment AthenaCoreDeployment { get; set; }
	}
}
