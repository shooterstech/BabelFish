using Amazon.CognitoIdentityProvider.Model;
using Scopos.BabelFish.DataModel.Definitions;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Scopos.BabelFish.DataActors.Specification.Definitions {
	public class IsEventAndStageStyleMappingValid : CompositeSpecification<EventAndStageStyleMapping> {

		public override async Task<bool> IsSatisfiedByAsync( EventAndStageStyleMapping candidate ) {

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
			var defaultMapping = new IsEventAndStageStyleMappingDefaultMappingValid();
            var mappings = new IsEventAndStageStyleMappingMappingsValid();

            if ( ! await defaultMapping.IsSatisfiedByAsync( candidate ) ) {
				valid = false;
				Messages.AddRange( defaultMapping.Messages );
			}

            if (!await mappings.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( mappings.Messages );
            }

            return valid;
		}
	}

	public class IsEventAndStageStyleMappingDefaultMappingValid : CompositeSpecification<EventAndStageStyleMapping> {

		public override async Task<bool> IsSatisfiedByAsync( EventAndStageStyleMapping candidate ) {
			Messages.Clear();
			var valid = true;

			//Must have a DefaultMapping property
			if (candidate.DefaultMapping == null) {
				valid = false;
				Messages.Add( $"The property DefaultMapping is required." );
				return valid;
			}

			//Must have a value for DefaultStageStyleDef
			var vmDefaultStageStyleDef = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( "DefaultMapping.DefaultStageStyleDef",
				candidate.DefaultMapping.DefaultStageStyleDef,
				DefinitionType.STAGESTYLE );

			if (!vmDefaultStageStyleDef.Valid) {
				Messages.Add( vmDefaultStageStyleDef.Message );
				valid = false;
			}

			//Must have a value for DefaultEventStyleDef
			var vmDefaultEventStyleDef = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( "DefaultMapping.DefaultEventStyleDef",
				candidate.DefaultMapping.DefaultEventStyleDef,
				DefinitionType.EVENTSTYLE );

			if (!vmDefaultEventStyleDef.Valid) {
				Messages.Add( vmDefaultEventStyleDef.Message );
				valid = false;
			}

			//AttributeValueAppellation must be an empty list within the DefaultMapping
			if (candidate.DefaultMapping.AttributeValueAppellation != null
				&& candidate.DefaultMapping.AttributeValueAppellation.Count > 0) {
				valid = false;
				Messages.Add( $"The DefaultMapping.AttributeValueAppellation list must be empty." );
			}

			//TargetCollectionName must be an empty list within the DefaultMapping
			if (candidate.DefaultMapping.TargetCollectionName != null
				&& candidate.DefaultMapping.TargetCollectionName.Count > 0) {
				valid = false;
				Messages.Add( $"The DefaultMapping.TargetCollectionName list must be empty." );
			}

			HashSet<string> seenNames = new HashSet<string>();

			//StageStyleMapping is allowed to be null or empty.
			if (candidate.DefaultMapping.StageStyleMappings != null) {

				seenNames.Clear();
				foreach (var ssm in candidate.DefaultMapping.StageStyleMappings) {

                    //Check that each StageStyleMapping has a non empty value.
                    if (string.IsNullOrEmpty( ssm.StageAppellation )) {
						valid = false;
						Messages.Add( $"The DefaultMapping has one or more StageStyleMappings with a null or empty string value for StageSppellation" );
						break;
					}

					//Each StageAppellation must be unique within the mapping.
					if ( seenNames.Contains( ssm.StageAppellation ) ) {
						valid = false;
						Messages.Add( $"The StageAppellation value {ssm.StageAppellation} is used more than once within the DefaultMapping.StageStyleMappings." );
					} else {
						seenNames.Add( ssm.StageAppellation );
					}

					//The StageAppellation must reference a real Stage Style
					var vmStageStyleMapping = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( $"DefaultMapping.StageStyleMappings.{ssm.StageAppellation}",
						ssm.StageStyleDef,
						DefinitionType.STAGESTYLE );

					if (!vmStageStyleMapping.Valid) {
						valid = false;
						Messages.Add( vmStageStyleMapping.Message );
					}
				}
            }

            //EventStyleMapping is allowed to be null or empty.
            if (candidate.DefaultMapping.EventStyleMappings != null) {

                seenNames.Clear();
                foreach (var esm in candidate.DefaultMapping.EventStyleMappings) {

                    //Check that each EventStyleMapping has a non empty value.
                    if (string.IsNullOrEmpty( esm.EventAppellation )) {
                        valid = false;
                        Messages.Add( $"The DefaultMapping has one or more EventStyleMappings with a null or empty string value for EventAppellation" );
                        break;
                    }

                    //Each EventAppellation must be unique within the mapping.
                    if (seenNames.Contains( esm.EventAppellation )) {
                        valid = false;
                        Messages.Add( $"The EventAppellation value {esm.EventAppellation} is used more than once within the DefaultMapping.EventStyleMappings." );
                    } else {
                        seenNames.Add( esm.EventAppellation );
                    }

                    //The EventAppellation must reference a real Event Style
                    var vmEventStyleMapping = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( $"DefaultMapping.EventStyleMappings.{esm.EventAppellation}",
                        esm.EventStyleDef,
                        DefinitionType.EVENTSTYLE );

                    if (!vmEventStyleMapping.Valid) {
                        valid = false;
                        Messages.Add( vmEventStyleMapping.Message );
                    }
                }
            }

            return valid;
		}
    }
    public class IsEventAndStageStyleMappingMappingsValid : CompositeSpecification<EventAndStageStyleMapping> {

        public override async Task<bool> IsSatisfiedByAsync( EventAndStageStyleMapping candidate ) {
            Messages.Clear();
            var valid = true;

            //Must may be null or an empty list
            if (candidate.Mappings != null) {

                int index = 0;
                foreach (var mapping in candidate.Mappings) {

                    //Must have a value for DefaultStageStyleDef
                    var vmDefaultStageStyleDef = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( $"Mappings[{index}].DefaultStageStyleDef",
                        mapping.DefaultStageStyleDef,
                        DefinitionType.STAGESTYLE );

                    if (!vmDefaultStageStyleDef.Valid) {
                        Messages.Add( vmDefaultStageStyleDef.Message );
                        valid = false;
                    }

                    //Must have a value for DefaultEventStyleDef
                    var vmDefaultEventStyleDef = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( $"Mappings[{index}].DefaultEventStyleDef",
                        mapping.DefaultEventStyleDef,
                        DefinitionType.EVENTSTYLE );

                    if (!vmDefaultEventStyleDef.Valid) {
                        Messages.Add( vmDefaultEventStyleDef.Message );
                        valid = false;
                    }

                    //AttributeValueAppellation may be an empty list

                    //TargetCollectionName may be an empty list

                    HashSet<string> seenNames = new HashSet<string>();

                    //StageStyleMapping is allowed to be null or empty.
                    if (mapping.StageStyleMappings != null) {

                        seenNames.Clear();
                        foreach (var ssm in mapping.StageStyleMappings) {

                            //Check that each StageStyleMapping has a non empty value.
                            if (string.IsNullOrEmpty( ssm.StageAppellation )) {
                                valid = false;
                                Messages.Add( $"The Mappings[{index}] has one or more StageStyleMappings with a null or empty string value for StageSppellation" );
                                break;
                            }

                            //Each StageAppellation must be unique within the mapping.
                            if (seenNames.Contains( ssm.StageAppellation )) {
                                valid = false;
                                Messages.Add( $"The StageAppellation value {ssm.StageAppellation} is used more than once within the Mappings[{index}].StageStyleMappings." );
                            } else {
                                seenNames.Add( ssm.StageAppellation );
                            }

                            //The StageAppellation must reference a real Stage Style
                            var vmStageStyleMapping = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( $"Mappings[{index}].StageStyleMappings.{ssm.StageAppellation}",
                                ssm.StageStyleDef,
                                DefinitionType.STAGESTYLE );

                            if (!vmStageStyleMapping.Valid) {
                                valid = false;
                                Messages.Add( vmStageStyleMapping.Message );
                            }
                        }
                    }

                    //EventStyleMapping is allowed to be null or empty.
                    if (mapping.EventStyleMappings != null) {

                        seenNames.Clear();
                        foreach (var esm in mapping.EventStyleMappings) {

                            //Check that each EventStyleMapping has a non empty value.
                            if (string.IsNullOrEmpty( esm.EventAppellation )) {
                                valid = false;
                                Messages.Add( $"The Mappings[{index}] has one or more EventStyleMappings with a null or empty string value for EventAppellation" );
                                break;
                            }

                            //Each EventAppellation must be unique within the mapping.
                            if (seenNames.Contains( esm.EventAppellation )) {
                                valid = false;
                                Messages.Add( $"The EventAppellation value {esm.EventAppellation} is used more than once within the Mappings[{index}].EventStyleMappings." );
                            } else {
                                seenNames.Add( esm.EventAppellation );
                            }

                            //The EventAppellation must reference a real Event Style
                            var vmEventStyleMapping = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( $"Mappings[{index}].EventStyleMappings.{esm.EventAppellation}",
                                esm.EventStyleDef,
                                DefinitionType.EVENTSTYLE );

                            if (!vmEventStyleMapping.Valid) {
                                valid = false;
                                Messages.Add( vmEventStyleMapping.Message );
                            }
                        }
                    }

                    index++;
                }
            }

            return valid;
        }
    }
}
