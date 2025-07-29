using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Runtime;
using NLog;

namespace Scopos.BabelFish.APIClients {


    /// <summary>
    /// Thrown when an API Client could not complete its call successfully.
    /// 
    /// Excpected to be used as a base class for more specific exceptions.
    /// </summary>
    public class ScoposAPIException : ScoposException {
        public ScoposAPIException()
            : base( "Rest API call could not be completed successfully." ) {
        }
        public ScoposAPIException( string message )
            : base( message ) {
        }

        public ScoposAPIException( string message, Logger logger )
            : base( message, logger ) {
        }

        public ScoposAPIException( string message, Exception inner )
            : base( message, inner ) {
        }

        public ScoposAPIException( string message, Exception inner, Logger logger )
            : base( message, inner, logger ) {
        }
    }

    /// <summary>
    /// Thrown when the Definition could not be found.
    /// Thrown by the DefinitionCache.
    /// </summary>
    public class DefinitionNotFoundException : ScoposAPIException {

        public DefinitionNotFoundException()
            : base( "Attribute definition could not be found." ) {
        }
        public DefinitionNotFoundException( string message )
            : base( message ) {
        }
        public DefinitionNotFoundException( string message, Logger logger )
            : base( message ) {
            logger.Error( this, message );
        }
        public DefinitionNotFoundException( string message, Exception inner )
            : base( message, inner ) {
        }
        public DefinitionNotFoundException( string message, Exception inner, Logger logger )
            : base( message, inner ) {
            logger.Error( this, message );
        }
    }
}
