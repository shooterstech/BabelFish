using System;
using System.Collections.Generic;
using System.Text.Json;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.Converters {
    public static class SerializerOptions {

        /// <summary>
        /// Serializer options for the APIClient.
        /// </summary>
        public static JsonSerializerOptions APIClientDeserializer = null;

        /// <summary>
        /// Initializes the static instance for the JsonSerializerOptions. Should be used by the APIClient.
        /// </summary>
        public static void InitAPIClientDeserializer() {

            if (APIClientDeserializer == null) {
                APIClientDeserializer = new JsonSerializerOptions();
                lock( APIClientDeserializer ) {
                    APIClientDeserializer.Converters.Add( new DefinitionConverter() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<FieldType>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<SortBy>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<DataModel.Definitions.ValueType>() );
                    APIClientDeserializer.Converters.Add( new ParticipantConverter() );
                    APIClientDeserializer.Converters.Add( new ScoposDateTimeConverter() );
                    APIClientDeserializer.Converters.Add( new ScoposDateOnlyConverter() );
                    APIClientDeserializer.Converters.Add( new ScoposTimeOnlyConverter() );
                    APIClientDeserializer.Converters.Add( new ScoreAverageBaseConverter() );
                    APIClientDeserializer.Converters.Add( new ScoreHistoryBaseConverter() );
                    APIClientDeserializer.Converters.Add( new ShowWhenBaseConverter() );
                    APIClientDeserializer.Converters.Add( new SquaddingAssignmentConverter() );
                    APIClientDeserializer.Converters.Add( new TieBreakingRuleConverter() );
                }
            }
        }
    }
}
