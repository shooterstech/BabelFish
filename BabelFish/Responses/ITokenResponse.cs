using Scopos.BabelFish.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Responses {

    interface ITokenResponse<T> where T : Request {

        /// <summary>
        /// When a respponse is tokenized, GetNextRequest prepares the next request object to use to return the next portion of the Items list.
        /// </summary>
        /// <returns></returns>
        T GetNextRequest();
    }
}
