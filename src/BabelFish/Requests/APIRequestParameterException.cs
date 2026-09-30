namespace Scopos.BabelFish.Requests {

    /// <summary>
    /// Represents an exception that is thrown when there is an issue with the parameters
    /// provided in a concrete <see cref="Request"/> instance.
    /// This exception can be used to indicate that the request parameters are invalid, missing, or do not meet the expected criteria.
    /// </summary>
    public class APIRequestParameterException : ScoposException {
        /// <summary>
        /// Initializes a new instance of the <see cref="APIRequestParameterException"/> class with a default error message.
        /// </summary>
        public APIRequestParameterException()
            : base( "Something bad happened!" ) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="APIRequestParameterException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public APIRequestParameterException( string message )
            : base( message ) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="APIRequestParameterException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The inner exception that is the cause of this exception.</param>
        public APIRequestParameterException( string message, Exception inner )
            : base( message, inner ) {
        }
    }
}
