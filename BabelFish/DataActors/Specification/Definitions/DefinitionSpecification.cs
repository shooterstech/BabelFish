using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Definitions;

/*
 * File contain s CompositeSpecification classes for the shared 
 * properties found in the abstract class Definition.
 * 
 * There is no need to create Specificaiton classes for the properties
 * Discipline (it's an enum)
 * Discontinued (it's a boolean)
 */

namespace Scopos.BabelFish.DataActors.Specification.Definitions {

    /// <summary>
    /// Tests whether the HierarchicalName property is valid in the passed in Definition instance.
    /// </summary>
    public class IsDefinitionHierarchicalNameValid : CompositeSpecification<Definition> {

        /// <inheritdoc />
        public override async Task<bool> IsSatisfiedByAsync( Definition candidate ) {
            Messages.Clear();

            if (string.IsNullOrEmpty( candidate.HierarchicalName )) {
                Messages.Add( $"HierarchicalName is required and may not be null or an empty string." );
                return false;
            }

            HierarchicalName hierarchicalName;
            if (! HierarchicalName.TryParse( candidate.HierarchicalName, out hierarchicalName )) {
                Messages.Add( $"HierarchicalName value '{candidate.HierarchicalName}' is not a correctly formatted HierarchicalName." );
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Tests whether the Version property is valid in the passed in Definition instance.
    /// Does not test if it is a meaningful version value, only that it parses as expected.
    /// </summary>
    public class IsDefiniitonVersionValid : CompositeSpecification<Definition> {

        /// <inheritdoc />
        public override async Task<bool> IsSatisfiedByAsync( Definition candidate ) {
            Messages.Clear();

            var vm = DefinitionValidationHelper.IsValidNonEmptyString( "Version", candidate.Version );
            if (!vm.Valid) {
                Messages.Add( vm.Message );
                return false;
            }

            try {
                DefinitionVersion dv = new DefinitionVersion( candidate.Version );
            } catch ( ArgumentException ae ) {
                Messages.Add( $"The value for Version '{candidate.Version}' is not in the expected format." );
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Tests whether the CommonName property is valid in the passed in Definition instance.
    /// </summary>
    public class IsDefiniitonCommonNameValid : CompositeSpecification<Definition> {

        /// <inheritdoc />
        public override async Task<bool> IsSatisfiedByAsync( Definition candidate ) {
            Messages.Clear();

            var vm = DefinitionValidationHelper.IsValidNonEmptyString( "CommonName", candidate.CommonName );
            if (!vm.Valid) {
                Messages.Add( vm.Message );
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Tests whether the Description property is valid in the passed in Definition instance.
    /// </summary>
    public class IsDefiniitonDescriptionValid : CompositeSpecification<Definition> {

        /// <inheritdoc />
        public override async Task<bool> IsSatisfiedByAsync( Definition candidate ) {
            Messages.Clear();

            var vm = DefinitionValidationHelper.IsValidNonEmptyString( "Description", candidate.Description );
            if (!vm.Valid) {
                Messages.Add( vm.Message );
                return false;
            }

            return true;
        }
    }

    /*
     * TODO: 
     * Validate JSONVersion
     */

    /// <summary>
    /// Tests whether the Owner property is valid in the passed in Definition instance.
    /// </summary>
    public class IsDefiniitonOwnerValid : CompositeSpecification<Definition> {

        /// <inheritdoc />
        public override async Task<bool> IsSatisfiedByAsync( Definition candidate ) {

            //The Owner property is valid, if it matches the namespace of the HierarchicalName.

            //First check the value of Owner is not an empty string or null;
            var vm = DefinitionValidationHelper.IsValidNonEmptyString( "Owner", candidate.Owner );
            if ( ! vm.Valid ) {
                Messages.Add( vm.Message );
                return false;
            }

            //The .GetClubDetail call includes a list of namespaces this owner owns.
            var clubDetailResponse = await DefinitionValidationHelper.ClubsAPIClient.GetClubDetailPublicAsync( candidate.Owner );

            if (clubDetailResponse.StatusCode != System.Net.HttpStatusCode.OK) {
                Messages.Add( $"Could not validate Owner, because could not successfully make the GetClubDetailPublicAsync REST API call." );
                return false;
            }

            var clubDetail = clubDetailResponse.ClubDetail;

            HierarchicalName hierarchicalName;
            if (!HierarchicalName.TryParse( candidate.HierarchicalName, out hierarchicalName )) {
                Messages.Add( $"Could not validate Owner, because the HierarchicalName value '{candidate.HierarchicalName}' could not be parsed." );
                return false;
            }

            foreach( var ownedNamespace in clubDetail.NamespaceList ) {
                if (ownedNamespace.Namespace == hierarchicalName.Namespace) {
                    return true;
                }
            }

            Messages.Add( $"Could not validate Owner '{candidate.Owner},' because the namespace listed in the HierarchicalName value '{candidate.HierarchicalName}' is not owned by the Owner." );
            return false;

        }
    }

    /// <summary>
    /// Tests whether the Subdiscipline property is valid in the passed in Definition instance.
    /// </summary>
    public class IsDefiniitonSubdisciplineValid : CompositeSpecification<Definition> {

        /// <inheritdoc />
        public override async Task<bool> IsSatisfiedByAsync( Definition candidate ) {
            Messages.Clear();

            //Value can just not be null. Will check for null, and instead of returning an error, will just set to an empty string.
            if (candidate.Subdiscipline == null)
                candidate.Subdiscipline = string.Empty;

            return true;
        }
    }

    /// <summary>
    /// Tests whether the Tags property is valid in the passed in Definition instance.
    /// </summary>
    public class IsDefiniitonTagsValid : CompositeSpecification<Definition> {

        /// <inheritdoc />
        public override async Task<bool> IsSatisfiedByAsync( Definition candidate ) {
            Messages.Clear();

            //Value can just not be null or empty list. Will check for null, and instead of returning an error, will just remove the offending values.
            if (candidate.Tags.Contains( null ) || candidate.Tags.Contains( string.Empty ) ) {
                List<string> newTags = new List<string>();
                foreach (var tag in candidate.Tags ) 
                {
                    if ( ! string.IsNullOrEmpty( tag ) )
                    newTags.Add( tag );
                }
                candidate.Tags = newTags;
            }

            return true;
        }
    }

    /// <summary>
    /// Tests whether the Comment property is valid in the passed in IReconfigurableRulebookObject instance.
    /// </summary>
    /// <remarks>NOTE: Because the Comment property exists on all RR objects, using IReconfigurableRulebook oject instead of a Definition in the definition nof the generic.</remarks>
    public class IsCommentValid : CompositeSpecification<IReconfigurableRulebookObject> {

        /// <inheritdoc />
        public override async Task<bool> IsSatisfiedByAsync( IReconfigurableRulebookObject candidate ) {
            Messages.Clear();

            //Value can just not be null. An empty stirng is valid.
            if (candidate.Comment == null) {
                Messages.Add( "The Comment property may not be null. It may however be an empty string." );
                return true;
            }

            return true;
        }
    }
}
