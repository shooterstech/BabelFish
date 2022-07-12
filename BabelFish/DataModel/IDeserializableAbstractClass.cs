using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.BabelFish.DataModel {
    /// <summary>
    /// To have added control over the Deserialization of abstract classes, in to
    /// Concrete classes, the JSON should include a ConcreteClassId that specifies
    /// the Concrete class.
    ///
    /// Abstract classes that implement this Interface, will also have to implement
    /// a custom JsonConverter.
    /// 
    /// Recipe comes from https://stackoverflow.com/questions/20995865/deserializing-json-to-abstract-class
    /// </summary>
    public class IDeserializableAbstractClass {

        /// <summary>
        /// The value of ConcreteClassId should be set in the Constructor of the
        /// Concrete class. Users
        /// should not change this value ... unless they want really bizarre and
        /// uncontrolled behavior. 
        /// </summary>
        int ConcreteClassId{ get; set; }
    }
}
