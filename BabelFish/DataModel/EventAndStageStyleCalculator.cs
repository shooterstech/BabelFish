using Newtonsoft.Json.Linq;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.DataModel {
    public class EventAndStageStyleMappingCalculation {

        private EventAndStageStyleMapping _definition;
        private List<EventAndStageStyleMappingObj> _mappings;
        private EventAndStageStyleMappingObj _defaultMapping;
        public EventAndStageStyleMappingCalculation(EventAndStageStyleMapping eventAndStageStyleMappingDefinition) {
            _definition = eventAndStageStyleMappingDefinition;
            _mappings = _definition.Mappings;
            _defaultMapping = _definition.DefaultMapping;
        }

        public string GetEventStyleDef(string attributeValueAppellation, string targetCollectionName, EventStyleMapping eventStyleMapping) {
            // COF defaultDefault Def
            // this is where I am getting eventAppellation FROM, so it will always match (duh) and it has a default value already.
            string eventStyleMappingToReturn = eventStyleMapping.DefaultDef;
            /*
            we DONT want to do this to try and get the most specific default,
            then most specific definition
            if eventAppellation == "":
                return eventStyleMappingToReturn
            */
            // event appellation matching something in the defaultMapping
            if (eventStyleMapping.DefaultDef == "v1.0:orion:Default") {
                foreach (EventStyleSelection eventSMapping in _defaultMapping.EventStyleMappings) {
                    if (eventStyleMapping.EventAppellation == eventSMapping.EventAppellation) {
                        eventStyleMappingToReturn = eventSMapping.EventStyleDef;
                    }
                }
            }

            // actually searching the Mappings to find a definition.
            foreach (EventAndStageStyleMappingObj mapping in _mappings) {
                // if attributeValue and TargetCollection both match, then we are in the right spot.
                if (mapping.AttributeValueAppellation.Contains(attributeValueAppellation) && mapping.TargetCollectionName.Contains(targetCollectionName)) {
                    if (eventStyleMapping.DefaultDef == "v1.0:orion:Default") {
                        //should only happen if the DefaultDef was not given. this is the general "DefaultEventStyleDef" in mapping files under a specific target and attrib
                        eventStyleMappingToReturn = mapping.DefaultEventStyleDef;
                    }
                    foreach (EventStyleSelection eventSMapping in mapping.EventStyleMappings) {
                        if ( eventSMapping.EventAppellation == eventStyleMapping.EventAppellation ) {
                            // if event appellation matches, set definition to that. this is most specific
                            eventStyleMappingToReturn = eventSMapping.EventStyleDef;
                        }
                    }
                }
            }
            return eventStyleMappingToReturn;
        }

        public string GetStageStyleDef(string attributeValueAppellation, string targetCollectionName, StageStyleMapping stageStyleMapping) {
            // COF defaultDefault Def
            // this is where I am getting eventAppellation FROM, so it will always match (duh)
            string stageStyleMappingToReturn = stageStyleMapping.DefaultDef;

            // stage appellation matching something in the defaultMapping
            // This needs to be looked at closely. I think it should be an EventStyleMapping, as that contains DefaultDef....
            if (stageStyleMapping.DefaultDef == "v1.0:orion:Default") {
                foreach (StageStyleSelection stageSMapping in _defaultMapping.StageStyleMappings) {
                    if (stageStyleMapping.StageAppellation == stageSMapping.StageAppellation) {
                        stageStyleMappingToReturn = stageSMapping.StageStyleDef;
                    }
                }
            }

            // actually searching the Mappings to find a definition.
            foreach (EventAndStageStyleMappingObj mapping in _mappings) {
                // if attributeValue and TargetCollection both match, then we are in the right spot.
                if (mapping.AttributeValueAppellation.Contains(attributeValueAppellation) && mapping.TargetCollectionName.Contains(targetCollectionName)) {
                    if (stageStyleMapping.DefaultDef == "v1.0:orion:Default") {
                        stageStyleMappingToReturn = mapping.DefaultStageStyleDef;
                    }
                    foreach (StageStyleSelection stageSMapping in mapping.StageStyleMappings) {
                        if (stageSMapping.StageAppellation == stageStyleMapping.StageAppellation ) {
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

