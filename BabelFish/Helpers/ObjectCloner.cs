using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Scopos.BabelFish.Helpers {
    public static class ObjectCloner {

        /// <summary>
        /// Perform a deep Copy of the object, using Json as a serialization method. 
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>

        public static T Clone<T>( T source ) {
            if (source == null) {
                return default;
            }

            var options = new JsonSerializerOptions {
                WriteIndented = false,
                ReferenceHandler = ReferenceHandler.Preserve
            };

            string jsonString = JsonSerializer.Serialize( source, options );
            return JsonSerializer.Deserialize<T>( jsonString, options );
        }

        /*
        public static T Clone<T>( this T source ) {
            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals( source, null )) return default;

            // initialize inner objects individually
            // for example in default constructor some list property initialized with some values,
            // but in 'source' these items are cleaned -
            // without ObjectCreationHandling.Replace default constructor values will be added to result
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>( JsonConvert.SerializeObject( source ), deserializeSettings );
        }
        */
    }
}
