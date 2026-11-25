namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Interface to be implemented by classes that need to have a checksum value calculated for them. The checksum value is intended to be a
    /// unique value that represents the state of the instance's properties, excluding the LastUpdated and CheckSum properties. The CalculateChecksum()
    /// method should return the same value across multiple serializations and deserializations of the same instance, assuming the underlying data did not change.
    /// This allows for integrity verification after deserialization and comparison between server and local copies of the instance.
    /// </summary>
    public interface ICheckSum {

        /// <summary>
        /// Calculates a checksum value that represents the current state of the instance's properties, excluding the
        /// LastUpdated and CheckSum properties. Assuming the underlying data did not change, the value of this method should be the same
        /// across multiple serializations and deserializations of the same instance.
        /// <para>The intent of this method is two-fold. First to provide a way to verify the integrity of the instance's data after deserialization.
        /// If someone (or something) modified the serialized values then CalculateChecksum should return a different value. While we may not know
        /// what changed, we can at least detect something manipulated the data.</para>
        /// <para>The second intent is to compare the value of the instance on the server to the local copy. If the value is the same (on the
        /// server and locally) then the server has the up to date instance value.</para>
        /// </summary>
        /// <returns>A ulong containing the calculated checksum value for the instance.</returns>
        public ulong CalculateChecksum();

        /*
         * NOTE: The CheckSum property is defined in the ISaveToFile interface, which is implemented by classes that also implement ICheckSum.
         * This design choice allows for a clear separation of concerns, where ICheckSum focuses on the calculation of the checksum value,
         * while ISaveToFile handles the storage and retrieval of that value. This approach ensures that classes implementing both interfaces
         * can maintain a consistent and reliable checksum mechanism without conflating the responsibilities of each interface.
         */
    }
}
