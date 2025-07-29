
namespace Scopos.BabelFish.Helpers {
    public static class ObjectCloner {

        /// <summary>
        /// Perform a deep Copy of the object, using Json as a serialization method. 
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>

        public static T Clone<T>( this T source ) {
            if (source == null) {
                return default;
            }

            var options = SerializerOptions.SystemTextJsonDeserializer;

            string jsonString = G_NS.JsonConvert.SerializeObject( source, Helpers.SerializerOptions.NewtonsoftJsonSerializer );
            return G_STJ.JsonSerializer.Deserialize<T>( jsonString, options );
        }
    }
}
