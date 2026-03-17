namespace Scopos.BabelFish.DataModel.Common {

    /// <summary>
    /// Interface defining a standard set of methods to implemnt to save a class instance
    /// to a file.
    /// </summary>
    interface ISaveToFile {

        /// <summary>
        /// Returns the standard file name used for this class instance. This should be 
        /// used by GetRelativePath() and SaveToFile().
        /// </summary>
        /// <param name="composite"></param>
        /// <returns></returns>
        string GetFileName( IBaseName composite );

        /// <summary>
        /// Saves this class instance to a file with the standard file name (as returned by GetFileName()) in the provided relative directory.
        /// </summary>
        /// <param name="relativeDirectory"></param>
        /// <param name="composite"></param>
        /// <returns></returns>
        string SaveToFile( DirectoryInfo relativeDirectory, IBaseName composite );

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
        string GetRelativePath( IBaseName composite );

        /// <summary>
        /// Returns this class instance as serialized json.
        /// </summary>
        /// <returns></returns>
        string SerializeToJson();
    }

    public interface IBaseName {

        string BaseName { get; }
    }

    public class StandardBaseName : IBaseName {

        public static StandardBaseName NONE = new StandardBaseName( "" );

        private StandardBaseName( string baseName ) {
            this.BaseName = baseName;
        }

        public string BaseName { get; private set; }

    }
}
