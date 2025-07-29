using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {

    public class ResultListFormatFactory {

        public static ResultListFormatFactory FACTORY = new ResultListFormatFactory();

        private ResultListFormatFactory() { }

        /// <summary>
        /// Returns the SetName of the ResultListFormat Definition to use given the passed in ResultList.
        /// Updated ResultLists contain a field ".ResultListFormatDef" that specifies which RESULT LIST FORMAT 
        /// definition to use. If one is listed, then it is returned. 
        /// 
        /// On older ResultList, with out a .ResultIstFromatDef listed, this method first tries and look up the default
        /// RESULT LIST FORMAT definition to use based on the COF definition. Again, if one is not listed, then
        /// a this method tries and predicts one.
        /// </summary>
        /// <param name="courseOfFireDef"></param>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public async Task<SetName> GetResultListFormatSetNameAsync( ResultList resultList) {

            if ( resultList == null )
                return GetDefaultByEventName( "Qualification" );

            SetName resultListFormatSetName;

            //First priority, check if the ResultList includes a Result List Format definition to use. If it does, return it.
            if (!string.IsNullOrEmpty( resultList.ResultListFormatDef)) {
                if (SetName.TryParse( resultList.ResultListFormatDef, out resultListFormatSetName ) ) {
                    return resultListFormatSetName;
                }
            }

            //If we get here the Result List does not have a Ranking List Format definition.
            //Which brings us to our second attempt by reading the COF definition and and matching the event name

            string courseOfFireDef = resultList.CourseOfFireDef;
            string eventName = resultList.EventName;

            var cof = await resultList.GetCourseOfFireDefinitionAsync().ConfigureAwait( false );
            if (cof != null ) {
                foreach ( var e in cof.Events) {
                    if (e.EventName == eventName) {
                        if (!string.IsNullOrEmpty( e.ResultListFormatDef ) && SetName.TryParse( e.ResultListFormatDef, out resultListFormatSetName )) {
                            return resultListFormatSetName;
                        }
                        break;
                    }
                }
            }

            //Third attempt, and we have to try and find one that will match it magically

            switch (courseOfFireDef) {
                //P-S-K Format
                case "v1.0:ntparc:Three-Position Air Rifle 3x10":
                case "v1.0:ntparc:Three-Position Air Rifle 3x20":
                case "v1.0:ntparc:Three-Position Air Rifle 3x10 plus Final":
                case "v1.0:ntparc:Three-Position Air Rifle 3x20 plus Final":
                case "v2.0:ntparc:Three-Position Air Rifle 3x10":
                case "v2.0:ntparc:Three-Position Air Rifle 3x20":
                case "v2.0:ntparc:Three-Position Air Rifle 3x10 plus Final":
                case "v2.0:ntparc:Three-Position Air Rifle 3x20 plus Final":
                case "v1.0:nra:Air Rifle 3x20":
                case "v2.0:nra:Air Rifle 3x10":

                    switch (eventName) {
                        case "Qualification":
                            return SetName.Parse( "v1.0:orion:3P Qualification" );
                        case "Individual":
                            return SetName.Parse( "v1.0:orion:3P Individual" );
                        case "Team":
                            return SetName.Parse( "v1.0:orion:3P Team" );
                        default:
                            return GetDefaultByEventName( eventName );
                    }

                //K-P-S Format
                case "v3.0:ntparc:Three-Position Air Rifle 3x10":
                case "v3.0:ntparc:Three-Position Air Rifle 3x20":
                case "v3.0:ntparc:Three-Position Air Rifle 3x40":
                case "v1.0:cmp:Smallbore Rifle 3x10":
                case "v1.0:cmp:Smallbore Rifle 3x20":
                case "v1.0:cmp:Smallbore Rifle 3x40":
                case "v1.0:usas:Smallbore Rifle 3x10":
                case "v1.0:usas:Smallbore Rifle 3x20":
                case "v1.0:usas:Smallbore Rifle 3x40":

                    switch (eventName) {
                        case "Qualification":
                            return SetName.Parse( "v2.0:orion:3P Qualification" );
                        case "Individual":
                            return SetName.Parse( "v2.0:orion:3P Individual" );
                        case "Team":
                            return SetName.Parse( "v2.0:orion:3P Team" );
                        default:
                            return GetDefaultByEventName( eventName );
                    }

                case "v1.0:nra:BB Gun 4x10":

                    switch (eventName) {
                        case "Qualification":
                            return SetName.Parse( "v1.0:orion:BB Gun 4x10 Qualification" );
                        case "Individual":
                            return SetName.Parse( "v1.0:orion:BB Gun 4x10 Individual" );
                        case "Team":
                            return SetName.Parse( "v1.0:orion:BB Gun 4x10 Team" );
                        default:
                            return GetDefaultByEventName( eventName );
                    }

                case "v1.0:nra:BB Gun 4x10 with Test":

                    switch (eventName) {
                        case "Qualification":
                            return SetName.Parse( "v1.0:orion:BB Gun 4x10 with Test Qualification" );
                        case "Individual":
                            return SetName.Parse( "v1.0:orion:BB Gun 4x10 with Test Individual" );
                        case "Team":
                            return SetName.Parse( "v1.0:orion:BB Gun 4x10 with Test Team" );
                        default:
                            return GetDefaultByEventName( eventName );
                    }

                case "v2.0:usas:Air Pistol Qualification 60 Shots":
                case "v1.0:usas:Air Pistol Qualification 60 Shots":

                    switch (eventName) {
                        case "Qualification":
                            return SetName.Parse( "v1.0:orion:Standing Qualification" );
                        case "Individual":
                            return SetName.Parse( "v1.0:orion:Standing Individual" );
                        default:
                            return GetDefaultByEventName( eventName );
                    }

                case "v1.0:cmp:National Match Course Pistol":
                case "v1.0:cmp:Presidents Pistol Course":
                case "v1.0:cmp:Training":
                case "v1.0:nra:National Match Course Pistol":
                case "v1.0:nra:Training":

                    switch (eventName) {
                        case "Individual":
                            return SetName.Parse( "v1.0:cmp:National Match Course Pistol Individual" );
                        case "Team":
                            return SetName.Parse( "v1.0:cmp:National Match Course Pistol Team" );
                        default:
                            return GetDefaultByEventName( eventName );
                    }

                case "v1.0:cmp:900 Aggregate":
                case "v1.0:nra:900 Aggregate":

                    switch (eventName) {
                        case "Individual":
                            return SetName.Parse( "v1.0:cmp:900 Aggregate Individual" );
                        case "Team":
                            return SetName.Parse( "v1.0:cmp:900 Aggregate Team" );
                        default:
                            return GetDefaultByEventName( eventName );
                    }
            }


            return GetDefaultByEventName( eventName );


        }

        private SetName GetDefaultByEventName( string eventName ) {
            switch( eventName ) {
                case "Qualification":
                    return SetName.Parse( $"v1.0:orion:Default Qualification" );
                case "Individual":
                    return SetName.Parse( $"v1.0:orion:Default Individual" );
                case "Team":
                    return SetName.Parse( $"v1.0:orion:Default Team" );
            }

            return SetName.Parse( $"v1.0:orion:{eventName} Individual" );
        }

    }
}
