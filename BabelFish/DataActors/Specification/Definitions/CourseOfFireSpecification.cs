using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataActors.Specification.Definitions {
	public class IsCourseOfFireValid : CompositeSpecification<CourseOfFire> {

		public override async Task<bool> IsSatisfiedByAsync( CourseOfFire candidate ) {

			var valid = true;
			Messages.Clear();

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
			var sc = new IsCourseOfFireScoreFormatCollectionDefValid();
            var essm = new IsCourseOfFireDefaultEventAndStageStyleMappingDefValid();
            var a = new IsCourseOfFireDefaultAttributeDefValid();

            if (! await tc.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( tc.Messages );
            }

            if (! await sc.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( sc.Messages );
            }

            if (! await essm.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( essm.Messages );
            }

            if (! await a.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( a.Messages );
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
			foreach( var tc in targetCollection.TargetCollections ) {
				if ( tc.TargetCollectionName == candidate.DefaultTargetCollectionName ) {
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
}
