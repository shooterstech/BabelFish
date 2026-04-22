namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// An OrionMatchException will be thrown when the user tries to perform an operation
    /// that is prohibited by the rules of the OrionMatch data model. For example,
    /// if a user tries to add a Participant to a Team that already has the maximum number of Participants allowed.
    /// an OrionMatchException will be thrown.
    /// <para>OrionMatchException is a base exception for more specific OrionMatch-related exceptions.</para>
    /// </summary>
    public class OrionMatchException : ScoposException {

        private const string DEFAULT_MESSAGE = "User tried to perform an operation that is prohibited by the rules of the OrionMatch data model.";

        /// <summary>
        /// Initializes a new instance of the OrionMatchException class with a default error message.
        /// </summary>
        /// <remarks>This constructor logs the default error message when the exception is
        /// created.</remarks>
        public OrionMatchException()
            : base( DEFAULT_MESSAGE ) {
            _logger.Error( DEFAULT_MESSAGE );
        }

        /// <summary>
        /// Initializes a new instance of the OrionMatchException class with a default error message and logs the error
        /// using the specified logger.
        /// </summary>
        /// <remarks>This constructor logs the default error message to the provided logger when the
        /// exception is instantiated. Use this overload to ensure that error details are captured in application
        /// logs.</remarks>
        /// <param name="logger">The logger used to record the error message when the exception is created.</param>
        public OrionMatchException( Logger logger )
            : base( DEFAULT_MESSAGE ) {
            logger.Error( DEFAULT_MESSAGE );
        }

        /// <summary>
        /// Initializes a new instance of the OrionMatchException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public OrionMatchException( string message )
            : base( message ) {
            _logger.Error( message );
        }

        /// <summary>
        /// Initializes a new instance of the OrionMatchException class with a specified error message and logs the
        /// error using the provided logger.
        /// </summary>
        /// <remarks>This constructor logs the exception message using the specified logger when the
        /// exception is created.</remarks>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="logger">The logger used to record the error message.</param>
        public OrionMatchException( string message, Logger logger )
            : base( message ) {
            logger.Error( this, message );
        }

        /// <summary>
        /// Initializes a new instance of the OrionMatchException class with a specified error message and a reference
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is
        /// specified.</param>
        public OrionMatchException( string message, Exception inner )
            : base( message, inner ) {
            _logger.Error( DEFAULT_MESSAGE );
        }

        /// <summary>
        /// Initializes a new instance of the OrionMatchException class with a specified error message, a reference to
        /// the inner exception that is the cause of this exception, and a logger for error reporting.
        /// </summary>
        /// <remarks>This constructor logs the exception message using the provided logger when the
        /// exception is created.</remarks>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is
        /// specified.</param>
        /// <param name="logger">The logger used to record the error information.</param>
        public OrionMatchException( string message, Exception inner, Logger logger )
            : base( message, inner ) {
            logger.Error( this, message );
        }
    }

    /// <summary>
    /// Thrown when the user tries to add a <see cref="Participant"/> to a <see cref="CourseOfFireEntryTeam">
    /// Team</see>
    /// that already has the maximum number of participants allowed by the CourseOfFireStructure.
    /// </summary>
    public class TeamFullException : OrionMatchException {
        private const string DEFAULT_MESSAGE = "The team already has the maximum number of participants allowed by the CourseOfFireStructure, and cannot accept any more participants.";

        /// <summary>
        /// Initializes a new instance of the TeamFullException class with a default error message indicating that the
        /// team is full.
        /// </summary>
        public TeamFullException()
            : base( DEFAULT_MESSAGE ) {
            _logger.Error( DEFAULT_MESSAGE );
        }

        /// <summary>
        /// Initializes a new instance of the TeamFullException class with a specified logger and a default error
        /// message.
        /// </summary>
        /// <remarks>This constructor logs the default error message using the provided logger when the
        /// exception is instantiated.</remarks>
        /// <param name="logger">The logger used to record the error message when the exception is created.</param>
        public TeamFullException( Logger logger )
            : base( DEFAULT_MESSAGE ) {
            logger.Error( DEFAULT_MESSAGE );
        }

        /// <summary>
        /// Initializes a new instance of the TeamFullException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public TeamFullException( string message )
            : base( message ) {
            _logger.Error( message );
        }

        /// <summary>
        /// Initializes a new instance of the TeamFullException class with a specified error message and logs the error
        /// using the provided logger.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="logger">The logger used to record the error message.</param>
        public TeamFullException( string message, Logger logger )
            : base( message ) {
            logger.Error( this, message );
        }

        /// <summary>
        /// Initializes a new instance of the TeamFullException class with a specified error message and a reference to
        /// the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is
        /// specified.</param>
        public TeamFullException( string message, Exception inner )
            : base( message, inner ) {
            _logger.Error( message );
        }

        /// <summary>
        /// Initializes a new instance of the TeamFullException class with a specified error message, a reference to the
        /// inner exception that is the cause of this exception, and a logger for recording the error.
        /// </summary>
        /// <remarks>The error message is logged using the provided logger when the exception is
        /// constructed.</remarks>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is
        /// specified.</param>
        /// <param name="logger">The logger used to record the error message associated with this exception.</param>
        public TeamFullException( string message, Exception inner, Logger logger )
            : base( message, inner ) {
            logger.Error( this, message );
        }
    }

    /// <summary>
    /// Thrown when an OrionMatch class instance does not have an expected backwards pointer
    /// to its holder. For example, if a CourseOfFireEntry does not have a backwards pointer to its MatchParticipant.
    /// <para>Most likely would occur if an instance is deserialized outside of a <see cref="MatchProject"/> and
    /// the user then tries to perform an action requiring a MatchProject.</para>
    /// </summary>
    public class BackwardsPointerException : OrionMatchException {

        private const string DEFAULT_MESSAGE = "The OrionMatch class instance does not have an expected value for a Backwards pointer to it's holder. Likely occured because the instance was created outside the scope of a MatchProject and the user tried to perform an operation requiring a MatchProject.";

        /// <summary>
        /// Initializes a new instance of the BackwardsPointException class with a default error message indicating that the
        /// team is full.
        /// </summary>
        public BackwardsPointerException()
            : base( DEFAULT_MESSAGE ) {
            _logger.Error( DEFAULT_MESSAGE );
        }

        /// <summary>
        /// Initializes a new instance of the BackwardsPointException class with a specified logger and a default error
        /// message.
        /// </summary>
        /// <remarks>This constructor logs the default error message using the provided logger when the
        /// exception is instantiated.</remarks>
        /// <param name="logger">The logger used to record the error message when the exception is created.</param>
        public BackwardsPointerException( Logger logger )
            : base( DEFAULT_MESSAGE ) {
            logger.Error( DEFAULT_MESSAGE );
        }

        /// <summary>
        /// Initializes a new instance of the BackwardsPointException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public BackwardsPointerException( string message )
            : base( message ) {
            _logger.Error( message );
        }

        /// <summary>
        /// Initializes a new instance of the BackwardsPointException class with a specified error message and logs the error
        /// using the provided logger.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="logger">The logger used to record the error message.</param>
        public BackwardsPointerException( string message, Logger logger )
            : base( message ) {
            logger.Error( this, message );
        }

        /// <summary>
        /// Initializes a new instance of the BackwardsPointException class with a specified error message and a reference to
        /// the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is
        /// specified.</param>
        public BackwardsPointerException( string message, Exception inner )
            : base( message, inner ) {
            _logger.Error( message );
        }

        /// <summary>
        /// Initializes a new instance of the BackwardsPointException class with a specified error message, a reference to the
        /// inner exception that is the cause of this exception, and a logger for recording the error.
        /// </summary>
        /// <remarks>The error message is logged using the provided logger when the exception is
        /// constructed.</remarks>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or a null reference if no inner exception is
        /// specified.</param>
        /// <param name="logger">The logger used to record the error message associated with this exception.</param>
        public BackwardsPointerException( string message, Exception inner, Logger logger )
            : base( message, inner ) {
            logger.Error( this, message );
        }
    }
}
