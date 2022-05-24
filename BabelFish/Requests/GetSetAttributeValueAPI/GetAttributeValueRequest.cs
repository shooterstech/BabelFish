namespace BabelFish.Requests.GetSetAttributeValueAPI
{
    public class GetAttributeValueRequest : Request
    {
        private const string ParamName = "attribute-def";
        private Dictionary<string, List<string>> AttributeList = new Dictionary<string, List<string>>();

        public GetAttributeValueRequest(List<string> attributeNames)
        {
            WithAuthentication = true;
            AttributeList.Add(ParamName,new List<string>());
            attributeNames.ForEach(x => AttributeList[ParamName].Add(x));
            AttributeList.Add("return-default", new List<string>() { "true" });
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
    }
}