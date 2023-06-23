using Newtonsoft.Json.Linq;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BabelFish.DataModel {
    public class EventAndStageStyleMappingCalculation {

        private JObject _definition;
        private JObject _mappings;
        private JObject _defaultMapping;
        public EventAndStageStyleMappingCalculation(string eventAndStageStyleMappingDefinition) {
            _definition = JObject.Parse(eventAndStageStyleMappingDefinition);
            if (_definition.ContainsKey("Mappings") && _definition.ContainsKey("DefaultMapping")) {
                // may need to deal with null assignment on these.
                _mappings = _definition["Mappings"] as JObject; 
                _defaultMapping = _definition["DefaultMapping"] as JObject;
            }
        }

        public string GetEventStyleDef(string attributeValueAppellation = "", string targetCollectionName = "", string eventAppellation = "", string lastLineOfDefenseDefinition = "") {
            JObject lastLineOfDefenseDefinitionJson = JObject.Parse(lastLineOfDefenseDefinition);
            //Default to known Orion Default
            string eventStyleMappingToReturn = "v1.0:orion:default";
            //Default to mapping file defaultMapping DefaultDef
            if (_defaultMapping.ContainsKey("DefaultEventStyleDef")) {
                eventStyleMappingToReturn = (string)_defaultMapping["DefaultEventStyleDef"];
            }
            // COF defaultDefault Def
            if (lastLineOfDefenseDefinitionJson.ContainsKey("EventStyleMapping") && ((JObject)lastLineOfDefenseDefinitionJson["EventStyleMapping"]).ContainsKey("DefaultDef")){
                // this is where I am getting eventAppellation FROM, so it will always match (duh)
                eventStyleMappingToReturn = (string)lastLineOfDefenseDefinitionJson["EventStyleMapping"]["DefaultDef"];
            }
            /*
            we DONT want to do this to try and get the most specific default,
            then most specific definition
            if eventAppellation == "":
                return eventStyleMappingToReturn
            */
            // event appellation matching something in the defaultMapping
            if (_defaultMapping.ContainsKey("EventStyleMappings")) {
                foreach (JObject eventStyleMapping in _defaultMapping["EventStyleMappings"]) {
                    if (eventStyleMapping.ContainsKey("EventAppellation") && eventStyleMapping.ContainsKey("EventStyleDef") && string.Compare(eventAppellation, (string)eventStyleMapping["EventAppellation"]) == 0) {
                    eventStyleMappingToReturn = (string)eventStyleMapping["EventStyleDef"];
                }
            }

            // actually searching the Mappings to find a definition.
            foreach (JObject mapping in _mappings) {
                // if attributeValue and TargetCollection both match, then we are in the right spot.
                if "AttributeValueAppellation" in mapping and "TargetCollectionName" in mapping and attributeValueAppellation in mapping["AttributeValueAppellation"] and targetCollectionName in mapping["TargetCollectionName"]){
                    if "DefaultEventStyleDef" in mapping){
                    // default in the specific mapping default
                            eventStyleMappingToReturn = mapping["DefaultEventStyleDef"]
                    }
                }
                for (eventStyleMapping in mapping["EventStyleMappings"]) {
                    if "EventAppellation" in eventStyleMapping and "EventStyleDef" in eventStyleMapping and eventStyleMapping["EventAppellation"] == eventAppellation) {
                    // if event appellation matches, set definition to that. this is most specific
                        eventStyleMappingToReturn = eventStyleMapping["EventStyleDef"]
                        }
                    }
                }
            }
            return eventStyleMappingToReturn
        }
    }
}
