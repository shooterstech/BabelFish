namespace Scopos.BabelFish.DataModel.OrionMatch {

    /// <summary>
    /// Interface to be implemented by classes that need to have a checksum value calculated for them. The checksum value is intended to be a
    /// unique value that represents the state of the instance's properties, excluding the LastUpdated and CheckSum properties. The CalculateChecksum()
    /// method should return the same value across multiple serializations and deserializations of the same instance, assuming the underlying data did not change.
    /// This allows for integrity verification after deserialization and comparison between server and local copies of the instance.
    /// </summary>
    public interface ICheckSum {

        /// <summary>
        /// A hashed value that is unique to this instance. The value is calculated by the CalculateChecksum() method.
        /// </summary>
        /// <remarks>To maintain backward compatibility, CheckSum is a string representation of the hash value (and not an ulong) and
        /// spelled with a capital S in CheckSum.</remarks>
        /// <remarks>Class instances that are not, by them selves, stored on the server may mark this property as JsonIgnore.</remarks>
        public string CheckSum { get; set; }

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
    }
}
