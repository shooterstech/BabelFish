using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace Scopos.BabelFish.Runtime {
    public class ScoposException : Exception {

        public ScoposException()
            : base( "Something bad happened!" ) {
        }
        public ScoposException( string message )
            : base( message ) {
        }
        public ScoposException( string message, Logger logger) 
            :base( message ) {
            logger.Error( this, message );
        }
        public ScoposException( string message, Exception inner )
            : base( message, inner ) {
        }
        public ScoposException( string message, Exception inner, Logger logger )
            : base( message, inner ) {
            logger.Error( this, message );
        }
    }

    /// <summary>
    /// Thrown when a Constructor requires Async method calls, that are completed in a InitializeAsync() method (can't be 
    /// called in the construcotr b/c constructors can not be marked async), but the user doesn't not call InitializeAsync(). 
    /// </summary>
    public class InitializeAsyncNotCompletedException : ScoposException {
        public InitializeAsyncNotCompletedException()
            : base( "InitializeAsync() was not called after the constructor. Can not proceed until after this call was successful." ) {
        }
        public InitializeAsyncNotCompletedException( string message )
            : base( message ) {
        }

        public InitializeAsyncNotCompletedException( string message, Logger logger )
            : base( message, logger ) {
        }

        public InitializeAsyncNotCompletedException( string message, Exception inner )
            : base( message, inner ) {
        }

        public InitializeAsyncNotCompletedException( string message, Exception inner, Logger logger )
            : base( message, inner, logger ) {
        }
    }
}
