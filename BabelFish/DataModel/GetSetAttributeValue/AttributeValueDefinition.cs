using Scopos.BabelFish.APIClients;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.GetSetAttributeValue {
    internal class AttributeValueDefinition {
        DefinitionAPIClient? clientDefinition;

        public Definitions.Attribute AttributeDefinition = new Definitions.Attribute();

        public List<AttributeField> Fields {
            get {
                return AttributeDefinition.Fields;
            }
        }

        public bool MultipleValues {
            get {
                return AttributeDefinition.MultipleValues;
            }
        }

        /// <summary>
        /// Checks Field's Required flag from AttributeDefinition
        /// </summary>
        /// <param name="fieldName">Field Name string from GetDefintionFields()</param>
        /// <returns>true or false</returns>
        public bool? IsFieldNameRequired( string fieldName ) {
            try {
                AttributeField attrField = AttributeDefinition.Fields.Where( x => x.FieldName == fieldName ).FirstOrDefault();
                //                if (string.IsNullOrEmpty(attrField.FieldName))
                //                    LastException = AttributeValueException.GetExceptionFieldNameError($"check Required for {fieldName} not found in {SetName}");
                //                else
                return attrField.Required;
            } catch (Exception ex) {
                //              LastException = AttributeValueException.GetExceptionFieldNameError($"check Required for {fieldName} not found in {SetName}: {ex.ToString}");
            }
            return null;
        }

    }
}
