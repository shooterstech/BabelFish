using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.Tests.Definition {
    public static class CourseOfFireHelper {

        public static CourseOfFire Get_3x20_KPS_Cof() {

            CourseOfFire cof = new CourseOfFire();
            cof.Singulars.Add( new Singular() {
                EventName = "K{}",
                StageLabel = "K",
                Values = "1..20"
            } );
            cof.Singulars.Add( new Singular() {
                EventName = "P{}",
                StageLabel = "P",
                Values = "1..20"
            } );
            cof.Singulars.Add( new Singular() {
                EventName = "S{}",
                StageLabel = "S",
                Values = "1..20"
            } );

            cof.Events.Add( new Event() {
                EventName = "Qualification",
                EventType = EventtType.EVENT,
                Children = new[] { "Kneeling", "Prone", "Standing" }
            } );
            cof.Events.Add( new Event() {
                EventName = "Kneeling",
                EventType = EventtType.STAGE,
                Children = new[] { "KN 1", "KN 2" }
            } );
            cof.Events.Add( new Event() {
                EventName = "Prone",
                EventType = EventtType.STAGE,
                Children = new[] { "PR 1", "PR 2" }
            } );
            cof.Events.Add( new Event() {
                EventName = "Standing",
                EventType = EventtType.STAGE,
                Children = new[] { "ST 1", "ST 2" }
            } );
            cof.Events.Add( new Event() {
                EventName = "KN 1",
                EventType = EventtType.STRING,
                Children = new[] { "K1", "K2", "K3", "K4", "K5", "K6", "K7", "K8", "K9", "K10" }
            } );
            cof.Events.Add( new Event() {
                EventName = "KN 2",
                EventType = EventtType.STRING,
                Children = new[] { "K11", "K12", "K13", "K14", "K15", "K16", "K17", "K18", "K19", "K20" }
            } );
            cof.Events.Add( new Event() {
                EventName = "PR 1",
                EventType = EventtType.STRING,
                Children = new[] { "P1", "P2", "P3", "P4", "P5", "P6", "P7", "P8", "P9", "P10" }
            } );
            cof.Events.Add( new Event() {
                EventName = "PR 2",
                EventType = EventtType.STRING,
                Children = new[] { "P11", "P12", "P13", "P14", "P15", "P16", "P17", "P18", "P19", "P20" }
            } );
            cof.Events.Add( new Event() {
                EventName = "ST 1",
                EventType = EventtType.STRING,
                Children = new[] { "S1", "S2", "S3", "S4", "S5", "S6", "S7", "S8", "S9", "S10" }
            } );
            cof.Events.Add( new Event() {
                EventName = "ST 2",
                EventType = EventtType.STRING,
                Children = new[] { "S11", "S12", "S13", "S14", "S15", "S16", "S17", "S18", "S19", "S20" }
            } );

            return cof;
        }

        public static CourseOfFire Get_60_Standing_Cof() {

            CourseOfFire cof = new CourseOfFire();
            cof.Singulars.Add( new Singular() {
                EventName = "S{}",
                StageLabel = "S",
                Values = "1..60"
            } );

            cof.Events.Add( new Event() {
                EventName = "Qualification",
                EventType = EventtType.EVENT,
                Children = new[] { "Standing" }
            } );
            cof.Events.Add( new Event() {
                EventName = "Standing",
                EventType = EventtType.STAGE,
                Children = new[] { "ST 1", "ST 2", "ST 3", "ST 4", "ST 5", "ST 6" }
            } );

            int singularIndex = 1;
            for ( int stringIndex = 1; stringIndex <= 6; stringIndex++) {

                cof.Events.Add( new Event() {
                    EventName = $"ST {stringIndex}",
                    EventType = EventtType.STRING,
                    Children = new[] { 
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
    }
}
