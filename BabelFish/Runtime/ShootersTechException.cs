using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace Scopos.BabelFish.Runtime {
    public class ShootersTechException : Exception {

        public ShootersTechException()
            : base( "Something bad happened!" ) {
        }
        public ShootersTechException( string message )
            : base( message ) {
        }
        public ShootersTechException( string message, Logger logger) 
            :base( message ) {
            logger.Error( this, message );
        }
        public ShootersTechException( string message, Exception inner )
            : base( message, inner ) {
        }
        public ShootersTechException( string message, Exception inner, Logger logger )
            : base( message, inner ) {
            logger.Error( this, message );
        }
    }
}
