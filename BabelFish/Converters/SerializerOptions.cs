using System;
using System.Collections.Generic;
using System.Text.Json;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
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
                    APIClientDeserializer.Converters.Add( new AttributeFieldConverter() );
                    APIClientDeserializer.Converters.Add( new AttributeValidationConverter() );
                    APIClientDeserializer.Converters.Add( new DefinitionConverter() );
                    APIClientDeserializer.Converters.Add( new ParticipantConverter() );
                    APIClientDeserializer.Converters.Add( new ScoposDateTimeConverter() );
                    APIClientDeserializer.Converters.Add( new ScoposDateOnlyConverter() );
                    APIClientDeserializer.Converters.Add( new ScoreAverageBaseConverter() );
                    APIClientDeserializer.Converters.Add( new ScoreHistoryBaseConverter() );
                    APIClientDeserializer.Converters.Add( new ShowWhenBaseConverter() );
                    APIClientDeserializer.Converters.Add( new SquaddingAssignmentConverter() );
                    APIClientDeserializer.Converters.Add( new TieBreakingRuleConverter() );

                    //Definition Enums
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<AimingMarkColor>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<AttributeDesignation>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<BarcodeLabelSize>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<COFTypeOptions>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<CompetitionType>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<DisciplineType>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<DisplayEventOptions>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<EventtType>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<FieldType>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<LightIllumination>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<LinkToOption>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<ResultFieldMethod>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<ScoreComponent>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<ScoringShape>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<ShotMappingMethodType>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<ShowWhenBoolean>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<ShowWhenCondition>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<ShowWhenOperation>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<SingularType>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<SpecialOptions>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<SortBy>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<TieBreakingRuleMethod>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<TimerCommandOptions>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<DataModel.Definitions.ValueType>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<VisibilityOption>() );

                    //Match Enums
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<LeagueRankingRuleType>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<LeagueSeasonType>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<LeagueVirtualType>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<MatchAuthorizationRole>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<MatchTypeOptions>() );
                    APIClientDeserializer.Converters.Add( new EnumConverterByDescription<ResultStatus>() );
                }
            }
        }
    }
}
