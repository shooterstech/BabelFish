using Scopos.BabelFish.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Responses {

    interface ITokenResponse<T> where T : Request {

        /// <summary>
        /// GetNextRequest() prepares the next request object to use to return the next portion of the Items list.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NoMoreItemsException">Thrown when the user calls .GetNextRequest() but .HasMoreItems is false. Always
        /// check .HasMoreItems before calling .GetNextRequest().</exception>
        T GetNextRequest();

        /// <summary>
        /// Returns a boolean indicating if the server has more items to respond with. If so, the user may call GetNextRequest() to retreive those items.
        /// <para>Will also return false if .HasOkStatusCode is false. </para>
        /// </summary>
        bool HasMoreItems { get; }
    }
}
