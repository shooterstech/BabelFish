using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Helpers.Extensions;
using System.Text.RegularExpressions;

namespace Scopos.BabelFish.DataActors.Specification.Definitions {
	public class IsResultListFormatValid : CompositeSpecification<ResultListFormat> {

		public override async Task<bool> IsSatisfiedByAsync( ResultListFormat candidate ) {

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
            var scoreFormatCollectionDef = new IsResultListFormatScoreFormatCollectionDefValid();
            var scoreConfigDefault = new IsResultListFormatScoreConfigDefaultValid();
			var fieldNames = new IsResultListFormatBodyValuesValid();
			var spanningColumns = new IsResultListFormatHaveAtMostOneSpanningColumn();

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

			if (!await fieldNames.IsSatisfiedByAsync( candidate )) {
				valid = false;
				Messages.AddRange( fieldNames.Messages );
			}

			if (!await spanningColumns.IsSatisfiedByAsync( candidate )) {
				valid = false;
                Messages.AddRange( fieldNames.Messages );
            }

            return valid;
		}
	}

	public class IsResultListFormatScoreFormatCollectionDefValid : CompositeSpecification< ResultListFormat> {

        public override async Task<bool> IsSatisfiedByAsync( ResultListFormat candidate ) {

			var vm = await DefinitionValidationHelper.IsValidSetNameAndExistsAsync(
				"ScoreFormatCollectionDef",
				candidate.ScoreFormatCollectionDef,
				DefinitionType.SCOREFORMATCOLLECTION );

            if (!vm.Valid) {
                Messages.Add( vm.Message );
                return false;
            }

            return true;
        }
    }
    public class IsResultListFormatScoreConfigDefaultValid : CompositeSpecification<ResultListFormat> {
        public override async Task<bool> IsSatisfiedByAsync( ResultListFormat candidate ) {
            Messages.Clear();
            //Generally assumes ScoreFormatCollection is valid

            var vm = DefinitionValidationHelper.IsValidNonEmptyString( "ScoreConfigDefault", candidate.ScoreConfigDefault );
            if (!vm.Valid) {
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

	public class IsResultListFormatBodyValuesValid : CompositeSpecification<ResultListFormat> {

        public override async Task<bool> IsSatisfiedByAsync( ResultListFormat candidate ) {

			bool valid = true;
			Messages.Clear();

			var validFieldValues = candidate.GetFieldNames();
			var pattern = @"\{(.*?)\}";
			List<string> extractedValues = new List<string>();

			int columnIndex = 0;
			foreach( var column in candidate.Format.Columns ) {

				//Validate the FieldNames in .Body
				if (!string.IsNullOrEmpty( column.Body )) {

					//Retreive a list of all the string values from Body that are to be interpolated
					var matches = Regex.Matches( column.Body, pattern );
					extractedValues.Clear();

					foreach (Match match in matches) {
						extractedValues.Add( match.Groups[1].Value );
					}

					//Check that each of them is a valid field value
					foreach (var potentialFieldValue in extractedValues) {
						if ( ! validFieldValues.Contains( potentialFieldValue ) ) {
							valid = false;
							Messages.Add( $"Format.Columns[{columnIndex}].Body contains a FieldName value '{potentialFieldValue}' that is unknown." );
						}
					}
                }

				//Validate the FieldNames in .Child
                if (!string.IsNullOrEmpty( column.Child )) {

                    //Retreive a list of all the string values from Child that are to be interpolated
                    var matches = Regex.Matches( column.Child, pattern );
                    extractedValues.Clear();

                    foreach (Match match in matches) {
                        extractedValues.Add( match.Groups[1].Value );
                    }

                    //Check that each of them is a valid field value
                    foreach (var potentialFieldValue in extractedValues) {
                        if (!validFieldValues.Contains( potentialFieldValue )) {
                            valid = false;
                            Messages.Add( $"Format.Columns[{columnIndex}].Body contains a FieldName value '{potentialFieldValue}' that is unknown." );
                        }
                    }
                }

                columnIndex++;
			}

			return valid;
        }
    }

    /// <summary>
    /// Tests if a RESULT LIST FORMAT has 0 or 1 columns with Spanning text.
    /// </summary>
    public class IsResultListFormatHaveAtMostOneSpanningColumn : CompositeSpecification<ResultListFormat> {

        public override async Task<bool> IsSatisfiedByAsync( ResultListFormat candidate ) {

            bool valid = true;
            Messages.Clear();

			List<int> columnsWithSpanning = new List<int>();

			//Count the number of columns with Spanning text. 
            int columnIndex = 0;
            foreach (var column in candidate.Format.Columns) {

				if ( column.Spanning is not null && ! column.Spanning.IsEmpty)
					columnsWithSpanning.Add( columnIndex++ );

                columnIndex++;
            }

            //There can be 0 or 1.
            if (columnsWithSpanning.Count > 1 ) {
				valid = false;

				var list = string.Join( ",", columnsWithSpanning );
				var msg = $"More than one column was detected with spanning text. You may have 0 or 1, but no more. The columns with spanning text are indexes {list}.";
				Messages.Add( msg );
			}

            return valid;
        }
    }

    //TODO: Only one column has spanning text
}
