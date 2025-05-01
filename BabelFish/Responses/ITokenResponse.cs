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

        /// <summary>
        /// Returns a boolean indicating if the server has more items to respond with. If so, the user may call GetNextRequest to retreive those items.
        /// </summary>
        bool HasMoreItems { get; }
    }
}
