using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataActors.Specification.Definitions {
	public class IsTargetCollectionValid : CompositeSpecification<TargetCollection> {

		public override async Task<bool> IsSatisfiedByAsync( TargetCollection candidate ) {

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
			var targetCollectionModals = new IsTargetCollectionTargetCollectionsValid();

            if (!await targetCollectionModals.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( targetCollectionModals.Messages );
            }

            return valid;
		}
	}

	public class IsTargetCollectionTargetCollectionsValid : CompositeSpecification<TargetCollection> {

        public override async Task<bool> IsSatisfiedByAsync( TargetCollection candidate ) {
			Messages.Clear();

			bool valid = true;
			HashSet<string> targetCollectionNames = new HashSet<string>();

			foreach (var tc in candidate.TargetCollections) {
				//Test that the TargetCollectionName is not empty
				if (string.IsNullOrEmpty( tc.TargetCollectionName )) {
					valid = false;
					Messages.Add( "Each TargetCollectionModel's TargetCollectionName must be not null and not empty." );
					continue;

				} else {
					//Test that the TargetCollectionName is unique
					if (targetCollectionNames.Contains( tc.TargetCollectionName )) {
					valid = false;
						Messages.Add( $"Each TargetCollectionModel's TargetCollectionName must be unique. {tc.TargetCollectionName} is used more than once." );

					} else {
						targetCollectionNames.Add( tc.TargetCollectionName );
					}

				}

                //Test that the RangeDistance is not empty
                if (string.IsNullOrEmpty( tc.RangeDistance )) {
                    valid = false;
                    Messages.Add( $"The TargetCollection {tc.TargetCollectionName}'s RangeDistance must be not null and not empty. Usually represented with values such as '10m', '25m' or 'Mixed'." );
                }

				//Test that there is at least one TargetDef
				if (tc.TargetDefs.Count == 0) {
					valid = false;
					Messages.Add( $"The TargetCollection {tc.TargetCollectionName}'s must have one or more TargetDef (TARGET definition references)." );
				} else {
					foreach (var targetDef in tc.TargetDefs) {
						var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( "TargetDefs", targetDef, DefinitionType.TARGET );

						if (!vm.Valid) {
							Messages.Add( vm.Message );
							return false;
						}
					}
				}
            }

            return valid;
        }
    }
}
