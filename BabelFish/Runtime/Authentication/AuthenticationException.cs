using Scopos.BabelFish.Runtime;
using System;
using System.Collections.Generic;
using System.Text;
using NLog;

namespace Scopos.BabelFish.Runtime.Authentication {
    public class AuthenticationException : ShootersTechException {
        public AuthenticationException()
            : base( "Something bad happened!" ) {
        }
        public AuthenticationException( string message )
            : base( message ) {
        }

        public AuthenticationException( string message, Logger logger )
            : base( message, logger ) {
        }

        public AuthenticationException( string message, Exception inner )
            : base( message, inner ) {
        }

        public AuthenticationException( string message, Exception inner, Logger logger )
            : base( message, inner, logger ) {
        }
    }

    public class NotAuthorizedException : AuthenticationException {
        public NotAuthorizedException()
            : base( "Something bad happened!" ) {
        }
        public NotAuthorizedException( string message )
            : base( message ) {
        }

        public NotAuthorizedException( string message, Logger logger )
            : base( message, logger ) {
        }

        public NotAuthorizedException( string message, Exception inner )
            : base( message, inner ) {
        }

        public NotAuthorizedException( string message, Exception inner, Logger logger )
            : base( message, inner, logger ) {
        }
    }

    public class DeviceNotKnownException : AuthenticationException {
        public DeviceNotKnownException()
            : base( "Something bad happened!" ) {
        }
        public DeviceNotKnownException( string message )
            : base( message ) {
        }

        public DeviceNotKnownException( string message, Logger logger )
            : base( message, logger ) {
        }

        public DeviceNotKnownException( string message, Exception inner )
            : base( message, inner ) {
        }

        public DeviceNotKnownException( string message, Exception inner, Logger logger )
            : base( message, inner, logger ) {
        }
    }
}
