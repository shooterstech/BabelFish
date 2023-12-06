using Newtonsoft.Json.Linq;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft;
using NLog;

namespace Scopos.BabelFish.DataModel.Definitions {

    //NOTE: Not sure if Scopos.BabelFish.DataModel.Definitions is the best namespace for this file, since it is not an object for a definition.
    //However, ca'nt figure out a better place for it current. EKA July 2023
    public class EventAndStageStyleMappingCalculation {

        private EventAndStageStyleMapping _definition;
        private List<EventAndStageStyleMappingObj> _mappings;
        private EventAndStageStyleMappingObj _defaultMapping;
        private Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private const string DEFAULT_DEF = "v1.0:orion:Default";

        public EventAndStageStyleMappingCalculation( EventAndStageStyleMapping eventAndStageStyleMappingDefinition ) {
            _definition = eventAndStageStyleMappingDefinition;
            if (_definition != null) {
                _mappings = _definition.Mappings;
                _defaultMapping = _definition.DefaultMapping;
            } else {
                logger.Warn( $"Parameter eventAndStageStyleMappingDefinition is null. Likely the medea concrete rulebook, or a course of fire definition file, is referencing a mapping definitions incorrectly." );
                _mappings = new List<EventAndStageStyleMappingObj>();
                _defaultMapping = new EventAndStageStyleMappingObj();
            }
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
            string eventStyleMappingToReturn = DEFAULT_DEF;

            //Next to the worst case, take the value from .DefaultDef if it exists
            if (!string.IsNullOrEmpty( eventStyleMapping.DefaultDef ) && eventStyleMapping.DefaultDef != DEFAULT_DEF)
                eventStyleMappingToReturn = eventStyleMapping.DefaultDef;

            //Next, take the default value from the Mapping definition file
            if (!string.IsNullOrEmpty( _defaultMapping.DefaultEventStyleDef ) && _defaultMapping.DefaultEventStyleDef != DEFAULT_DEF)
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
				// if .AttributeValueAppellation is empty or .TargetCollectionName is empty then that serves as a wild card that matches everything
				if ((mapping.AttributeValueAppellation.Contains( attributeValueAppellation ) || mapping.AttributeValueAppellation.Count == 0)
                    && (mapping.TargetCollectionName.Contains( targetCollectionName ) || mapping.TargetCollectionName.Count == 0)) {

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
            string stageStyleMappingToReturn = DEFAULT_DEF;

            //Next to the worst case, take the value from .DefaultDef if it exists
            if (!string.IsNullOrEmpty( stageStyleMapping.DefaultDef ) && stageStyleMapping.DefaultDef != DEFAULT_DEF)
                stageStyleMappingToReturn = stageStyleMapping.DefaultDef;

            //Next, take the default value from the Mapping definition file
            if (!string.IsNullOrEmpty( _defaultMapping.DefaultStageStyleDef ) && _defaultMapping.DefaultStageStyleDef != DEFAULT_DEF)
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
                // if .AttributeValueAppellation is empty or .TargetCollectionName is empty then that serves as a wild card that matches everything
				if ((mapping.AttributeValueAppellation.Contains( attributeValueAppellation ) || mapping.AttributeValueAppellation.Count == 0)
					&& (mapping.TargetCollectionName.Contains( targetCollectionName ) || mapping.TargetCollectionName.Count == 0)) {

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

