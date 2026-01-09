using System.Text.RegularExpressions;
using Scopos.BabelFish.DataModel.Definitions;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Scopos.BabelFish.DataActors.Specification.Definitions {
#pragma warning restore IDE0130 // Namespace does not match folder structure

    /// <summary>
    /// Specification class for RESULT LIST FORMAT.
    /// </summary>
    public class IsResultListFormatValid : CompositeSpecification<ResultListFormat> {

        /// <summary>
        /// Method to call to check if a RESULT LIST FORMAT instance meets specifications.
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
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
            var scoreConfigNames = new IsResultListFormatFieldScoreFormatValid();

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

            if (!await scoreConfigNames.IsSatisfiedByAsync( candidate )) {
                valid = false;
                Messages.AddRange( scoreConfigNames.Messages );
            }

            return valid;
        }
    }

    /// <summary>
    /// Specification class that checks one specific rule for RESULT LIST FORMAT. Namely, it checks that the SetName defined in
    /// ScoreFormatCollectionDef is a valid SCORE FORMAT COLLECTION.
    /// <para>To check if a RESULT LIST FORMAT instance passes all specifications use the IsResultListFormatValid class.</para>
    /// </summary>
    public class IsResultListFormatScoreFormatCollectionDefValid : CompositeSpecification<ResultListFormat> {

        /// <inheritdoc />
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

    /// <summary>
    /// Specification class that checks one specific rule for RESULT LIST FORMAT. Namely, it checks that the value for
    /// ScoreConfigDefault is a valid value.
    /// <para>To check if a RESULT LIST FORMAT instance passes all specifications use the IsResultListFormatValid class.</para>
    /// </summary>
    public class IsResultListFormatScoreConfigDefaultValid : CompositeSpecification<ResultListFormat> {

        /// <inheritdoc />
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

    /// <summary>
    /// Specification class that checks one specific rule for RESULT LIST FORMAT. Namely, checks that each interpolated FieldName
    /// is a known field. Either a defined field name in the .Fields list, or one of the standard field names.
    /// <para>To check if a RESULT LIST FORMAT instance passes all specifications use the IsResultListFormatValid class.</para>
    /// </summary>
    public class IsResultListFormatBodyValuesValid : CompositeSpecification<ResultListFormat> {

        /// <inheritdoc />
        public override async Task<bool> IsSatisfiedByAsync( ResultListFormat candidate ) {

            bool valid = true;
            Messages.Clear();

            var validFieldValues = candidate.GetFieldNames();
            var pattern = @"\{(.*?)\}";
            List<string> extractedValues = new List<string>();

            int columnIndex = 0;
            foreach (var column in candidate.Format.Columns) {

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
                        if (!validFieldValues.Contains( potentialFieldValue )) {
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
    /// Specification class that checks one specific rule for RESULT LIST FORMAT. Namely, checks if a RESULT LIST FORMAT has
    /// 0 or 1 columns with Spanning text.
    /// <para>To check if a RESULT LIST FORMAT instance passes all specifications use the IsResultListFormatValid class.</para>
    /// </summary>
    public class IsResultListFormatHaveAtMostOneSpanningColumn : CompositeSpecification<ResultListFormat> {

        /// <inheritdoc />
        public override async Task<bool> IsSatisfiedByAsync( ResultListFormat candidate ) {

            bool valid = true;
            Messages.Clear();

            List<int> columnsWithSpanning = new List<int>();

            //Count the number of columns with Spanning text. 
            int columnIndex = 0;
            foreach (var column in candidate.Format.Columns) {

                if (column.Spanning is not null && !column.Spanning.IsEmpty)
                    columnsWithSpanning.Add( columnIndex++ );

                columnIndex++;
            }

            //There can be 0 or 1.
            if (columnsWithSpanning.Count > 1) {
                valid = false;

                var list = string.Join( ",", columnsWithSpanning );
                var msg = $"More than one column was detected with spanning text. You may have 0 or 1, but no more. The columns with spanning text are indexes {list}.";
                Messages.Add( msg );
            }

            return valid;
        }
    }

    /// <summary>
    /// Specification class that checks one specific rule for RESULT LIST FORMAT. Namely, checks that the .ScoreFormat value,
    /// in each ResultListField instance, is a known ScoreFormatName.
    /// <para>To check if a RESULT LIST FORMAT instance passes all specifications use the IsResultListFormatValid class.</para>
    /// </summary>
    public class IsResultListFormatFieldScoreFormatValid : CompositeSpecification<ResultListFormat> {

        /// <inheritdoc />
        public override async Task<bool> IsSatisfiedByAsync( ResultListFormat candidate ) {

            bool valid = true;
            Messages.Clear();

            var sfc = await candidate.GetScoreFormatCollectionDefinitionAsync();
            var expectedValues = sfc.ScoreFormats;
            var expectedValuesAsString = string.Join( ", ", expectedValues );

            foreach (var field in candidate.Fields) {
                switch (field.Method) {
                    case ResultFieldMethod.SCORE:
                    case ResultFieldMethod.PROJECTED_SCORE:
                    case ResultFieldMethod.GAP:
                        if (field.Source is null) {
                            valid = false;
                            Messages.Add( $"ResultListField with name '{field.FieldName}' does not define a FieldSource." );
                        } else if (!expectedValues.Contains( field.Source.ScoreFormat )) {
                            valid = false;
                            Messages.Add( $"ResultListField with name '{field.FieldName}' has a ScoreFormat that is not defined by the SCORE FORMAT COLLECTION {candidate.ScoreFormatCollectionDef}. It instead must be one of the values {expectedValuesAsString}." );
                        }
                        break;
                    default:
                        break;
                }
            }

            return valid;

        }

    }
}
