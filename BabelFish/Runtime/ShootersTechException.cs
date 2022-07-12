using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.BabelFish.Runtime {
    public class ShootersTechException : Exception {

        public ShootersTechException()
            : base( "Something bad happened!" ) {
        }
        public ShootersTechException( string message )
            : base( message ) {
        }
        public ShootersTechException( string message, Exception inner )
            : base( message, inner ) {
        }
    }
}
