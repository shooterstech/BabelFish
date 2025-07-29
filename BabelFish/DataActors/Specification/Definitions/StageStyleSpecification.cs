using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataActors.Specification;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataActors.Specification.Definitions {


    public class IsStageStyleValid : CompositeSpecification<StageStyle> {

        /// <inheritdoc />
        public override async Task<bool> IsSatisfiedByAsync( StageStyle candidate ) {

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

            if (!await version.IsSatisfiedByAsync( candidate ) )
            {
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


            //StageStyle specific fields
            var shotsInSeries = new IsStageStyleShotsInSeriesValid();
            var scoreFormatCollectionDef = new IsStageStyleScoreFormatCollectionDefValid();
            var scoreConfigDefault = new IsStageStyleScoreConfigDefaultValid();
            var relatedStageStyles = new IsStageStyleRelatedStageStylesValid();

            if ( ! await shotsInSeries.IsSatisfiedByAsync( candidate ) ) {
                valid = false;
                Messages.AddRange( shotsInSeries.Messages );
            }

            if (!await scoreFormatCollectionDef.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( scoreFormatCollectionDef.Messages );
            } else {

                //Only run IsScoreConfigDefaultValid if IsScoreFormatCollectionDefValid is valid
                if (!await scoreConfigDefault.IsSatisfiedByAsync( candidate )) {
                    valid = false;
                    Messages.AddRange( scoreConfigDefault.Messages );
                }
            }

            if (!await relatedStageStyles.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( relatedStageStyles.Messages );
            }

            return valid;
        }
    }

    /// <summary>
    /// Tests whether the ShotsInSeries property is valid in the passed in StageStyle instance.
    /// </summary>
    public class IsStageStyleShotsInSeriesValid : CompositeSpecification<StageStyle> {
            
        /// <inheritdoc />
        public override async Task<bool> IsSatisfiedByAsync( StageStyle candidate ) {
            Messages.Clear();

            bool valid = true;
            if (candidate.ShotsInSeries <= 0 ) {
                valid = false;
                Messages.Add( "ShotsInSeries must be greater than 0." );
            }

            if (candidate.ShotsInSeries > 100 ) {
                valid = false;
                Messages.Add( "ShotsInSeries must be less than or equal to 100." );
            }

            return valid;
        }
    }

    /// <summary>
    /// Tests whether the ScoreFormatCollectionDef property is valid in the passed in StageStyle instance.
    /// </summary>
    public class IsStageStyleScoreFormatCollectionDefValid : CompositeSpecification<StageStyle> {
        public override async Task<bool> IsSatisfiedByAsync( StageStyle candidate ) {
            Messages.Clear();

            var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( "ScoreFormatCollectionDef", 
                candidate.ScoreFormatCollectionDef, 
                DefinitionType.SCOREFORMATCOLLECTION );

            if (!vm.Valid) {
                Messages.Add( vm.Message );
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Tests whether the ScoreConfigDefault property is valid in the passed in StageStyle instance.
    /// </summary>
    public class IsStageStyleScoreConfigDefaultValid : CompositeSpecification<StageStyle> {
        public override async Task<bool> IsSatisfiedByAsync( StageStyle candidate ) {
            Messages.Clear();
            //Generally assumes ScoreFormatCollection is valid

            var vm = DefinitionValidationHelper.IsValidNonEmptyString( "ScoreConfigDefault", candidate.ScoreConfigDefault );
            if ( ! vm.Valid ) {
                Messages.Add( vm.Message );
                return false;
            }

            try {
                var scoreFormatCollection = await candidate.GetScoreFormatCollectionDefinitionAsync();
                if (!scoreFormatCollection.GetScoreConfigNames().Contains( candidate.ScoreConfigDefault )) {
                    Messages.Add( $"ScoreConfigDefault must have a value that's defined in the SCORE FORMAT COLLECTION '{candidate.ScoreFormatCollectionDef}'. The valid values are {string.Join( ", ", scoreFormatCollection.GetScoreConfigNames() )}" );
                    return false;
                }
            } catch (Exception ex) {
                Messages.Add( $"Couldn't validate ScoreConfigDefault. Caught exception {ex}." );
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// Tests whether the RelatedStageStyles property is valid in the passed in StageStyle instance.
    /// </summary>
    public class IsStageStyleRelatedStageStylesValid : CompositeSpecification<StageStyle> {

        public override async Task<bool> IsSatisfiedByAsync( StageStyle candidate ) {
            Messages.Clear();

            var valid = true;

            foreach (var rss in candidate.RelatedStageStyles) {

                var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync( "RelatedStageStyles",
                    rss,
                    DefinitionType.STAGESTYLE );

                if ( ! vm.Valid ) {
                    valid = false;
                    Messages.Add( vm.Message );
                }

                //Should we add a rule that says a RelatedStageStyle can't point to itself? 
                //If we did, likely have to compare Hierarchical names (and not SetNames).
            }

            return valid;
        }
    }

}
