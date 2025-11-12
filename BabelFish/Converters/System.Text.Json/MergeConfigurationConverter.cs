using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataActors.Tournaments;

namespace Scopos.BabelFish.Converters.Microsoft {
    public class MergeConfigurationConverter : JsonConverter<MergeConfiguration> {

        public override MergeConfiguration? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {
            using (JsonDocument doc = JsonDocument.ParseValue( ref reader )) {
                JsonElement root = doc.RootElement;

                if (root.TryGetProperty( "Method", out JsonElement methodValue )) {

                    switch (methodValue.ToString()) {
                        case SumMethod.IDENTIFIER:
                            return JsonSerializer.Deserialize<SumMethodConfiguration>( root.GetRawText(), options );
                        case AverageMethod.IDENTIFIER:
                            return JsonSerializer.Deserialize<AverageMethodConfiguration>( root.GetRawText(), options );
                        case ReentryMethod.IDENTIFIER:
                            var foo = JsonSerializer.Deserialize<ReentryMethodConfiguration>( root.GetRawText(), options );
                            return foo;
                        default:
                            break;
                    }
                }

                //If we get here, give up. 
                throw new NotImplementedException( $"Unable to convert abstract class MergeMethodConfiguration with it's concrete class. Likely because it does not have a 'Method' property with known value." );
            }
        }

        public override void Write( Utf8JsonWriter writer, MergeConfiguration value, JsonSerializerOptions options ) {

            JsonSerializer.Serialize( writer, value, value.GetType(), options );

        }
    }
}
