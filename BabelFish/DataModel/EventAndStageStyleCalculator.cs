using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.DataModel {
    public class EventAndStageStyleMappingCalculation {
        public EventAndStageStyleMappingCalculation(eventAndStageStyleMappingDefinition, DefaultEventAndStyleMapping) {
            var _definition = eventAndStageStyleMappingDefinition;
            var _mappings = _definition["Mappings"];
            var _defaultMapping = _definition["DefaultMapping"];
            var _defaultDefinition = DefaultEventAndStyleMapping;
        }

        public GetEventStyleDef(string attributeValueAppellation = "", string targetCollectionName = "", string eventAppellation = "", string lastLineOfDefenseDefinition = "") {
            //Default to known Orion Default
            eventStyleMappingToReturn = self._defaultDefinition["DefaultMapping"]["DefaultEventStyleDef"]
            //Default to mapping file defaultMapping DefaultDef
            if "DefaultEventStyleDef" in self._defaultMapping:
                eventStyleMappingToReturn = self._defaultMapping["DefaultEventStyleDef"]
            // COF defaultDefault Def
            if "EventStyleMapping" in lastLineOfDefenseDefinition and\
                    "DefaultDef" in lastLineOfDefenseDefinition["EventStyleMapping"]: // this is where I am getting eventAppellation FROM, so it will always match (duh)
                eventStyleMappingToReturn = lastLineOfDefenseDefinition["EventStyleMapping"]["DefaultDef"]
            /*
            we DONT want to do this to try and get the most specific default,
            then most specific definition
            if eventAppellation == "":
                return eventStyleMappingToReturn
            */
            // event appellation matching something in the defaultMapping
            if "EventStyleMappings" in self._defaultMapping:
                for eventStyleMapping in self._defaultMapping["EventStyleMappings"]:
                    if "EventAppellation" in eventStyleMapping and "EventStyleDef" in eventStyleMapping and \
                            eventAppellation == eventStyleMapping["EventAppellation"]:
                        eventStyleMappingToReturn = eventStyleMapping["EventStyleDef"]

            // actually searching the Mappings to find a definition.
            for mapping in self._mappings:
                // if attributeValue and TargetCollection both match, then we are in the right spot.
                if "AttributeValueAppellation" in mapping and "TargetCollectionName" in mapping and \
                        attributeValueAppellation in mapping["AttributeValueAppellation"] and \
                        targetCollectionName in mapping["TargetCollectionName"]:
                    if "DefaultEventStyleDef" in mapping:
            // default in the specific mapping default
            eventStyleMappingToReturn = mapping["DefaultEventStyleDef"]
                    for eventStyleMapping in mapping["EventStyleMappings"]:
                        if "EventAppellation" in eventStyleMapping and "EventStyleDef" in eventStyleMapping and\
                                eventStyleMapping["EventAppellation"] == eventAppellation: 
                            // if event appellation matches, set definition to that. this is most specific
                            eventStyleMappingToReturn = eventStyleMapping["EventStyleDef"]

            return eventStyleMappingToReturn
            }

    }
}
