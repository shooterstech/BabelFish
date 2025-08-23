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
using Scopos.BabelFish.DataModel.ScoposData;
using Scopos.BabelFish.DataModel.Athena.AbstractEST;

namespace Scopos.BabelFish.Helpers {
    public static class SerializerOptions {

        /// <summary>
        /// Serializer options for the APIClient.
        /// </summary>
        public static G_STJ.JsonSerializerOptions SystemTextJsonDeserializer = null;

        public static G_NS.JsonSerializerSettings NewtonsoftJsonSerializer = null;

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

            if (SystemTextJsonDeserializer == null) {
                SystemTextJsonDeserializer = new G_STJ.JsonSerializerOptions();

                lock( SystemTextJsonDeserializer ) {

                    //Don't write a property if the value is equal to the default
                    //NOTE: Add the [JsonInclude] decorate on properties to always write to json
                    SystemTextJsonDeserializer.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;

                    //Write indented
                    SystemTextJsonDeserializer.WriteIndented = true;


					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.AbbreviatedFormatChildConverter() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.AttributeFieldConverter() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.AttributeValueDataPacketAPIResponseConverter() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.AttributeValueDataPacketConverter() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.AttributeValueDataPacketMatchConverter() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.CalculationVariableConverter() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.CommandAutomationConverter() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.DefinitionConverter() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.DisplayEntityConfigurationConverter() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EventConverter() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.ListOfAttributeValueDataPackets() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.ParticipantConverter() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.RelayConverter() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.ScoreAverageBaseConverter() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.ScoreHistoryBaseConverter() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.SetAttributeValueListConverter() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.ShowWhenBaseConverter() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.SquaddingAssignmentConverter() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.TieBreakingRuleConverter() );

					//Definition Enums 
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<AbbreviatedFormatDerivedOptions>() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<AimingMarkColor>() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<AttributeDesignation>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<BarcodeLabelSize>() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<CalculationVariableType>() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<COFTypeOptions>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<CommandAutomationSubject>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<CompetitionType>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<DefinitionType>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<DisciplineType>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<DisplayEventOptions>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<EventCalculation>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<EventDerivationType>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<EventtType>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<FieldType>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<LightIllumination>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<LinkToOption>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<Months>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ParticipantRemark>() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<RangeScriptType>() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<RemarkVisibility>() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ResultEngineCompareType>() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ResultFieldMethod>() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ScoreComponent>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ScoringShape>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ScoringSystem>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ShotMappingMethodType>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ShowWhenBoolean>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ShowWhenCondition>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ShowWhenOperation>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<SingularType>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<SortBy>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<TieBreakingRuleMethod>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<TieBreakingRuleParticipantAttributeSource>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<TieBreakingRuleScoreSource>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<TimerCommandOptions>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<DataModel.Definitions.ValueType>() );

                    //Common Enums
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<VisibilityOption>() );

                    //Club Enums
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ClubLicenseCapability>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ClubLicenseType>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ClubOptions>() );

                    //Match Enums
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<LeagueRankingRuleType>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<LeagueSeasonType>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<LeagueVirtualType>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<MatchAuthorizationRole>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<MatchParticipantRole>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<MatchTypeOptions>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ResultStatus>() );
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<SquaddingAssignmentType>() );

					//Other Enums
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<APIStage>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<APISubDomain>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ClubLicenseCapability>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<SocialRelationshipName>() );
                    SystemTextJsonDeserializer.Converters.Add(new G_BF_STJ_CONV.EnumConverterByDescription<ApplicationName>());
                    SystemTextJsonDeserializer.Converters.Add(new G_BF_STJ_CONV.EnumConverterByDescription<ReleasePhase>());

                    //AbstractEST
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<DisplayEntityType>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ESTUnitCommand>() );
                    SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ReplaceVariableOptions>() ); 
					SystemTextJsonDeserializer.Converters.Add( new G_BF_STJ_CONV.EnumConverterByDescription<ViewDefinitionShotDisplay>() );
                    SystemTextJsonDeserializer.Converters.Add(new G_BF_STJ_CONV.EnumConverterByDescription<PaintGraphic>());
                    SystemTextJsonDeserializer.Converters.Add(new G_BF_STJ_CONV.EnumConverterByDescription<NeedsDisplayOptions>());



                }
            }
        }

        private static void InitNewtonsoftJsonDeserializer() {
            NewtonsoftJsonSerializer = new G_NS.JsonSerializerSettings();

            NewtonsoftJsonSerializer.TypeNameHandling = G_NS.TypeNameHandling.None;
            NewtonsoftJsonSerializer.NullValueHandling = G_NS.NullValueHandling.Ignore;
            NewtonsoftJsonSerializer.Formatting = G_NS.Formatting.Indented;
            NewtonsoftJsonSerializer.DefaultValueHandling = G_NS.DefaultValueHandling.Ignore;

            //Lets enums get deserizlized from their stirng or description representation.
            NewtonsoftJsonSerializer.Converters.Add( new G_NS_CONV.StringEnumConverter() );
        }
    }
}
