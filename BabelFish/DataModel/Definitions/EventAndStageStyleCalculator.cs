using Newtonsoft.Json.Linq;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {

    //NOTE: Not sure if Scopos.BabelFish.DataModel.Definitions is the best namespace for this file, since it is not an object for a definition.
    //However, ca'nt figure out a better place for it current. EKA July 2023
    public class EventAndStageStyleMappingCalculation {

        private EventAndStageStyleMapping _definition;
        private List<EventAndStageStyleMappingObj> _mappings;
        private EventAndStageStyleMappingObj _defaultMapping;
        public EventAndStageStyleMappingCalculation( EventAndStageStyleMapping eventAndStageStyleMappingDefinition ) {
            _definition = eventAndStageStyleMappingDefinition;
            _mappings = _definition.Mappings;
            _defaultMapping = _definition.DefaultMapping;
        }

        /// <summary>
        /// Returns the SetName (as a string) of the EventStyle definition that cooresponds to the passed in attributeValueAppellation, targetCollectionName, and eventStyleMapping.
        /// </summary>
        /// <param name="attributeValueAppellation"></param>
        /// <param name="targetCollectionName"></param>
        /// <param name="eventStyleMapping"></param>
        /// <returns></returns>
        public string GetEventStyleDef( string attributeValueAppellation, string targetCollectionName, EventStyleMapping eventStyleMapping ) {
            // this is the worst case, nothing else matches circumstance
            string eventStyleMappingToReturn = "v1.0:orion:Default";

            //Next to the worst case, take the value from .DefaultDef if it exists
            if (!string.IsNullOrEmpty( eventStyleMapping.DefaultDef ))
                eventStyleMappingToReturn = eventStyleMapping.DefaultDef;

            //Next, take the default value from the Mapping definition file
            if (!string.IsNullOrEmpty( _defaultMapping.DefaultEventStyleDef ))
                eventStyleMappingToReturn = _defaultMapping.DefaultEventStyleDef;

            //Next, try and match the EventAppellation from the default mappings.
            foreach (EventStyleSelection eventSMapping in _defaultMapping.EventStyleMappings) {
                if (eventStyleMapping.EventAppellation == eventSMapping.EventAppellation) {
                    eventStyleMappingToReturn = eventSMapping.EventStyleDef;
                }
            }

            //Now we move on to the mappings specific to AttributeValueAppellation and TargetCollectionName

            // actually searching the Mappings to find a definition.
            foreach (EventAndStageStyleMappingObj mapping in _mappings) {
                // if attributeValue and TargetCollection both match, then we are in the right spot.
                if (mapping.AttributeValueAppellation.Contains( attributeValueAppellation ) && mapping.TargetCollectionName.Contains( targetCollectionName )) {

                    //should only happen if the DefaultDef was not given. this is the general "DefaultEventStyleDef" in mapping files under a specific target and attrib
                    eventStyleMappingToReturn = mapping.DefaultEventStyleDef;
                    foreach (EventStyleSelection eventSMapping in mapping.EventStyleMappings) {
                        if (eventSMapping.EventAppellation == eventStyleMapping.EventAppellation) {
                            // if event appellation matches, set definition to that. this is most specific
                            eventStyleMappingToReturn = eventSMapping.EventStyleDef;
                        }
                    }
                }
            }
            return eventStyleMappingToReturn;
        }

        /// <summary>
        /// Returns the SetName (as a string) of the StageStyle definition that cooresponds to the passed in attributeValueAppellation, targetCollectionName, and eventStyleMapping.
        /// </summary>
        /// <param name="attributeValueAppellation"></param>
        /// <param name="targetCollectionName"></param>
        /// <param name="stageStyleMapping"></param>
        /// <returns></returns>
        public string GetStageStyleDef( string attributeValueAppellation, string targetCollectionName, StageStyleMapping stageStyleMapping ) {
            // this is the worst case, nothing else matches circumstance
            string stageStyleMappingToReturn = "v1.0:orion:Default";

            //Next to the worst case, take the value from .DefaultDef if it exists
            if (!string.IsNullOrEmpty( stageStyleMapping.DefaultDef ))
                stageStyleMappingToReturn = stageStyleMapping.DefaultDef;

            //Next, take the default value from the Mapping definition file
            if (!string.IsNullOrEmpty( _defaultMapping.DefaultStageStyleDef ))
                stageStyleMappingToReturn = _defaultMapping.DefaultStageStyleDef;

            // stage appellation matching something in the defaultMapping
            // This needs to be looked at closely. I think it should be an EventStyleMapping, as that contains DefaultDef....

            foreach (StageStyleSelection stageSMapping in _defaultMapping.StageStyleMappings) {
                if (stageStyleMapping.StageAppellation == stageSMapping.StageAppellation) {
                    stageStyleMappingToReturn = stageSMapping.StageStyleDef;
                }
            }

            // actually searching the Mappings to find a definition.
            foreach (EventAndStageStyleMappingObj mapping in _mappings) {
                // if attributeValue and TargetCollection both match, then we are in the right spot.
                if (mapping.AttributeValueAppellation.Contains( attributeValueAppellation ) && mapping.TargetCollectionName.Contains( targetCollectionName )) {

                    stageStyleMappingToReturn = mapping.DefaultStageStyleDef;
                    foreach (StageStyleSelection stageSMapping in mapping.StageStyleMappings) {
                        if (stageSMapping.StageAppellation == stageStyleMapping.StageAppellation) {
                            // if stage appellation matches, set definition to that. this is most specific
                            stageStyleMappingToReturn = stageSMapping.StageStyleDef;
                        }
                    }
                }
            }
            return stageStyleMappingToReturn;
        }
    }
}

