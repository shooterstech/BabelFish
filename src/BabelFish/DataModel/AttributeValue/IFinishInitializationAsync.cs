namespace Scopos.BabelFish.DataModel.AttributeValue {

    /// <summary>
    /// Classes that contain AttributeValueDataPackets, or has properties that do, should implement this interface. After reading a class insstnace from json
    /// (aka deserialization) the FinishInitializationAsync() method should be called to finish the initialization of all AttributeValueDataPackets in the class instance. 
    public interface IFinishInitializationAsync {

        /// <summary>
        /// Deserilization of a AttributeValueDataPacket is handled by the overridden ReadJson()
        /// method of AttributeValueDataPacketConverter class. Because to deserialize an AttributeValue
        /// the Definition of the Attribute must be known. And reading the Definition is an IO bound
        /// Async call. But ReadJson() is not Async and can't be made async because it is overridden.
        /// To get around this limitation, the Task is assigned to AttributeValueTask (instead
        /// of awaiting and assigning to AttributeValue. The awaiting of AttributeValueTask
        /// is then handled in an async call sepeartly.
        /// </summary>
        /// 
        /// <returns></returns>
        public Task FinishInitializationAsync();
    }
}
