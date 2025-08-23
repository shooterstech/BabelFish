using Scopos.BabelFish.DataModel.Greengrass;
using Scopos.BabelFish.Requests.Athena;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Responses.Athena
{
	public class MoveGGv2CoreToAthenaDeploymentGroupPublicResponse: Response<MoveGGv2CoreToAthenaDeploymentGroupWrapper>
	{
		public MoveGGv2CoreToAthenaDeploymentGroupPublicResponse(MoveGGv2CoreToAthenaDeploymentGroupPublicRequest request): base()
		{
			Request = request;
		}
		public AthenaCoreDeployment AthenaCoreDeployment => Value.AthenaCoreDeployment;
	}
}
