using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Runtime;
using NLog;

namespace Scopos.BabelFish.DataModel.AttributeValue {
    public class AttributeValueException : ShootersTechException {

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
}
