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
}
