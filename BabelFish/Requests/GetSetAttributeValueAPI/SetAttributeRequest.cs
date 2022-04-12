using System.Text;


namespace BabelFish.Requests.GetSetAttributeValueAPI
{
    public class SetAttributeValueRequest : Request
    {
        private const string ParamName = "attribute-def";
        private Dictionary<string, List<string>> AttributeList = new Dictionary<string, List<string>>();
        private List<Dictionary<string, dynamic>> AttributeValues { get; set; } = new List<Dictionary<string, dynamic>>();

        public SetAttributeValueRequest(List<Dictionary<string,dynamic>> postParameters, List<string> queryParameters = null)
        {
            WithAuthentication = true;
            if ( queryParameters != null)
            {
                AttributeList.Add(ParamName, new List<string>());
                queryParameters.ForEach(x => AttributeList[ParamName].Add(x));
            }
            if (postParameters != null)
                AttributeValues = postParameters;
        }

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/users/attribute-value"; }
        }

        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {
                return AttributeList;
            }
        }

        public override StringContent PostParameters
        {
            get
            {
                //I don't think this will work...
                StringBuilder serializedJSON = new StringBuilder();
                //serializedJSON.Append("{\"attribute-values\":{");
                //foreach ( Dictionary<string,dynamic> val in AttributeValues)
                //{
                //    serializedJSON.Append($"\"{val["attribute-def"]}\": {{");
                //    foreach ( KeyValuePair<string,dynamic> kvp in val) {
                //        serializedJSON.Append($"\"{kvp.Key}\": ");
                //        if (kvp.Key == "attribute-value")
                //            foreach (KeyValuePair<string, string> kvp2 in kvp.Value)
                //                serializedJSON.Append($"\"{kvp2.Key}\": \"{kvp2.Value}\",");

                //        else

                //    }
                //    serializedJSON.Append("}");
                //}
                //serializedJSON.Append("}");
                return new StringContent(serializedJSON.ToString(), Encoding.UTF8, "application/json");
            }
        }
    }
}