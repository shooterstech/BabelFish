using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Definition {
    public static class CourseOfFireHelper {

        public static CourseOfFire Get_3x20_KPS_Cof() {

            CourseOfFire cof = new CourseOfFire();
            cof.Singulars.Add( new Singular() {
                EventName = "K{}",
                StageLabel = "K",
                Values = new ValueSeries( "1..20" )
            } );
            cof.Singulars.Add( new Singular() {
                EventName = "P{}",
                StageLabel = "P",
                Values = new ValueSeries( "1..20" )
            } );
            cof.Singulars.Add( new Singular() {
                EventName = "S{}",
                StageLabel = "S",
                Values = new ValueSeries( "1..20" )
            } );

            cof.Events.Add( new EventExplicit() {
                EventName = "Qualification",
                EventType = EventtType.EVENT,
                Children = new List<string>() { "Kneeling", "Prone", "Standing" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "Kneeling",
                EventType = EventtType.STAGE,
                Children = new List<string>() { "KN 1", "KN 2" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "Prone",
                EventType = EventtType.STAGE,
                Children = new List<string>() { "PR 1", "PR 2" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "Standing",
                EventType = EventtType.STAGE,
                Children = new List<string>() { "ST 1", "ST 2" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "KN 1",
                EventType = EventtType.STRING,
                Children = new List<string>() { "K1", "K2", "K3", "K4", "K5", "K6", "K7", "K8", "K9", "K10" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "KN 2",
                EventType = EventtType.STRING,
                Children = new List<string>() { "K11", "K12", "K13", "K14", "K15", "K16", "K17", "K18", "K19", "K20" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "PR 1",
                EventType = EventtType.STRING,
                Children = new List<string>() { "P1", "P2", "P3", "P4", "P5", "P6", "P7", "P8", "P9", "P10" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "PR 2",
                EventType = EventtType.STRING,
                Children = new List<string>() { "P11", "P12", "P13", "P14", "P15", "P16", "P17", "P18", "P19", "P20" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "ST 1",
                EventType = EventtType.STRING,
                Children = new List<string>() { "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "S10" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "ST 2",
                EventType = EventtType.STRING,
                Children = new List<string>() { "S11", "S12", "S13", "S14", "S15", "S16", "S17", "S18", "S19", "S20" }
            } );

            return cof;
        }

        public static CourseOfFire Get_60_Standing_Cof() {

            CourseOfFire cof = new CourseOfFire();
            cof.Singulars.Add( new Singular() {
                EventName = "S{}",
                StageLabel = "S",
                Values = new ValueSeries( "1..60" )
            } );

            cof.Events.Add( new EventExplicit() {
                EventName = "Qualification",
                EventType = EventtType.EVENT,
                Children = new List<string>() { "Standing" }
            } );

            cof.Events.Add( new EventExplicit() {
                EventName = "Standing",
                EventType = EventtType.STAGE,
                Children = new List<string>() { "ST 1", "ST 2", "ST 3", "ST 4", "ST 5", "ST 6" }
            } );

            int singularIndex = 1;
            for ( int stringIndex = 1; stringIndex <= 6; stringIndex++) {

                cof.Events.Add( new EventExplicit() {
                    EventName = $"ST {stringIndex}",
                    EventType = EventtType.STRING,
                    Children = new List<string>() { 
                        $"S{singularIndex++}",
                        $"S{singularIndex++}",
                        $"S{singularIndex++}",
                        $"S{singularIndex++}",
                        $"S{singularIndex++}",
                        $"S{singularIndex++}",
                        $"S{singularIndex++}",
                        $"S{singularIndex++}",
                        $"S{singularIndex++}",
                        $"S{singularIndex++}"
                    }
                } ) ;
            }

            return cof;
        }
        public static CourseOfFire Get_4x10_Cof() {

            CourseOfFire cof = new CourseOfFire();
            cof.Singulars.Add( new Singular() {
                EventName = "K{}",
                StageLabel = "K",
                Values = new ValueSeries( "1..10" )
            } );
            cof.Singulars.Add( new Singular() {
                EventName = "P{}",
                StageLabel = "P",
                Values = new ValueSeries( "1..10" )
            } );
            cof.Singulars.Add( new Singular() {
                EventName = "S{}",
                StageLabel = "S",
                Values = new ValueSeries( "1..10" )
            } );
            cof.Singulars.Add( new Singular() {
                EventName = "I{}",
                StageLabel = "I",
                Values = new ValueSeries( "1..10" )
            } );

            cof.Events.Add( new EventExplicit() {
                EventName = "Qualification",
                EventType = EventtType.EVENT,
                Children = new List<string>() { "Prone", "Standing", "Sitting", "Kneeling" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "Kneeling",
                EventType = EventtType.STAGE,
                Children = new List<string>() { "KN 1" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "Prone",
                EventType = EventtType.STAGE,
                Children = new List<string>() { "PR 1" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "Standing",
                EventType = EventtType.STAGE,
                Children = new List<string>() { "ST 1" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "Sitting",
                EventType = EventtType.STAGE,
                Children = new List<string>() { "SI 1" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "KN 1",
                EventType = EventtType.STRING,
                Children = new List<string>() { "K1", "K2", "K3", "K4", "K5", "K6", "K7", "K8", "K9", "K10" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "PR 1",
                EventType = EventtType.STRING,
                Children = new List<string>() { "P1", "P2", "P3", "P4", "P5", "P6", "P7", "P8", "P9", "P10" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "ST 1",
                EventType = EventtType.STRING,
                Children = new List<string>() { "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "S10" }
            } );
            cof.Events.Add( new EventExplicit() {
                EventName = "SI 1",
                EventType = EventtType.STRING,
                Children = new List<string>() { "I1", "I2", "I3", "I4", "I5", "I6", "I7", "I8", "I9", "I10" }
            } );

            return cof;
        }

        public static CourseOfFire Get_Informal_Air_Rifle() {

            CourseOfFire cof = new CourseOfFire();

            /* EVENT */
            cof.Events.Add( new EventExplicit() {
                EventName = "Top Level",
                EventType = EventtType.EVENT,
                ScoreFormat = "Events",
                Children = new List<string>() {
                    "Kneeling",
                    "Prone",
                    "Standing"
                    },
                EventStyleMapping = new EventStyleMapping() {
                    EventAppellation = "Qualification3P",
                    DefaultDef = "v1.0:ntparc:Three-Position Precision Air Rifle"
                }
            } );
            cof.Events[0].RankingRuleMapping["DefaultDef"] = "v2.0:ntparc:Three-Position Air Rifle Qualification Decimal";
            cof.Events[0].RankingRuleMapping["Integer"] = "v2.0:ntparc:Three-Position Air Rifle Qualification";

            /* STAGES */
            cof.Events.Add( new EventDerived() {
                EventName = "Prone",
                EventType = EventtType.STAGE,
                ScoreFormat = "Events",
                ChildEventName = "PR {}",
                ChildValues = new ValueSeries( "1..50" ),
                StageStyleMapping = new StageStyleMapping() {
                    StageAppellation = "Prone",
                    DefaultDef = "v1.0:ntparc:Precision Air Rifle Prone"
                }
            } );
            cof.Events[0].RankingRuleMapping["DefaultDef"] = "v1.0:orion:Generic Decimal Prone";
            cof.Events[0].RankingRuleMapping["Integer"] = "v1.0:ntparc:Prone Position";

            cof.Events.Add( new EventDerived() {
                EventName = "Standing",
                EventType = EventtType.STAGE,
                ScoreFormat = "Events",
                ChildEventName = "ST {}",
                ChildValues = new ValueSeries( "1..50" ),
                StageStyleMapping = new StageStyleMapping() {
                    StageAppellation = "Standing",
                    DefaultDef = "v1.0:ntparc:Precision Air Rifle Standing"
                }
            } );
            cof.Events[0].RankingRuleMapping["DefaultDef"] = "v1.0:orion:Generic Decimal Standing";
            cof.Events[0].RankingRuleMapping["Integer"] = "v1.0:ntparc:Standing Position";

            cof.Events.Add( new EventDerived() {
                EventName = "Kneeling",
                EventType = EventtType.STAGE,
                ScoreFormat = "Events",
                ChildEventName = "KN {}",
                ChildValues = new ValueSeries( "1..50" ),
                StageStyleMapping = new StageStyleMapping() {
                    StageAppellation = "Kneeling",
                    DefaultDef = "v1.0:ntparc:Precision Air Rifle Kneeling"
                }
            } );
            cof.Events[0].RankingRuleMapping["DefaultDef"] = "v1.0:orion:Generic Decimal Kneeling";
            cof.Events[0].RankingRuleMapping["Integer"] = "v1.0:ntparc:Kneeling Position";

            /* STRINGS */
            cof.Events.Add( new EventExpand() {
                EventName = "PR {}",
                EventType = EventtType.STRING,
                ScoreFormat = "Events",
                Values = new ValueSeries( "1..50" ),
                ChildEventName = "P{}",
                ChildStringSize = 10
            } );

            cof.Events.Add( new EventExpand() {
                EventName = "ST {}",
                EventType = EventtType.STRING,
                ScoreFormat = "Events",
                Values = new ValueSeries( "1..50" ),
                ChildEventName = "S{}",
                ChildStringSize = 10
            } );

            cof.Events.Add( new EventExpand() {
                EventName = "KN {}",
                EventType = EventtType.STRING,
                ScoreFormat = "Events",
                Values = new ValueSeries( "1..50" ),
                ChildEventName = "K{}",
                ChildStringSize = 10
            } );

            /* SHOTS */
            cof.Singulars.Add( new Singular() {
                EventName = "K{}",
                StageLabel = "K",
                ScoreFormat = "Shots",
                Values = new ValueSeries( "1..500" ),
                ShotMappingMethod = ShotMappingMethodType.SEQUENTIAL
            } );
            cof.Singulars.Add( new Singular() {
                EventName = "P{}",
                StageLabel = "P",
                ScoreFormat = "Shots",
                Values = new ValueSeries( "1..500" ),
                ShotMappingMethod = ShotMappingMethodType.SEQUENTIAL
            } );
            cof.Singulars.Add( new Singular() {
                EventName = "S{}",
                StageLabel = "S",
                ScoreFormat = "Shots",
                Values = new ValueSeries( "1..500" ),
                ShotMappingMethod = ShotMappingMethodType.SEQUENTIAL
            } );

            /* EXTERNAL */
            cof.Events.Add( new EventDerived() {
                EventName = "PR AVG",
                Calculation = EventCalculation.AVG_TEN,
                ScoreFormat = "Events",
                ChildValues = new ValueSeries( "1..500" ),
                ChildEventName = "P{}",
                ExternalToEventTree = true
            } );
            cof.Events.Add( new EventDerived() {
                EventName = "ST AVG",
                Calculation = EventCalculation.AVG_TEN,
                ScoreFormat = "Events",
                ChildValues = new ValueSeries( "1..500" ),
                ChildEventName = "S{}",
                ExternalToEventTree = true
            } );
            cof.Events.Add( new EventDerived() {
                EventName = "KN AVG",
                Calculation = EventCalculation.AVG_TEN,
                ScoreFormat = "Events",
                ChildValues = new ValueSeries( "1..500" ),
                ChildEventName = "K{}",
                ExternalToEventTree = true
            } );

            return cof;
        }

    }
}
