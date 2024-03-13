using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel {

    /// <summary>
    /// An ITokenItems interface is for DataModels that are returned as a Response objects primary data model, and then follow
    /// the standard NextToken / Token programming model. 
    /// 
    /// The request needs to have 'token' as an optional query string parameter.
    /// 
    /// The top level response object may return NextToken, to indicate more data is avaliable.
    /// The response object's primary data model must also include a List<>. It is this list that could be combined accross multiple calls. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITokenItems<T> {

        List<T> Items { get; set; }

        /// <summary>
        /// When there are more items to return, NextToken contains the next token to pass to the call to the next set of items.
        /// A value of null or empoty string indicate there are no more items to return.
        /// </summary>
        string NextToken { get; set; }

        /*
         * EKA 3 / 2024: Not sure if Limit is every included in a response object. Yes, it is used in the request object, but if it is part 
         * of the response, it would just be echoing back what was sent.
         * 
         * Stated antoher way, we might be able to remove Limit
         */

        /// <summary>
        /// The maximum number of items that may be returned. 
        /// The user may request a limit, usually between 1 and 50, but the API is allowed to restrict the value of limit. This is the value the API used.
        /// </summary>
        int Limit { get; set; }

        /// <summary>
        /// Indicates if this response has more items. Almost always determiend if NextToken is null or an empty string.
        /// </summary>
        bool HasMoreItems { get; }
    }
}
