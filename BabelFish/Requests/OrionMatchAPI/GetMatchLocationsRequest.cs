using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.Requests.OrionMatchAPI 
{
    public class GetMatchLocationsRequest : Request
    {
        public GetMatchLocationsRequest() { }

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/match/locations"; }
        }
    }
}
