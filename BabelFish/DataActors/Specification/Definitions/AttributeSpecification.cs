using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Attribute = Scopos.BabelFish.DataModel.Definitions.Attribute;
using Scopos.BabelFish.DataModel.AttributeValue;
using Scopos.BabelFish.Helpers;

namespace Scopos.BabelFish.DataActors.Specification.Definitions {
	public class IsAttributeValid : CompositeSpecification<Attribute> {

		public override async Task<bool> IsSatisfiedByAsync( Attribute candidate ) {

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

			//Attribute specific fields
			var displayName = new IsAttributeDisplayNameValid();
			var designation = new IsAttributeDesignationValid();
			var defaultVisibility = new IsAttributeDefaultVisibilityValid();
			var fieldNameUnique = new IsAttributeFieldNameUnique();
			var fieldNotEmpty = new IsAttributeFieldNotEmpty();
			var fieldDefaultValue = new IsAttributeFieldDefaultValueValid();

			if (!await displayName.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( displayName.Messages );
			}

			if (!await designation.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( designation.Messages );
			}

			if (!await defaultVisibility.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( defaultVisibility.Messages );
			}

			if (!await fieldNameUnique.IsSatisfiedByAsync( candidate )) {
				valid = false; 
				Messages.AddRange( fieldNameUnique.Messages );
			}

			if (!await fieldNotEmpty.IsSatisfiedByAsync( candidate )) {
				valid = false; 
				Messages.AddRange( fieldNotEmpty.Messages );
			}

			if (!await fieldDefaultValue.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( fieldDefaultValue.Messages );
			}

			return valid;
		}
	}

	/// <summary>
	/// Tests whether the .DisplayName property is valid.
	/// </summary>
	public class IsAttributeDisplayNameValid : CompositeSpecification<Attribute> {

		/// <inheritdoc/>
		public override async Task<bool> IsSatisfiedByAsync( Attribute candidate ) {

			Messages.Clear();
			bool valid = true;

			var vm = DefinitionValidationHelper.IsValidNonEmptyString( "DisplayName", candidate.DisplayName );

			if (!vm.Valid) {
				Messages.Add( vm.Message );
				valid = false;
			}

			return valid;
		}
	}

	/// <summary>
	/// Tests whether the .Designation property is valid.
	/// </summary>
	public class IsAttributeDesignationValid : CompositeSpecification<Attribute> {

		/// <inheritdoc/>
		public override async Task<bool> IsSatisfiedByAsync( Attribute candidate ) {

			Messages.Clear();
			bool valid = true;

			if (candidate.Designation.Count == 0) {
				//Must be non empty
				Messages.Add( $"The Designation list must have one or more unique AttributeDesignations." );
				valid = false;
			} else if (candidate.Designation.Count != candidate.Designation.Distinct().Count()) {
				//Values must be unique
				Messages.Add( $"The Designation list must not contain duplicates." );
				valid = false;

			}

			return valid;
		}
	}

	/// <summary>
	/// Tests whether the .DefaultVisibility property is valid.
	/// </summary>
	public class IsAttributeDefaultVisibilityValid : CompositeSpecification<Attribute> {

		/// <inheritdoc/>
		public override async Task<bool> IsSatisfiedByAsync( Attribute candidate ) {

			Messages.Clear();
			bool valid = true;

			//The DefaultVisibility must be greater than or equal to the MaxVisibility
			switch (candidate.MaxVisibility) {
				case VisibilityOption.PUBLIC:
					//DefaultVisibility may be anything, so always passes
					break;

				case VisibilityOption.PROTECTED:
					switch (candidate.DefaultVisibility) {
						case VisibilityOption.PRIVATE:
						case VisibilityOption.INTERNAL:
						case VisibilityOption.PROTECTED:
							valid = true;
							break;
						case VisibilityOption.PUBLIC:
							valid = false;
							break;
					}
					break;

				case VisibilityOption.INTERNAL:
					switch (candidate.DefaultVisibility) {
						case VisibilityOption.PRIVATE:
						case VisibilityOption.INTERNAL:
							valid = true;
							break;
						case VisibilityOption.PROTECTED:
						case VisibilityOption.PUBLIC:
							valid = false;
							break;
					}
					break;

				case VisibilityOption.PRIVATE:
					switch (candidate.DefaultVisibility) {
						case VisibilityOption.PRIVATE:
							valid = true;
							break;
						case VisibilityOption.PUBLIC:
						case VisibilityOption.INTERNAL:
						case VisibilityOption.PROTECTED:
							valid = false;
							break;
					}
					break;
			}

			if (!valid) {
				Messages.Add( $"The DefaultVisibility must be greater than or equal to the MaxVisibility. Instead the DefaultVisibilityh is {candidate.DefaultVisibility.Description()} and the MaxVisibility is {candidate.MaxVisibility.Description()}" );
			}

			return valid;
		}
	}


	/// <summary>
	/// Tests whether the FieldName value in .Fields are unique.
	/// </summary>
	public class IsAttributeFieldNameUnique : CompositeSpecification<Attribute> {

		/// <inheritdoc/>
		public override async Task<bool> IsSatisfiedByAsync( Attribute candidate ) {

			Messages.Clear();
			bool valid = true;

			List<string> fieldNames = new List<string>();

			foreach (var field in candidate.Fields) {
				if (!fieldNames.Contains( field.FieldName )) {
					fieldNames.Add( field.FieldName );
				} else {
					valid = false;
					Messages.Add( $"In the Fields list, the FieldName '{field.FieldName}' is used more than once. Each AttributeField must have a unique FieldName" );
				}
			}

			return valid;
		}
	}

	/// <summary>
	/// Tests whether there is at least one AttributeField.
	/// </summary>
	public class IsAttributeFieldNotEmpty : CompositeSpecification<Attribute> {

		/// <inheritdoc/>
		public override async Task<bool> IsSatisfiedByAsync( Attribute candidate ) {

			Messages.Clear();
			bool valid = true;

			if (candidate.Fields == null || candidate.Fields.Count == 0) {
				valid = false;
				Messages.Add( "Fields must contain at least one AttributeField. Currently it is empty." );
			}

			return valid;
		}
	}

	/// <summary>
	/// Tests whether the Default Value on each AttributeField is valid.
	/// </summary>
	public class IsAttributeFieldDefaultValueValid : CompositeSpecification<Attribute> {

		/// <inheritdoc/>
		public override async Task<bool> IsSatisfiedByAsync( Attribute candidate ) {

			Messages.Clear();
			bool valid = true;

			if (candidate.Fields != null) {
				foreach (var field in candidate.Fields) {
					if ( field.FieldType == DataModel.Definitions.FieldType.CLOSED
						|| field.FieldType == DataModel.Definitions.FieldType.SUGGEST ) {
						bool isAValueOption = false;

						foreach (var option in field.Values) {
							if (option.Value == field.DefaultValue) {
								isAValueOption = true;
							}
						}

						if ( !isAValueOption) {
							valid = false;
							Messages.Add( $"The AttributeField '{field.FieldName}' DefaultValue is not listed as one of the optional Values. Instead have '{field.DefaultValue}.'" );
						}
					}
				}
			}
			//Not catching if Fields is null, as IsAttributeFieldNotEmpty does this.

			return valid;
		}
	}
}
