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

        private TimeSpan prefCacheExpireTime = TimeSpan.FromDays(DEFINITION_EXPIRE);

        private DirectoryInfo localDefinitionDirectory;

        //This is where previously read definitions will be stored in memory. The Key is a combination of set name and definition type. Teh value is the Definition object.
        private Dictionary< string, Definition> cachedDefinitions = new Dictionary< string, Definition>();

        public static DefinitionCache CACHE= new DefinitionCache();

        private DefinitionCache() {
            string appDataDirectory = Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData );
            string definitionDirectory = $"{appDataDirectory}\\{DEFINITION_DIR}";

            localDefinitionDirectory = new DirectoryInfo( definitionDirectory );
        }

        /// <summary>
        /// Attempts to retreive a definition from memory. Returns true if there was a cache hit, false otherwise
        /// </summary>
        /// <param name="definitionType"></param>
        /// <param name="setName"></param>
        /// <param name="definition"></param>
        /// <returns></returns>
        public bool TryGetDefinition<T>( DefinitionType definitionType, SetName setName, out T definition ) where T : Definition {
            var key = Key( definitionType, setName );
            Definition d;
            if (cachedDefinitions.TryGetValue( key, out d)) {
                definition = (T) d;
                return true;
            } else {
                definition = default(T);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="definition"></param>
        public void SaveDefinition<T>(  SetName setName, T definition ) where T : Definition {
            var definitionType = definition.Type;
            var key = Key( definitionType, setName );
            //todo only save to cache, if we dont have a copy, or this is a newer version

            cachedDefinitions[ key ] = definition;
        }

        private string Key( DefinitionType definitionType, SetName setName ) {
            return $"{definitionType}:{setName}";
        }
    }
}