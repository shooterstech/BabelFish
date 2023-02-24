using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.DataModel.Athena.ESTUnitCommands {

    /// <summary>
    /// Abstract class representing the parameters to send to an EST Unit Command message.
    /// </summary>
    public abstract class CommandParameters : IDeserializableAbstractClass {

        /*
         * A description of how to describe Inherited / Abstract classes in OpenAPI 3.0 is at https://swagger.io/docs/specification/data-models/inheritance-and-polymorphism/
         */

        /// <summary>
        /// Implementation of the IDeserializableAbstractClass interface.
        /// To have added control over the Deserialization of abstract classes, in to
        /// Concrete classes, the JSON should include a ConcreteClassId that specifies
        /// the Concrete class.
        /// </summary>
        public int ConcreteClassId { get; set; }

    }
}
