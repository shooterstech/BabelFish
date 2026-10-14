using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.Converters.Microsoft {

    /// <summary>
    /// Custom converter for <see cref="ConcurrentBag{T}"/> to allow it to be serialized and deserialized by System.Text.Json, since Microsoft can't seem to do this themselves.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConcurrentBagConverter<T> : JsonConverter<ConcurrentBag<T>> {
        public override ConcurrentBag<T> Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            var items = JsonSerializer.Deserialize<List<T>>( ref reader, options );
            return new ConcurrentBag<T>( items ?? new List<T>() );
        }

        public override void Write( Utf8JsonWriter writer, ConcurrentBag<T> value, JsonSerializerOptions options ) {
            JsonSerializer.Serialize( writer, value.ToList(), options );
        }
    }

}
