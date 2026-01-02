using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Converters.Newtonsoft {
    public class MatchIdConverter : JsonConverter<MatchID> {

        public override void WriteJson( JsonWriter writer, MatchID value, JsonSerializer serializer ) {
            writer.WriteValue( value.ToString() );
        }

        public override MatchID ReadJson( JsonReader reader, Type objectType, MatchID existingValue, bool hasExistingValue, JsonSerializer serializer ) {
            string matchIdString = (string)reader.Value;

            if ( MatchID.TryParse( matchIdString, out MatchID mId ) ) {
                return mId;
            }

            return new MatchID( "1.1.1.1" );
        }
    }
}
