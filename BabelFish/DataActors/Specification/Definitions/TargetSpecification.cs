using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataActors.Specification.Definitions {
	public class IsTargetValid : CompositeSpecification<Target> {

		public override async Task<bool> IsSatisfiedByAsync( Target candidate ) {

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
			var scoringRingsDescending = new IsTargetScoringRingsDescending();
			if (!await scoringRingsDescending.IsSatisfiedByAsync(candidate))
			{
				valid = false;
				Messages.AddRange(scoringRingsDescending.Messages);
			}


            return valid;
		}

	
	}

    public class IsTargetScoringRingsDescending : CompositeSpecification<Target>
    {

        public override async Task<bool> IsSatisfiedByAsync(Target candidate)
        {
			var valid = true;
            Messages.Clear();

			List<ScoringRing> rings = candidate.ScoringRings;
            for( int i = 0; i < rings.Count-1; ++i)
			{
				if (rings[i].Value < rings[i + 1].Value)
				{
					Messages.Add($"Scoring rings must be descending. {rings[i + 1].Value} is greater than {rings[i].Value}.");
					valid = false;
					break; 
				}
			}

            return valid;
        }


    }


}
