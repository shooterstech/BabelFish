using System.Reflection;
using System.IO;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.APIClients
{
    public class DefinitionCache
    {
        private const string DEFINITION_FILE_EXT = ".json";
        private const string DEFINITION_DIR = "DEFINITIONS";
        private const int DEFINITION_EXPIRE = 14;

        private List<DataModel.Definitions.DefinitionCache> definitionsCached = new List<DataModel.Definitions.DefinitionCache>();

        private bool prefDefintionsCacheIgnore = false;

        private TimeSpan prefCacheExpireTime = TimeSpan.FromDays(DEFINITION_EXPIRE);

        private DirectoryInfo localDefinitionDirectory;

        public static DefinitionCache CACHE= new DefinitionCache();

        private DefinitionCache() {
            string appDataDirectory = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData );
            string definitionDirectory = $"{appDataDirectory}\\{DEFINITION_DIR}";

            localDefinitionDirectory = new DirectoryInfo( definitionDirectory );
        }

        /// <summary>
        /// Attempts to retreive a definition from memory. Returns true if there was a cache hit, false otherwise
        /// </summary>
        /// <typeparam name="T">Should be a concrete implementation of abstract class .DataModel.Definitions.Definition</typeparam>
        /// <param name="definitionType"></param>
        /// <param name="setName"></param>
        /// <param name="definition"></param>
        /// <returns></returns>
        public bool TryGetDefinition<T>( DefinitionType definitionType, SetName setName, out T definition ) {
            definition = default(T);
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Should be a concrete implementation of abstract class .DataModel.Definitions.Definition</typeparam>
        /// <param name="definition"></param>
        public void SaveDefinition<T>( T definition ) {
            ;
        }
    }
}