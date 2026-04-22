namespace Scopos.BabelFish.Runtime {

    /// <summary>
    /// ScoposException is the base exception for all exceptions thrown by the Scopos BabelFish library.
    /// </summary>
    public class ScoposException : Exception {

        /// <summary>
        /// General purpose logger, which is different from Loggers that may hae been passed on.
        /// </summary>
        protected static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Initializes a new instance of the ScoposException class with a default error message, "Something bad happened!",
        /// And writes the same message to the log as an error.
        /// </summary>
        public ScoposException()
            : base( "Something bad happened!" ) {
            _logger.Error( "Something bad happened!" );
        }


        /// <summary>
        /// Initializes a new instance of the ScoposException class with a default error message, "Something bad happened!",
        /// And writes the same message to the passed in logger as an error.
        /// </summary>
        public ScoposException( Logger logger )
            : base( "Something bad happened!" ) {
            logger.Error( "Something bad happened!" );
        }

        /// <summary>
        /// Initializes a new instance of the ScoposException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ScoposException( string message )
            : base( message ) {
            _logger.Error( message );
        }

        /// <summary>
        /// Initializes a new instance of the ScoposException class with a specified error message and logs the error
        /// using the provided logger.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="logger">The logger used to record the error message.</param>
        public ScoposException( string message, Logger logger )
            : base( message ) {
            logger.Error( this, message );
        }

        /// <summary>
        /// Initializes a new instance of the ScoposException class with a specified error message and a reference to
        /// the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is
        /// specified.</param>
        public ScoposException( string message, Exception inner )
            : base( message, inner ) {
            _logger.Error( "Something bad happened!" );
        }

        /// <summary>
        /// Initializes a new instance of the ScoposException class with a specified error message, a reference to the
        /// inner exception that is the cause of this exception, and a logger for error reporting.
        /// </summary>
        /// <remarks>This constructor logs the error message using the provided logger when the exception
        /// is created.</remarks>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is
        /// specified.</param>
        /// <param name="logger">The logger used to record the error information associated with this exception.</param>
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

        public InitializeAsyncNotCompletedException( Logger logger )
            : base( logger ) {
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
