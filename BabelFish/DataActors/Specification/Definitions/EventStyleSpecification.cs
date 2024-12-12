using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataActors.Specification.Definitions {
    public class IsEventStyleValid : CompositeSpecification<EventStyle> {
        /// <inheritdoc />
        public override async Task<bool> IsSatisfiedByAsync( EventStyle candidate ) {

            var valid = true;

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

            //EventStyle specific fields
            var stageStylesAndEventStyles = new IsEventStyleStageStylesAndEventStylesValid();
            var eventStyles = new IsEventStyleEventStylesValid();
            var relatedEventStyles = new IsEventStyleRelatedEventStylesValid();
            var stageStyles = new IsEventStyleStageStylesValid();
            var simpleCof = new IsEventStyleSimpleCOFsValid();

            if (!await stageStylesAndEventStyles.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( stageStylesAndEventStyles.Messages );
			}

			if (!await eventStyles.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( eventStyles.Messages );
			}

			if (!await relatedEventStyles.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( relatedEventStyles.Messages );
			}

			if (!await stageStyles.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( stageStyles.Messages );
			}

			if (!await simpleCof.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( simpleCof.Messages );
			}

			return valid;
        }

        /// <summary>
        /// Tests whether StageStyles or EventStyles is included, but not both.
        /// </summary>
        public class IsEventStyleStageStylesAndEventStylesValid : CompositeSpecification<EventStyle> {

            /// <inheritdoc />
            public override async Task<bool> IsSatisfiedByAsync( EventStyle candidate ) {
				Messages.Clear();

				bool valid = true;

                if (candidate.EventStyles == null) {
                    //Just set to an empty list.
                    candidate.EventStyles = new List<string>();
                }

                if (candidate.StageStyles == null) {
                    //Just set to an empty list.
                    candidate.StageStyles = new List<string>();
                }

                if (candidate.EventStyles.Count == 0 && candidate.StageStyles.Count == 0) {
                    Messages.Add( $"The EventStyles property and StageStyles property may not both be empty. Exactly one of them must contain values." );
                    valid = false;
                }

                if (candidate.EventStyles.Count > 0 && candidate.StageStyles.Count > 0) {
                    Messages.Add( $"The EventStyles property and StageStyles property may not both have values. Only one of them may contain values." );
                    valid = false;
                }

                return valid;
            }

        }

        /// <summary>
        /// Tests whether EventStyles property is valid. 
        /// </summary>
        public class IsEventStyleEventStylesValid : CompositeSpecification<EventStyle> {

            /// <inheritdoc />
            public override async Task<bool> IsSatisfiedByAsync( EventStyle candidate ) {
                Messages.Clear();

                bool valid = true;
                if (candidate.EventStyles != null) {

                    foreach (var es in candidate.EventStyles) {
                        var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( "EventStyles", es, DefinitionType.EVENTSTYLE );
                        if (!vm.Valid) {
                            valid = false;
                            Messages.Add( vm.Message );
                        }
                    }
                } else {
                    //If the current valud is null, set it to an empty list.
                    candidate.EventStyles = new List<string>();
                }

                return valid;
            }
        }

        /// <summary>
        /// Tests whether EventStyles property is valid. 
        /// </summary>
        public class IsEventStyleRelatedEventStylesValid : CompositeSpecification<EventStyle> {

            /// <inheritdoc />
            public override async Task<bool> IsSatisfiedByAsync( EventStyle candidate ) {
				Messages.Clear();

				bool valid = true;
                if (candidate.RelatedEventStyles != null) {

                    foreach (var es in candidate.RelatedEventStyles) {
                        var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( "RelatedEventStyles", es, DefinitionType.EVENTSTYLE );
                        if (!vm.Valid) {
                            valid = false;
                            Messages.Add( vm.Message );
                        }
                    }
                } else {
                    //If the current valud is null, set it to an empty list.
                    candidate.RelatedEventStyles = new List<string>();
                }

                return valid;
            }
        }

        /// <summary>
        /// Tests whether StageStyles property is valid. 
        /// </summary>
        public class IsEventStyleStageStylesValid : CompositeSpecification<EventStyle> {

            /// <inheritdoc />
            public override async Task<bool> IsSatisfiedByAsync( EventStyle candidate ) {
				Messages.Clear();

				bool valid = true;
                if (candidate.StageStyles != null) {

                    foreach (var es in candidate.StageStyles) {
                        var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( "StageStyles", es, DefinitionType.STAGESTYLE );
                        if (!vm.Valid) {
                            valid = false;
                            Messages.Add( vm.Message );
                        }
                    }
                } else {
                    //If the current valud is null, set it to an empty list.
                    candidate.StageStyles = new List<string>();
                }

                return valid;
            }
        }

        /// <summary>
        /// Tests whether SimpleCOFs property is valid. 
        /// </summary>
        public class IsEventStyleSimpleCOFsValid : CompositeSpecification<EventStyle> {

            /// <inheritdoc />
            public override async Task<bool> IsSatisfiedByAsync( EventStyle candidate ) {
				Messages.Clear();

				bool valid = true;

                //NOTE .SimpleCOFs do not have to be checked, if .EventStyles is not empty
                if (candidate.EventStyles.Count == 0 && 
                    (candidate.SimpleCOFs == null || candidate.SimpleCOFs.Count == 0)) {
                    candidate.SimpleCOFs = new List<SimpleCOF>();
                    valid = false;
                    Messages.Add( $"The SimpleCOFs list may not be null and must contain at least one SimpleCOF object." );
                } else {
                    var listOfSeenCOFDefs = new List<string>();

                    //foreach SimpleCOF object
                    foreach (var simpleCof in candidate.SimpleCOFs) {

                        //Validate the value for CourseOfFireDef
                        var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( "SimpleCOFs", simpleCof.CourseOfFireDef, DefinitionType.COURSEOFFIRE );
                        if (!vm.Valid) {
                            valid = false;
                            Messages.Add( $"The SimpleCOF identified with CourseOfFireDef '{simpleCof.CourseOfFireDef}' is not a known CourseOfFire definiiton, or it is discontinued." );
                        } else if (listOfSeenCOFDefs.Contains( simpleCof.CourseOfFireDef )) {

                            valid = false;
                            Messages.Add( $"The SimpleCOF identified with CourseOfFireDef '{simpleCof.CourseOfFireDef}' is used in more than one SimpleCOF object. It may only be used in one SimpleCOF object." );
                        } else {
                            listOfSeenCOFDefs.Add( simpleCof.CourseOfFireDef );
                        }

                        if (!valid)
                            return valid;

                        //Validate the SimpleCOFComponents
                        if (simpleCof.Components == null || simpleCof.Components.Count == 0) {
                            valid = false;
                            Messages.Add( $"The SimpleCOF identified with CourseOfFireDef '{simpleCof.CourseOfFireDef}' must contain at least one SimpleCOFComponent object." );
                        } else {

                            var cof = await simpleCof.GetCourseOfFireDefinitionAsync();
                            var sfc = await cof.GetScoreFormatCollectionDefinitionAsync();

                            //Foreach SimpleCOFComponent
                            foreach (var component in simpleCof.Components) {
                                //Shots must be > 0
                                if ( component.Shots == 0 ) {
                                    valid = false;
                                    Messages.Add( $"The SimpleCOF identified with CourseOfFireDef '{simpleCof.CourseOfFireDef}' has a SimpleCOFCompoenent identified with StageStyleDef '{component.StageStyleDef}' with a value of Shots that is less than 0." );
                                }

                                //StageStyleDef must be listed in the candidate's .StageStyles list
                                if (! candidate.StageStyles.Contains( component.StageStyleDef) ) {
                                    valid = false;
                                    Messages.Add( $"The SimpleCOF identified with CourseOfFireDef '{simpleCof.CourseOfFireDef}' has a SimpleCOFCompoenent identified with StageStyleDef '{component.StageStyleDef}' that is not listed in the EventStyle's list of StageStyles." );
                                }

                                if ( !sfc.GetScoreConfigNames().Contains( component.ScoreConfigName ) ) {
                                    valid = false;
                                    Messages.Add( $"The SimpleCOF identified with CourseOfFireDef '{simpleCof.CourseOfFireDef}' has a SimpleCOFCompoenent identified with StageStyleDef '{component.StageStyleDef}' that has a ScoreConfigName '{component.ScoreConfigName}' that is not found in the ScoreFormatCollection '{cof.ScoreFormatCollectionDef}'." );
                                }
                            }

                        }
                    }
                }

                return valid;
            }

        }
    }
}
