using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scopos.BabelFish.DataActors.Specification.Definitions {
    public class IsCourseOfFireValid : CompositeSpecification<CourseOfFire> {

        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {

            var valid = true;
            Messages.Clear();
            //Clear the EventComposit cache, which may be holding on to out-dated COF structure.
            EventComposite.ClearCache();

            //Common fields
            var hierarchicalName = new IsDefinitionHierarchicalNameValid();
            var commonName = new IsDefiniitonCommonNameValid();
            var description = new IsDefiniitonDescriptionValid();
            var subdiscipline = new IsDefiniitonSubdisciplineValid();
            var tags = new IsDefiniitonTagsValid();
            var comment = new IsCommentValid();
            var owner = new IsDefiniitonOwnerValid();
            var version = new IsDefiniitonVersionValid();

            if (!await hierarchicalName.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( hierarchicalName.Messages );
            } else {
                if (!await owner.IsSatisfiedByAsync( candidate )) {
                    valid = false;
                    Messages.AddRange( owner.Messages );
                }
            }

            if (!await version.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( version.Messages );
            }

            if (!await commonName.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( commonName.Messages );
            }

            if (!await description.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( description.Messages );
            }

            if (!await subdiscipline.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( subdiscipline.Messages );
            }

            if (!await tags.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( tags.Messages );
            }

            if (!await comment.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( comment.Messages );
            }

            //Attribute specific fields
            var tc = new IsCourseOfFireTargetCollectionDefValid();
            var scoreFormatCollection = new IsCourseOfFireScoreFormatCollectionDefValid();
            var essm = new IsCourseOfFireDefaultEventAndStageStyleMappingDefValid();
            var a = new IsCourseOfFireDefaultAttributeDefValid();
            var scoreFormats = new IsCourseOfFireScoreFormatsValid();
            var singularStageLabesl = new IsCourseOfFireSingularStageLabelsValid();
            var rankingRuleMapping = new IsCourseOfFireResultEventRankingRuleMappingValid();
            var resultListFormatDefs = new IsCourseOfFireEventResultListFormatDefValid();
            var eventStyleMapping = new IsCourseOfFireEventEventStyleMappingValid();
            var stageStyleMapping = new IsCourseOfFireEventStageStyleMappingValid();
            var eventTree = new IsCourseOfFireEventTreeValid();
            var abbrFormats = new IsCourseOfFireAbbreviatedFormatsValid();
            var commandAutomationIds = new IsCourseOfFireRangeScriptAutomationIdsValid();
            var calculationVariables = new IsCourseOfFireEventCalculationVariablesValid();


			if (!await tc.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( tc.Messages );
            }

            if (!await scoreFormatCollection.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( scoreFormatCollection.Messages );
            } else {
                if (!await scoreFormats.IsSatisfiedByAsync( candidate )) {
                    valid = false;
                    Messages.AddRange( scoreFormats.Messages );
                }
            }

            if (!await essm.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( essm.Messages );
            }

            if (!await a.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( a.Messages );
            }

            if (!await singularStageLabesl.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( singularStageLabesl.Messages );
            }

            if (!await rankingRuleMapping.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( rankingRuleMapping.Messages );
            }

            if (!await resultListFormatDefs.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( resultListFormatDefs.Messages );
            }

            if (!await eventStyleMapping.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( eventStyleMapping.Messages );
            }

            if (!await stageStyleMapping.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( stageStyleMapping.Messages );
            }

            if (!await eventTree.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( eventTree.Messages );
            }

            if (!await abbrFormats.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( abbrFormats.Messages );
            }

            if (!await commandAutomationIds.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( commandAutomationIds.Messages );
            }

            if (!await calculationVariables.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( calculationVariables.Messages );
            }

            return valid;
        }
    }

    /// <summary>
    /// Tests whether the ScoreFormatCollectionDef property is valid in the passed in CourseOfFire instance.
    /// </summary>
    public class IsCourseOfFireTargetCollectionDefValid : CompositeSpecification<CourseOfFire> {
        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {
            Messages.Clear();
            bool valid = true;

            //Test if .TargetCollectionDef is a valid set name for a real lifee TARGET COLLECTION
            var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( "TargetCollectionDef",
                candidate.TargetCollectionDef,
                DefinitionType.TARGETCOLLECTION );

            if (!vm.Valid) {
                Messages.Add( vm.Message );
                return false;
            }

            //Test that the DefaultTargetCollectionName value is a name listed in the TARGET COLLECTION 
            var setName = SetName.Parse( candidate.TargetCollectionDef );
            var targetCollection = await DefinitionCache.GetTargetCollectionDefinitionAsync( setName );

            bool foundTargetCollectionName = false;
            foreach (var tc in targetCollection.TargetCollections) {
                if (tc.TargetCollectionName == candidate.DefaultTargetCollectionName) {
                    foundTargetCollectionName = true;
                    break;
                }
            }

            if (!foundTargetCollectionName) {
                valid = false;
                Messages.Add( $"The value of DefaultTargetCollectionName must be listed in the TARGET COLLECTION. The valid values are {string.Join( ", ", targetCollection.GetTargetCollectionNames() )}" );
            }

            return valid;
        }
    }

    /// <summary>
    /// Tests whether the ScoreFormationCollectionDef property is valid in the passed in CourseOfFire instance.
    /// </summary>
    public class IsCourseOfFireScoreFormatCollectionDefValid : CompositeSpecification<CourseOfFire> {
        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {
            Messages.Clear();
            bool valid = true;

            //Test if .TargetCollectionDef is a valid set name for a real lifee SCORE FORMAT COLLECTION
            var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( "ScoreFormatCollectionDef",
                candidate.ScoreFormatCollectionDef,
                DefinitionType.SCOREFORMATCOLLECTION );

            if (!vm.Valid) {
                Messages.Add( vm.Message );
                return false;
            }

            //Test that the ScoreConfigDefault value is a name listed in the SCORE FORMAT COLLECTION 
            var setName = SetName.Parse( candidate.ScoreFormatCollectionDef );
            var scoreConfigDefinition = await DefinitionCache.GetScoreFormatCollectionDefinitionAsync( setName );

            bool foundScoreConfigName = false;
            foreach (var tc in scoreConfigDefinition.ScoreConfigs) {
                if (tc.ScoreConfigName == candidate.ScoreConfigDefault) {
                    foundScoreConfigName = true;
                    break;
                }
            }

            if (!foundScoreConfigName) {
                valid = false;
                Messages.Add( $"The value of ScoreConfigDefault must be listed in the SCORE FORMAT COLLECTION. The valid values are {string.Join( ", ", scoreConfigDefinition.GetScoreConfigNames() )}" );
            }

            return valid;
        }
    }

    /// <summary>
    /// Tests that the COURSE OF FIRE's value for DefaultEventAndStageStyleMappingDef is valid.
    /// </summary>
    public class IsCourseOfFireDefaultEventAndStageStyleMappingDefValid : CompositeSpecification<CourseOfFire> {

        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {
            Messages.Clear();
            bool valid = true;

            var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync(
                "DefaultEventAndStageStyleMappingDef",
                candidate.DefaultEventAndStageStyleMappingDef,
                DefinitionType.EVENTANDSTAGESTYLEMAPPING );

            if (!vm.Valid) {
                Messages.Add( vm.Message );
                valid = false;
            }

            return valid;
        }
    }

    public class IsCourseOfFireDefaultAttributeDefValid : CompositeSpecification<CourseOfFire> {

        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {
            Messages.Clear();
            bool valid = true;

            //DefaultAttributeDef is allowed to be empty or null
            if (!string.IsNullOrEmpty( candidate.DefaultAttributeDef )) {

                var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync(
                    "DefaultAttributeDef",
                    candidate.DefaultAttributeDef,
                    DefinitionType.ATTRIBUTE );

                if (!vm.Valid) {
                    Messages.Add( vm.Message );
                    valid = false;
                }
            }

            return valid;
        }
    }

    /// <summary>
    /// Inspects both the Events and Singulars for valid values for .ScoreFormat.
    /// </summary>
    /// <remarks>Assumes that the value for ScoreFormatCollectionDef has already been validated.</remarks>
    public class IsCourseOfFireScoreFormatsValid : CompositeSpecification<CourseOfFire> {

        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {
            Messages.Clear();
            bool valid = true;

            var scoreConfig = await candidate.GetScoreFormatCollectionDefinitionAsync();
            var scoreFormats = scoreConfig.ScoreFormats;

            int index = 0;
            foreach (var singular in candidate.Singulars) {
                if (!scoreFormats.Contains( singular.ScoreFormat )) {
                    valid = false;
                    Messages.Add( $"Singular[{index}].ScoreForamt '{singular.ScoreFormat}' is not known. The valid values are {string.Join( ", ", scoreFormats )}." );
                }

                index++;
            }

            index = 0;
            foreach (var @event in candidate.Events) {
                if (!scoreFormats.Contains( @event.ScoreFormat )) {
                    valid = false;
                    Messages.Add( $"Singular[{index}].ScoreForamt '{@event.ScoreFormat}' is not known. The valid values are {string.Join( ", ", scoreFormats )}." );
                }

                index++;
            }

            return true;
        }
    }

    /// <summary>
    /// Tests that each Singular has a non empty string for the StageLabel. And that each StageLabel is unique.
    /// </summary>
    public class IsCourseOfFireSingularStageLabelsValid : CompositeSpecification<CourseOfFire> {

        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {

            Messages.Clear();
            bool valid = true;

            HashSet<string> seen = new HashSet<string>();

            int index = 0;
            foreach (var singular in candidate.Singulars) {
                if (string.IsNullOrEmpty( singular.StageLabel )) {
                    valid = false;
                    Messages.Add( $"Singular[{index}] has an empty string as its value for StageLabel." );
                } else {
                    if (seen.Contains( singular.StageLabel )) {
                        valid = false;
                        Messages.Add( $"Singular[{index}] has a StageLabel '{singular.StageLabel}' that is used more than once." );
                    } else {
                        seen.Add( singular.StageLabel );
                    }
                }
            }

            return valid;
        }
    }

    /// <summary>
    /// Tests that each Event's RankingRuleMapping has valid key names (either DefaultDef or a ScoreConfig name),
    /// and the SetName points to a valid RANKING RULE
    /// </summary>
    public class IsCourseOfFireResultEventRankingRuleMappingValid : CompositeSpecification<CourseOfFire> {

        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {

            Messages.Clear();
            bool valid = true;


            var scoreConfigDefinition = await candidate.GetScoreFormatCollectionDefinitionAsync();
            var scoreConfigNames = scoreConfigDefinition.GetScoreConfigNames();
            scoreConfigNames.Add( "DefaultDef" ); //DefaultDef is valid as the default ScoreConfig name

            var index = 0;
            foreach (var @event in candidate.Events) {

                foreach (var rrm in @event.RankingRuleMapping) {

                    //Check that the Key value is known ScoreFormat
                    if (!scoreConfigNames.Contains( rrm.Key )) {
                        valid = false;
                        Messages.Add( $"Event[{index}].RankingRuleMapping has a key value '{rrm.Key}' that is unknown. The acceptable ScoreConfigName values are {string.Join( ", ", scoreConfigNames )}." );
                    } else {

                        var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync(
                            $"Event[{index}].RankingRuleMapping",
                            rrm.Value,
                            DefinitionType.RANKINGRULES );

                        if (!vm.Valid) {
                            valid = false;
                            Messages.Add( vm.Message );
                        }

                    }
                }
                index++;
            }

            return valid;
        }
    }

    /// <summary>
    /// Checks that each Command Automation has a unique ID.
    /// </summary>
    public class IsCourseOfFireRangeScriptAutomationIdsValid : CompositeSpecification<CourseOfFire> {

        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {

            Messages.Clear();
            bool valid = true;

            List<int> seenIds =     new List<int>();

            int indexRangeScript = 0;
            int indexSegmentGroup = 0;
            int indexCommand = 0;
            int indexAutomation = 0;
            foreach( var rs in candidate.RangeScripts ) {

                indexSegmentGroup = 0;
                foreach( var sg in rs.SegmentGroups ) {

                    indexCommand = 0;
                    foreach ( var command in sg.Commands ) {

                        indexAutomation = 0;
                        foreach( var automation in command.Automation ) {

                            if (automation.Id <= 0) {
                                valid = false;
                                Messages.Add( $"Command Automation RangeScripts[{indexRangeScript}].SegmentGroup[{indexSegmentGroup}].Command[{indexCommand}].Automation[{indexAutomation}] must have an unique id that is greater than 0." );
                            } else if ( seenIds.Contains( automation.Id )) {
                                valid = false;
                                Messages.Add( $"Command Automation RangeScripts[{indexRangeScript}].SegmentGroup[{indexSegmentGroup}].Command[{indexCommand}].Automation[{indexAutomation}] has an Id '{automation.Id}' that has been used already." );
                            } else {
                                seenIds.Add( automation.Id );
                            }
                            indexAutomation++;

                        }
                        indexCommand++;

                    }
                    indexSegmentGroup++;
                }
            }

            return valid;

        }
    }

    /// <summary>
    /// Tests if the value for ResultListFormatDef, on each Event, is valid.
    /// </summary>
    public class IsCourseOfFireEventResultListFormatDefValid : CompositeSpecification<CourseOfFire> {

        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {

            Messages.Clear();
            bool valid = true;

            var index = 0;
            foreach (var @event in candidate.Events) {

                //ResultListFormatDef is allowed to be an empty string
                if (!string.IsNullOrEmpty( @event.ResultListFormatDef )) {
                    var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync(
                        $"Event[{index}].ResultListFormatDef",
                        @event.ResultListFormatDef,
                        DefinitionType.RESULTLISTFORMAT );

                    if (!vm.Valid) {
                        valid = false;
                        Messages.Add( vm.Message );
                    }
                }

                index++;
            }

            return valid;
        }
	}

	/// <summary>
	/// Tests if the CalculatoinVariables are valid, given an Event's Calculation method
	/// </summary>
	public class IsCourseOfFireEventCalculationVariablesValid : CompositeSpecification<CourseOfFire> {

        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {

            Messages.Clear();
            bool valid = true;

            var index = 0;
            var variableIndex = 0;
            foreach (var @event in candidate.Events) {

                switch (@event.Calculation) {
                    case EventCalculation.SUM:
                        //CalculationVariables must be of type Score Component.
                        variableIndex = 0;
                        foreach (var variable in @event.CalculationVariables) {
                            if (variable.VariableType != CalculationVariableType.SCORE) {
                                valid = false;
                                Messages.Add( $"Event[{index}] has Calculation method SUM. However, CalculationVariable[{variableIndex}] is of tpe {variable.VariableType} and instead must be of type SCORE." );
                            }
                            variableIndex++;
                        }

                        //May have zero to many. So not doing a check on the number of variables.
                        break;

					case EventCalculation.AVERAGE:
						//CalculationVariables must be type Integer
						variableIndex = 0;
						foreach (var variable in @event.CalculationVariables) {
							if (variable.VariableType != CalculationVariableType.INTEGER) {
								valid = false;
								Messages.Add( $"Event[{index}] has Calculation method AVERAGE. However, CalculationVariable[{variableIndex}] is of tpe {variable.VariableType} and instead must be of type INTEGER." );
							}
							variableIndex++;
						}

                        //Must have exactly one variable.
                        if ( @event.CalculationVariables.Count != 1 ) {
							valid = false;
							Messages.Add( $"Event[{index}] has Calculation method AVERAGE, and must have exactly 1 CalculationVariable of type INTEGER. Instead have {@event.CalculationVariables.Count}." );
						}
						break;

                    default:
                        //The other EventCalculation values are deprecated and one day should cause an Specification error. EKA May 2025.
                        break;
				}


                index++;
            }

			return valid;
		}
	}

	/// <summary>
	/// Tests that there is only one EventType EVENT.
	/// Tests that the one EventType EVENT has a EventStyleMapping object.
	/// Tests that the EventStyleMapping object has a valid reference to an EVENT STYLE.
	/// Tests that the remaining (non EventType EVENT) events do not have a EventStyleMapping object.
	/// </summary>
	public class IsCourseOfFireEventEventStyleMappingValid : CompositeSpecification<CourseOfFire> {

        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {

            Messages.Clear();
            bool valid = true;

            var index = 0;
            bool foundEventTypeEvent = false;
            foreach (var @event in candidate.Events) {

                if (@event.EventType == EventtType.EVENT) {
                    //Test that there is only one EventType EVENT.
                    if (foundEventTypeEvent) {
                        valid = false;
                        Messages.Add( $"Event[{index}] is listed as an EventType EVENT, but one was already found. A COURSE OF FIRE must have exactly one EventType EVENT, and it must be the top level event." );
                        continue;
                    } else {
                        foundEventTypeEvent = true;
                    }

                    //Should have an EventStyleMapping object
                    //With a valid EVENT STYLE definition reference
                    if (@event.EventStyleMapping == null) {
                        valid = false;
                        Messages.Add( $"Event[{index}] is listed as an EventType EVENT, but does not have an EventStyleMapping" );
                    } else {
                        var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync(
                            $"Event[{index}].EventStyleMapping.DefaultDef",
                            @event.EventStyleMapping.DefaultDef,
                            DefinitionType.EVENTSTYLE );

                        if (!vm.Valid) {
                            valid = false;
                            Messages.Add( vm.Message );
                        }
                    }
                } else {
                    //Test that other (non EventType EVENT) events do not have an EventStyleMapping object
                    if (@event.EventStyleMapping != null) {
                        valid = false;
                        Messages.Add( $"Event[{index}] is listed as an EventType {@event.EventType}, but includes an EventStyleMapping" );
                    }

                }

                index++;
            }

            if (!foundEventTypeEvent) {
                valid = false;
                Messages.Add( $"An Event with EventType EVENT was not found. A COURSE OF FIRE must have exactly one EventType EVENT, and it must be the top level event." );
            }

            return valid;
        }
    }


    /// <summary>
    /// Tests that there is at least one EventType STAGE.
    /// Tests that each EventType STAGE has a StageStyleMapping object.
    /// Tests that the StageStyleMapping object has a valid reference to an STAGE STYLE.
    /// Tests that the remaining (non EventType STAGE) events do not have a StageStyleMapping object.
    /// </summary>
    public class IsCourseOfFireEventStageStyleMappingValid : CompositeSpecification<CourseOfFire> {

        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {

            Messages.Clear();
            bool valid = true;

            var index = 0;
            bool foundEventTypeStage = false;
            foreach (var @event in candidate.Events) {

                if (@event.EventType == EventtType.STAGE) {
                    //Test that there is at least one EventType STAGE.
                    foundEventTypeStage = true;

                    //Should have an StageStyleMapping object
                    //With a valid STAGE STYLE definition reference
                    if (@event.StageStyleMapping == null) {
                        valid = false;
                        Messages.Add( $"Event[{index}] is listed as an EventType STAGE, but does not have an StageStyleMapping" );
                    } else {
                        var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync(
                            $"Event[{index}].StageStyleMapping.DefaultDef",
                            @event.StageStyleMapping.DefaultDef,
                            DefinitionType.STAGESTYLE );

                        if (!vm.Valid) {
                            valid = false;
                            Messages.Add( vm.Message );
                        }
                    }
                } else {
                    //Test that other (non EventType STAGE) events do not have an StageStyleMapping object
                    if (@event.StageStyleMapping != null) {
                        valid = false;
                        Messages.Add( $"Event[{index}] is listed as an EventType {@event.EventType}, but includes an StageStyleMapping" );
                    }

                }

                index++;
            }

            if (!foundEventTypeStage) {
                valid = false;
                Messages.Add( $"An Event with EventType STAGE was not found. A COURSE OF FIRE must have at least one EventType STAGE." );
            }

            return valid;
        }
    }

    public class IsCourseOfFireEventTreeValid : CompositeSpecification<CourseOfFire> {

        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {

            Messages.Clear();
            bool valid = true;

            try {
                var eventTree = EventComposite.GrowEventTree( candidate );

                //Count how many singulars are defined.
                var singularCount = 0;
                foreach (var singular in candidate.Singulars) {
                    singularCount += singular.GetSingularEventList().Count;
                }

                //Which should match the number of Singulars in the EventTree
                if (singularCount != eventTree.GetAllSingulars().Count) {
                    valid = false;
                    Messages.Add( $"Not all Singulars are accounted for in the Event Tree. Make sure they each have a parent Event." );
                }

                //Compile a list of Events, excluding those marked ExternalToEventTree
                var listOfEvents = new List<Event>();
                foreach (var origEvent in candidate.Events) {
                    foreach (var cloneEvent in origEvent.GetCompiledEvents()) {
                        if (!cloneEvent.ExternalToEventTree) {
                            listOfEvents.Add( cloneEvent );
                        }
                    }
                }

                //Check that there are no duplicates
                var duplicates = listOfEvents
                        .GroupBy(x => x.EventName)
                        .Where(g => g.Count() > 1)
                        .Select(g => g.Key)
                        .ToList();
                if (duplicates.Any())
                {
                    valid = false;
                    Messages.Add($"The following events occur multiple times in the Event Tree: " + string.Join(", ", duplicates));
                }

                //Check that each of them are in the EventTree
                foreach (var @event in listOfEvents) {
                    var foo = eventTree.FindEventComposite( @event.EventName );
                    if (foo == null) {
                        valid = false;
                        Messages.Add( $"The Event {@event.EventName} is defined, but is not found in the Event Tree." );
                    }
                }

            } catch (Exception ex) {
                valid = false;
                Messages.Add( $"Could not grow the Event Tree, which means something is wrong. Recevied error {ex}." );
            }

            return valid;
        }
    }
    public class IsCourseOfFireAbbreviatedFormatsValid : CompositeSpecification<CourseOfFire> {

        public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {

            Messages.Clear();
            bool valid = true;

            try {
                if (candidate.AbbreviatedFormats.Count == 0) {
                    valid = false;
                    Messages.Add( $"At least one AbbreviatedFormat is required." );
                } else {
                    var eventTree = EventComposite.GrowEventTree( candidate );
                    var externalEvents = EventComposite.FindExternalEvents( candidate );

                    int index = 0;
                    int childIndex = 0;
                    foreach (var af in candidate.AbbreviatedFormats) {

                        if (string.IsNullOrEmpty( af.FormatName )) {
                            valid = false;
                            Messages.Add( $"AbbreviatedFormats[{index}] does not have a FormatName." );
                        }

                        var foo = eventTree.FindEventComposite( af.EventName );
                        if (foo == null) {
                            valid = false;
                            Messages.Add( $"AbbreviatedFormats[{index}] names an Event '{af.EventName}' that does not exist." );
                        }

                        childIndex = 0;
                        foreach (var child in af.Children) {
                            switch (child.Derivation) {
                                case EventDerivationType.EXPLICIT:
                                    //Check if the Event name is in the Event Tree
                                    //If not, check if it is an external event.
                                    foo = eventTree.FindEventComposite( child.EventName );
                                    if (foo == null && ! externalEvents.ContainsKey( child.EventName ) ) {
                                        valid = false;
                                        Messages.Add( $"AbbreviatedFormats[{index}][{childIndex}] names an Event '{child.EventName}' that does not exist." );
                                    }
                                    break;

                                case EventDerivationType.EXPAND:
                                    var vs1 = new ValueSeries( ((AbbreviatedFormatChildExpand)child).Values );
                                    foreach (var eventName in vs1.GetAsList( child.EventName )) {

										//Check if the Event name is in the Event Tree
										//If not, check if it is an external event.
										foo = eventTree.FindEventComposite( eventName );
										if (foo == null && !externalEvents.ContainsKey( child.EventName ) ) {
											valid = false;
											Messages.Add( $"AbbreviatedFormats[{index}][{childIndex}] compiles to an Event '{child.EventName}' that does not exist." );
										}
									}
                                    break;

                                case EventDerivationType.DERIVED:
									//Currently don't have a good way of checking these, as the expansion depends on the Result Event. So only going to check against a ValueSeries of "1".
									var vs2 = new ValueSeries( "1" );
									foreach (var eventName in vs2.GetAsList( child.EventName )) {

										//Check if the Event name is in the Event Tree
										//If not, check if it is an external event.
										foo = eventTree.FindEventComposite( eventName );
										if (foo == null && !externalEvents.ContainsKey( child.EventName ) ) {
											valid = false;
											Messages.Add( $"AbbreviatedFormats[{index}][{childIndex}] compiles to an Event '{child.EventName}' that does not exist." );
										}
									}
									break;
							}

                            childIndex++;
                        }

                        index++;
                    }
                }
            } catch (Exception ex) {
                valid = false;
                Messages.Add( ex.ToString() );
            }

            return valid;
        }
    }
}
