using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Runtime;

namespace Scopos.BabelFish.Requests {
    public class RequestException : ShootersTechException {
        public RequestException()
            : base( "Something bad happened!" ) {
        }
        public RequestException( string message )
            : base( message ) {
        }
        public RequestException( string message, Exception inner )
            : base( message, inner ) {
        }
    }
}
