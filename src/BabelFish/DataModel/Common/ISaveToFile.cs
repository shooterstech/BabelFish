using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataModel.Common {

    /// <summary>
    /// Interface defining a standard set of methods to implemnt to save a class instance
    /// to a file.
    /// </summary>
    public interface ISaveToFile {

        /// <summary>
        /// Returns the standard file name used for this class instance. This should be 
        /// used by GetRelativePath() and SaveToFile().
        /// </summary>
        /// <param name="composite"></param>
        /// <returns></returns>
        string GetFileName();

        /// <summary>
        /// Saves this class instance to a file with the standard file name (as returned by GetFileName()) in the provided relative directory.
        /// </summary>
        /// <param name="relativeDirectory"></param>
        /// <param name="composite"></param>
        /// <returns></returns>
        string SaveToFile( DirectoryInfo relativeDirectory );

        /// <summary>
        /// Saves this class instance to the provided file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        string SaveToFile( FileInfo file );

        /// <summary>
        /// Returns the relative path where this class instance is saved.
        /// </summary>
        /// <param name="composite"></param>
        /// <returns></returns>
        string GetRelativePath();

        /// <summary>
        /// Returns this class instance as serialized json.
        /// </summary>
        /// <returns></returns>
        string SerializeToJson();

        /*
         * NOTE Can not add LoadFromFile() type methods to this interface because the method needs to be static, and C# does not allow static methods in interfaces.
         */

        /// <summary>
        /// A hashed value that is unique to this instance. The value is calculated by the <see cref="ICheckSum.CalculateChecksum()"/> method.
        /// </summary>
        /// <remarks>To maintain backward compatibility, CheckSum is a string representation of the hash value (and not an ulong) and
        /// spelled with a capital S in CheckSum.</remarks>
        public string CheckSum { get; set; }
    }
}
