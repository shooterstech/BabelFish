﻿using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.Runtime;

namespace ShootersTech.Requests {
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