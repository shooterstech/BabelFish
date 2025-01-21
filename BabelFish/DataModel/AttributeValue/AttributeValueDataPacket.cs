﻿using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.DataModel.AttributeValue
{

    [Serializable]
    public abstract class AttributeValueDataPacket : IDeserializableAbstractClass {

        public const int CONCRETE_CLASS_ID = 1;

        public AttributeValueDataPacket() {
            this.ConcreteClassId = CONCRETE_CLASS_ID;
        }

        /// <summary>
        /// the SetName, formatted as a string, of the Attribute definition.
        /// </summary>
        public string AttributeDef { get; set; }

        
        public AttributeValue AttributeValue { get; set; }

        protected internal Task<AttributeValue> AttributeValueTask { get; set; }

        /// <summary>
        /// Deserilization of a AttributeValueDataPacket is handled by the overridden ReadJson()
        /// method of AttributeValueDataPacketConverter class. Because to deserialize an AttributeValue
        /// the Definition of the Attribute must be known. And reading the Definition is an IO bound
        /// Async call. But ReadJson() is not Async and can't be made async because it is overridden.
        /// To get around this limitation, the Task is assigned to AttributeValueTask (instead
        /// of awaiting and assigning to AttributeValue. The awaiting of AttributeValueTask
        /// is then handled in an async call sepeartly.
        /// </summary>
        /// <returns></returns>
        protected internal async Task FinishInitializationAsync() {
            AttributeValue = await AttributeValueTask;
        }

        public VisibilityOption Visibility { get; set; }

        /// <summary>
        /// Implementation of the IDeserializableAbstractClass interface.
        /// To have added control over the Deserialization of abstract classes, in to
        /// Concrete classes, the JSON should include a ConcreteClassId that specifies
        /// the Concrete class.
        /// </summary>
        public int ConcreteClassId { get; set; }
    }
}
