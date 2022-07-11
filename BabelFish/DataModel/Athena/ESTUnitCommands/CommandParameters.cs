using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShootersTech.DataModel;

namespace ShootersTech.BabelFish.DataModel.Athena.ESTUnitCommands {

    /// <summary>
    /// Abstract class representing the parameters to send to an EST Unit Command message.
    /// </summary>
    public abstract class CommandParameters : IDeserializableAbstractClass {



        /// <inheritdoc/>
        public int ConcreteClassId { get; set; }

    }
}
