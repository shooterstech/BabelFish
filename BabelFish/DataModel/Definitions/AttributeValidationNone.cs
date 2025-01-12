using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class AttributeValidationNone : AttributeValidation, ICopy<AttributeValidationNone> {


        public AttributeValidationNone Copy() {

            AttributeValidationNone copy = new AttributeValidationNone();
            base.Copy( copy );


            return copy;
        }
    }
}
