using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootersTech.BabelFish.DataModel.OrionMatch;
using ShootersTech.BabelFish.Requests.OrionMatchAPI;

namespace ShootersTech.BabelFish.Responses.OrionMatchAPI
{
    public class GetSquaddingListResponse : Response<Squadding>
    {

        public GetSquaddingListResponse( GetSquaddingListRequest request ) : base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        /// 
        public Squadding Squadding {
            get { return Value; }
        }
    }
}
