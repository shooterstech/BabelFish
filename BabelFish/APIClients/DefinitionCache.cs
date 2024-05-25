using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.APIClients {
    public static class DefinitionCache {

        private static Dictionary<SetName, CourseOfFire> CourseOfFireCache = new Dictionary<SetName, CourseOfFire> ();

        private static Dictionary<SetName, EventAndStageStyleMapping> EventAndStageStyleMappingCache = new Dictionary<SetName, EventAndStageStyleMapping>();

        public static async Task<CourseOfFire> GetCourseOfFireDefinitionAsync( SetName setName ) {
            if ( CourseOfFireCache.TryGetValue( setName, out CourseOfFire c ) ) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetCourseOfFireDefinitionAsync( setName );
            var definition = response.Definition;

            CourseOfFireCache.Add( setName, definition );
            return definition;
        }

        public static async Task<EventAndStageStyleMapping> GetEventAndStageStyleMappingDefinitionAsync( SetName setName ) {
            if (EventAndStageStyleMappingCache.TryGetValue( setName, out EventAndStageStyleMapping c )) { return c; }

            var response = await DefinitionFetcher.FETCHER.GetEventAndStageStyleMappingDefinitionAsync( setName );
            var definition = response.Definition;

            EventAndStageStyleMappingCache.Add( setName, definition );
            return definition;
        }
    }
}
