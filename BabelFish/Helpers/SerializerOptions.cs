using System;
using System.Collections.Generic;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.Helpers;
using Scopos.BabelFish.DataModel.Athena;
using System.Net;
using Scopos.BabelFish.DataModel.Clubs;
using Scopos.BabelFish.DataModel.SocialNetwork;
using Scopos.BabelFish.DataModel.Common;

namespace Scopos.BabelFish.Helpers {
    public static class SerializerOptions {

        /// <summary>
        /// Serializer options for the APIClient.
        /// </summary>
        public static G_STJ.JsonSerializerOptions SystemTextJsonSerializer = null;

        public static G_NS.JsonSerializerSettings NewtonsoftJsonDeserializer = null;

        /// <summary>
        /// Initializes the standard serializers used by and with BabelFish.
        /// </summary>
        public static void InitSerializers() {
            InitSystemTextJsonSerializer();
            InitNewtonsoftJsonDeserializer();
        }
        /// <summary>
        /// Initializes the static instance for the JsonSerializerOptions. Should be used by the APIClient.
        /// </summary>
        private static void InitSystemTextJsonSerializer() {

            if (SystemTextJsonSerializer == null) {
                SystemTextJsonSerializer = new G_STJ.JsonSerializerOptions();

                lock( SystemTextJsonSerializer ) {

                    //Don't write a property if the value is equal to the default
                    //NOTE: Add the [JsonInclude] decorate on properties to always write to json
                    SystemTextJsonSerializer.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;

                    //Write indented
                    SystemTextJsonSerializer.WriteIndented = true;

                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.AttributeFieldConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.AttributeValidationConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.AttributeValueDataPacketConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.AttributeValueDataPacketAPIResponseConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.AttributeValueDataPacketMatchConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EventConverter() );
                    //APIClientSerializer.Converters.Add( new ExcludeEmptyStringConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.DefinitionConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.ListOfAttributeValueDataPackets() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.ParticipantConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.ScoposDateTimeConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.ScoposDateOnlyConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.ScoreAverageBaseConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.ScoreHistoryBaseConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.SetAttributeValueListConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.ShowWhenBaseConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.SquaddingAssignmentConverter() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.TieBreakingRuleConverter() );

                    //Definition Enums
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<AimingMarkColor>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<AttributeDesignation>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<BarcodeLabelSize>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<COFTypeOptions>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<CompetitionType>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<DefinitionType>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<DisciplineType>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<DisplayEventOptions>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<EventCalculation>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<EventDerivationType>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<EventtType>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<FieldType>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<LightIllumination>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<LinkToOption>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ResultFieldMethod>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ScoreComponent>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ScoringShape>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ShotMappingMethodType>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ShowWhenBoolean>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ShowWhenCondition>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ShowWhenOperation>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<SingularType>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<SpecialOptions>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<SortBy>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<TieBreakingRuleMethod>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<TimerCommandOptions>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<DataModel.Definitions.ValueType>() );

                    //Common Enums
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<VisibilityOption>() );

                    //Club Enums
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ClubLicenseCapability>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ClubLicenseType>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ClubOptions>() );

                    //Match Enums
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<LeagueRankingRuleType>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<LeagueSeasonType>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<LeagueVirtualType>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<MatchAuthorizationRole>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<MatchParticipantRole>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<MatchTypeOptions>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ResultStatus>() );

                    //Other Enums
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<APIStage>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<APISubDomain>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ClubLicenseCapability>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<SocialRelationshipName>() );

                    //AbstractEST
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ESTUnitCommand>() );
                    SystemTextJsonSerializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ReplaceVariableOptions>() );
                }
            }
        }

        private static void InitNewtonsoftJsonDeserializer() {
            NewtonsoftJsonDeserializer = new G_NS.JsonSerializerSettings();

            NewtonsoftJsonDeserializer.TypeNameHandling = G_NS.TypeNameHandling.None;
            NewtonsoftJsonDeserializer.NullValueHandling = G_NS.NullValueHandling.Ignore;
            NewtonsoftJsonDeserializer.Formatting = G_NS.Formatting.Indented;
            NewtonsoftJsonDeserializer.DefaultValueHandling = G_NS.DefaultValueHandling.Ignore;

            //Lets enums get deserizlized from their stirng or description representation.
            NewtonsoftJsonDeserializer.Converters.Add( new G_NS_CONV.StringEnumConverter() );
        }
    }
}
