namespace Scopos.BabelFish.DataModel.OrionMatch {
    public interface ICheckSum {

        /// <summary>
        /// A hashed value that is unique to this object. Must ignore LastUpdated and CheckSum properties when calculating the hash.
        /// </summary>
        /// <remarks>To maintain backward compatibility, CheckSum is a string representation of the hash value (and not an int) and
        /// spelled wiht a capital S in Sum.</remarks>
        public string CheckSum { get; set; }

        /// <summary>
        /// Calculates a checksum value that represents the current state of the object's properties, excluding the
        /// LastUpdated and CheckSum properties.
        /// <para>After an object is deserialized, this method can be used to verify the integrity of the deserialized data by comparing
        /// the calculated value against the stored CheckSum.</para>
        /// </summary>
        /// <returns>A string containing the calculated checksum value for the object.</returns>
        public ulong CalculateChecksum();
    }
}
