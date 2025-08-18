using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.Responses.OrionMatchAPI
{
	public  class GetPressReleaseGenerationAuthenticatedResponse: Response<PressReleaseGenerationResponseWrapper>
	{
		public GetPressReleaseGenerationAuthenticatedResponse(GetPressReleaseGenerationAuthenticatedRequest request)
		{
			this.Request = request;	
		}

		public PressReleaseGeneration PressReleaseGeneration => Value.PressReleaseGeneration;
	}
}
