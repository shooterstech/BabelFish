using BabelFish.DataModel;
using Scopos.BabelFish.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Responses {

    /// <summary>
    /// The Message Response contains all of the standard fields returned in a Scopos Rest API call, including Message and NextToken (if used). What it doesn't contain is the requested data model object.
    /// </summary>
    public class MessageResponse : IResponse {

        /// <inheritdoc/>
        public List<string> Message { get; set; } = new List<string>();


        /// <inheritdoc/>
        public override string ToString() {
            return string.Join( ", ", Message );
        }
    }
}
