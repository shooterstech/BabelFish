using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.DataModel.OrionMatch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BabelFish.Responses.OrionMatchAPI
{
    public class GetSquaddingListResponse : Response<Squadding>
    {
        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        /// 
        public Squadding Squadding {
            get { return Value; }
        }
    }
}
