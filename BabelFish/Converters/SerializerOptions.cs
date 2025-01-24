using System;
using System.Collections.Generic;
using System.Text.Json;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.Athena;
using System.Net;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.DataModel.SocialNetwork;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.Converters
{
    public static class SerializerOptions {

        /// <summary>
        /// Serializer options for the APIClient.
        /// </summary>
        public static JsonSerializerOptions APIClientSerializer = null;

        /// <summary>
        /// Initializes the static instance for the JsonSerializerOptions. Should be used by the APIClient.
        /// </summary>
        public static void InitAPIClientDeserializer() {

            if (APIClientSerializer == null) {
                APIClientSerializer = new JsonSerializerOptions();
                lock( APIClientSerializer ) {

                    //Don't write a property if the value is equal to the default
                    //NOTE: Add the [JsonInclude] decorate on properties to always write to json
                    APIClientSerializer.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;

                    //Write indented
                    APIClientSerializer.WriteIndented = true;

                    APIClientSerializer.Converters.Add( new AttributeFieldConverter() );
                    APIClientSerializer.Converters.Add( new AttributeValidationConverter() );
                    APIClientSerializer.Converters.Add( new AttributeValueDataPacketConverter() );
                    APIClientSerializer.Converters.Add( new AttributeValueDataPacketAPIResponseConverter() );
                    APIClientSerializer.Converters.Add( new AttributeValueDataPacketMatchConverter() );
                    //APIClientSerializer.Converters.Add( new DefaultValueHandlingConverter<int>() );
                    APIClientSerializer.Converters.Add( new DefinitionConverter() );
                    APIClientSerializer.Converters.Add( new ListOfAttributeValueDataPackets() );
                    APIClientSerializer.Converters.Add( new ParticipantConverter() );
                    APIClientSerializer.Converters.Add( new ScoposDateTimeConverter() );
                    APIClientSerializer.Converters.Add( new ScoposDateOnlyConverter() );
                    APIClientSerializer.Converters.Add( new ScoreAverageBaseConverter() );
                    APIClientSerializer.Converters.Add( new ScoreHistoryBaseConverter() );
                    APIClientSerializer.Converters.Add( new SetAttributeValueListConverter() );
                    APIClientSerializer.Converters.Add( new ShowWhenBaseConverter() );
                    APIClientSerializer.Converters.Add( new SquaddingAssignmentConverter() );
                    APIClientSerializer.Converters.Add( new TieBreakingRuleConverter() );

                    //Definition Enums
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<AimingMarkColor>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<AttributeDesignation>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<BarcodeLabelSize>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<COFTypeOptions>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<CompetitionType>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<DefinitionType>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<DisciplineType>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<DisplayEventOptions>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<EventtType>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<FieldType>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<LightIllumination>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<LinkToOption>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<ResultFieldMethod>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<ScoreComponent>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<ScoringShape>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<ShotMappingMethodType>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<ShowWhenBoolean>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<ShowWhenCondition>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<ShowWhenOperation>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<SingularType>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<SpecialOptions>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<SortBy>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<TieBreakingRuleMethod>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<TimerCommandOptions>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<DataModel.Definitions.ValueType>() );

                    //Common Enums
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<VisibilityOption>() );

                    //Match Enums
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<LeagueRankingRuleType>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<LeagueSeasonType>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<LeagueVirtualType>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<MatchAuthorizationRole>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<MatchTypeOptions>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<ResultStatus>() );

                    //Other Enums
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<APIStage>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<APISubDomain>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<ClubLicenseCapability>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<SocialRelationshipName>() );

                    //AbstractEST
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<ESTUnitCommand>() );
                    APIClientSerializer.Converters.Add( new EnumConverterByDescription<ReplaceVariableOptions>() );
                }
            }
        }
    }
}
