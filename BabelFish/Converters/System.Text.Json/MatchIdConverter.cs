using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Converters.Microsoft {
    public class MatchIdConverter : JsonConverter<MatchID> {
        public override MatchID? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

            string matchIdString = (string)reader.GetString();

            if (MatchID.TryParse( matchIdString, out MatchID mId )) {
                return mId;
            }

            return new MatchID( "1.1.1.1" );
        }

        public override void Write( Utf8JsonWriter writer, MatchID value, JsonSerializerOptions options ) {

            writer.WriteStringValue( value.ToString() );
        }
    }
}
