using BabelFish.DataModel.Definitions;

namespace BabelFish.DataModel.GetSetAttributeValue
{
    internal class AttributeValueDefinition
    {
        DefinitionAPIClient? clientDefinition;
        
        public Definitions.Attribute AttributeDefinition = new Definitions.Attribute();

        public List<AttributeField> Fields
        {
            get
            {
                return AttributeDefinition.Fields;
            }
        }

        public bool MultipleValues
        {
            get 
            { 
                return AttributeDefinition.MultipleValues; 
            }
        }

        /// <summary>
        /// Checks Field's Required flag from AttributeDefinition
        /// </summary>
        /// <param name="fieldName">Field Name string from GetDefintionFields()</param>
        /// <returns>true or false</returns>
        public bool? IsFieldNameRequired(string fieldName)
        {
            try
            {
                AttributeField attrField = AttributeDefinition.Fields.Where(x => x.FieldName == fieldName).FirstOrDefault();
//                if (string.IsNullOrEmpty(attrField.FieldName))
//                    LastException = AttributeValueException.GetExceptionFieldNameError($"check Required for {fieldName} not found in {SetName}");
//                else
                    return attrField.Required;
            }
            catch (Exception ex)
            {
//              LastException = AttributeValueException.GetExceptionFieldNameError($"check Required for {fieldName} not found in {SetName}: {ex.ToString}");
            }
            return null;
        }

        /// <summary>
        /// Load Definition file for reference
        /// </summary>
        public async void LoadDefinition(string SetName)
        {
            try
            {
                clientDefinition = new DefinitionAPIClient(Helpers.SettingsHelper.UserSettings["XApiKey"], Helpers.SettingsHelper.RevertSettingsFormat());

                Responses.DefinitionAPI.GetDefinitionResponse<Definitions.Attribute> DefinitionResponse =
                    await clientDefinition.GetAttributeDefinitionAsync(Definitions.SetName.Parse(SetName)).ConfigureAwait(false);
                if (DefinitionResponse.Definition != null)
                    AttributeDefinition = DefinitionResponse.Definition;
            }
            catch (Exception ex)
            {
//                AttributeDefinition = AttributeValueException.GetExceptionDefinitionError($"Not Found for {SetName}");
            }
        }

    }
}
