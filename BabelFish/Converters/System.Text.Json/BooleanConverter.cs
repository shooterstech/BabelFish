using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.Converters.Microsoft {

	/// <summary>
	/// Tries and converts, what should be a bool, but got serialzied as a string  (e.g. "False") into a boolean.
	/// </summary>
	public class BooleanConverter : JsonConverter<bool> {

		protected static Logger Logger = LogManager.GetCurrentClassLogger();

		/// <inheritdoc />
		public override bool Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

			if (reader.TokenType == JsonTokenType.True)
				return true;

			if (reader.TokenType == JsonTokenType.False
				|| reader.TokenType == JsonTokenType.Null)
				return false;

			if (reader.TokenType == JsonTokenType.String) {
				string boolString = reader.GetString();

				if (!string.IsNullOrEmpty( boolString )) {

					if (bool.TryParse( boolString, out bool result )) {
						return result;
					}

					Logger.Warn( $"Could not parse bool string '{boolString}' using a generic parser." );

				}
			}

			return false;
		}

		/// <inheritdoc />
		public override void Write( Utf8JsonWriter writer, bool value, JsonSerializerOptions options ) {
			writer.WriteBooleanValue( value );
		}
	}
}
