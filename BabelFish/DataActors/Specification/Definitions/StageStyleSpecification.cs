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

            var shotsInSeries = new IsShotsInSeriesValid();
            var scoreFormatCollectionDef = new IsScoreFormatCollectionDefValid();
            var scoreConfigDefault = new IsScoreConfigDefaultValid();
            var relatedStageStyles = new IsRelatedStageStylesValid();

            var valid = true;

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
    public class IsShotsInSeriesValid : CompositeSpecification<StageStyle> {
            
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
    public class IsScoreFormatCollectionDefValid : CompositeSpecification<StageStyle> {
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
    public class IsScoreConfigDefaultValid : CompositeSpecification<StageStyle> {
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
    public class IsRelatedStageStylesValid : CompositeSpecification<StageStyle> {

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
