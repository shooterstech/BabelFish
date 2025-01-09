using System;
using System.Collections.Generic;
using System.Text.Json;

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
                lock( APIClientDeserializer ) {
                    APIClientDeserializer.Converters.Add( new TieBreakingRuleConverter() );
                    APIClientDeserializer.Converters.Add( new SquaddingAssignmentConverter() );
                    APIClientDeserializer.Converters.Add( new ShowWhenBaseConverter() );
                    APIClientDeserializer.Converters.Add( new ScoreHistoryBaseConverter() );
                    APIClientDeserializer.Converters.Add( new ScoreAverageBaseConverter() );
                    APIClientDeserializer.Converters.Add( new ParticipantConverter() );
                    APIClientDeserializer.Converters.Add( new DefinitionConverter() );
                }
            }
        }
    }
}
