﻿using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Runtime;
using NLog;

namespace Scopos.BabelFish.DataModel.AttributeValue {
    public class AttributeValueException : ScoposException {

        public AttributeValueException()
            : base( "Something bad happened!" ) {
        }
        public AttributeValueException( string message )
            : base( message ) {
        }
        public AttributeValueException( string message, Logger logger )
            : base( message ) {
            logger.Error( this, message );
        }
        public AttributeValueException( string message, Exception inner )
            : base( message, inner ) {
        }
        public AttributeValueException( string message, Exception inner, Logger logger )
            : base( message, inner ) {
            logger.Error( this, message );
        }
    }

    /// <summary>
    /// Thrown when attempting to set a value that is either the wrong type or does not pass validation.
    /// </summary>
    public class AttributeValueValidationException : ScoposException {

        public AttributeValueValidationException()
            : base( "Inappropraite value." ) {
        }
        public AttributeValueValidationException( string message )
            : base( message ) {
        }
        public AttributeValueValidationException( string message, Logger logger )
            : base( message ) {
            logger.Error( this, message );
        }
        public AttributeValueValidationException( string message, Exception inner )
            : base( message, inner ) {
        }
        public AttributeValueValidationException( string message, Exception inner, Logger logger )
            : base( message, inner ) {
            logger.Error( this, message );
        }
    }
}
