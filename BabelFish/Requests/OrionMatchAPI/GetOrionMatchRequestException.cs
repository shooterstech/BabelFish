using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Requests;

namespace Scopos.BabelFish.Requests.OrionMatchAPI {
    public class GetOrionMatchRequestException : RequestException {
        public GetOrionMatchRequestException()
            : base("Something bad happened!") {
        }
        public GetOrionMatchRequestException(string message)
            : base(message) {
        }
        public GetOrionMatchRequestException(string message, Exception inner)
            : base(message, inner) {
        }
    }
}
