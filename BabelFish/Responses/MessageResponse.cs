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
    public class MessageResponse : IResponse, ICopy<MessageResponse> {

        /// <inheritdoc/>
        public List<string> Message { get; set; } = new List<string>();

        /// <inheritdoc/>
        public MessageResponse Copy() {
            MessageResponse copy = new MessageResponse();
            foreach( var item in Message) {
                copy.Message.Add(item);
            }

            return copy;
        }


        /// <inheritdoc/>
        public override string ToString() {
            return string.Join( ", ", Message );
        }
    }
}
