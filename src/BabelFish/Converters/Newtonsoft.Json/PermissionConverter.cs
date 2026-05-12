using Newtonsoft.Json;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.Converters.Newtonsoft {
    public class PermissionConverter : JsonConverter<Permission> {

        private Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public override void WriteJson( JsonWriter writer, Permission value, JsonSerializer serializer ) {
            writer.WriteValue( value.ToString() );
        }

        public override Permission ReadJson( JsonReader reader, Type objectType, Permission existingValue, bool hasExistingValue, JsonSerializer serializer ) {
            string permissionString = (string)reader.Value;

            if (Permission.TryParse( permissionString, out Permission sn )) {
                return sn;
            }

            _logger.Error( $"Couldn't parse the incoming Permission value '{permissionString}'." );
            return Permission.DEFAULT;
        }
    }
}
