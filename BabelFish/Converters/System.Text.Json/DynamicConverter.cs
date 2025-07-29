using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.Converters.Microsoft {

	/// <summary>
	/// Tries and converts unknown data strucutures into a dynamic field
	/// </summary>
	public class DynamicConverter : JsonConverter<ExpandoObject> {

		protected static Logger Logger = LogManager.GetCurrentClassLogger();

		/// <inheritdoc />
		public override ExpandoObject Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options ) {

			using JsonDocument doc = JsonDocument.ParseValue( ref reader );
			return JsonToExpando( doc.RootElement );
		}

		private ExpandoObject JsonToExpando( JsonElement element ) {
			var expando = new ExpandoObject() as IDictionary<string, object>;

			foreach (JsonProperty prop in element.EnumerateObject()) {
				expando[prop.Name] = prop.Value.ValueKind switch {
					JsonValueKind.Number => prop.Value.GetDouble(),
					JsonValueKind.String => prop.Value.GetString(),
					JsonValueKind.True => true,
					JsonValueKind.False => false,
					JsonValueKind.Object => JsonToExpando( prop.Value ),
					JsonValueKind.Array => prop.Value.EnumerateArray(),
					_ => null
				};
			}

			return (ExpandoObject)expando;
		}

		public override void Write( Utf8JsonWriter writer, ExpandoObject value, JsonSerializerOptions options ) {
			writer.WriteStartObject();

			var dictionary = (IDictionary<string, object>)value;
			foreach (var kvp in dictionary) {
				writer.WritePropertyName( kvp.Key );
				JsonSerializer.Serialize( writer, kvp.Value, options );
			}

			writer.WriteEndObject();
		}

	}
}
