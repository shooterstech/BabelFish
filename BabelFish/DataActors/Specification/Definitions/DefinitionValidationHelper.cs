using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataActors.Specification.Definitions {

    public static class DefinitionValidationHelper {

        private static DefinitionAPIClient definitionAPIClient = new DefinitionAPIClient();
        public static ClubsAPIClient ClubsAPIClient = new ClubsAPIClient();

        public static ValidationModel IsValidNonEmptyString( string propertyName, string value ) {

            if (string.IsNullOrEmpty( value )) {
                var message = $"{propertyName} is required and may not be null or an empty string.";
                return new ValidationModel( false, message );
            }

            return new ValidationModel( true, string.Empty );
        }

        public static async Task<ValidationModel> IsValidSetNameAndExistsAsync( string propertyName, string setNameUnderTest, DefinitionType definitionType ) {

            if (string.IsNullOrEmpty( setNameUnderTest )) {
                var message = $"{propertyName} is required and may not be null or an empty string.";
                return new ValidationModel( false, message ) ;
            }

            SetName setName;
            if (! SetName.TryParse( setNameUnderTest, out setName )) {
                var message = $"{propertyName} value '{setNameUnderTest}' is not a correctly formatted SetName.";
                return new ValidationModel( false, message );
            }

            if (!await IsValidDefinitionAsync( setName, definitionType )) {
                var message = $"{propertyName} value '{setNameUnderTest}' is not a known {definitionType} Definition or it is discontinued.";
                return new ValidationModel( false, message );
            }

            return new ValidationModel( true, string.Empty ) ;
        }

        /// <summary>
        /// Returns a boolean if the passed in setName is a known Definition.
        /// May return false if the Definition could not be found, if the Definition
        /// is known but has been discontinued.
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="definitionType"></param>
        /// <returns></returns>
        /// <exception cref="ScoposAPIException">Thrown if there was a communication or server error.</exception>
        private static async Task<bool> IsValidDefinitionAsync( SetName setName, DefinitionType definitionType ) {

            
            var response = await definitionAPIClient.GetDefinitionVersionPublicAsync( definitionType, setName );

            if (response.RestApiStatusCode == System.Net.HttpStatusCode.NotFound)
                return false;

            if (response.RestApiStatusCode != System.Net.HttpStatusCode.OK) {
                throw new ScoposAPIException( $"Received Status Code {response.RestApiStatusCode} when retreiving the Sparse Definition for {definitionType} {setName}." );
            }

            var sd = response.SparseDefinition;
            return ! sd.Discontinued;
        }
    }

    public class ValidationModel {

        public ValidationModel() { }

        public ValidationModel( bool valid, string message ) {
            this.Valid = valid;
            this.Message = message;
        }

        public bool Valid { get; set; }
        public string Message { get; set; }
    }
}
