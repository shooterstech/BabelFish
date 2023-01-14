using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Scopos.BabelFish.Helpers
{
    //https://stackoverflow.com/questions/5546142/how-do-i-use-json-net-to-deserialize-into-nested-recursive-dictionary-and-list/19140420#19140420
    public static class JsonHelper
    {
        public static object Deserialize(string json)
        {
            return ToObject(JToken.Parse(json));
        }

        public static object ToObject(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    return token.Children<JProperty>()
                                .ToDictionary(prop => prop.Name,
                                              prop => ToObject(prop.Value));

                case JTokenType.Array:
                    return token.Select(ToObject).ToList();

                default:
                    return ((JValue)token).Value;
            }
        }

        public static string ToJsonString(HttpResponseMessage response) 
        {
            using (Stream s = response.Content.ReadAsStreamAsync().Result)
            using (StreamReader sr = new StreamReader(s))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                return JObject.ReadFrom(reader).ToString();
            }
        }

        public static string FieldValueFromJson(string stringJson, string fieldName)
        {
            string returnValue = string.Empty;
            List<string> ignoreNames = new List<string>() { "Title", "Message", "ResponseCodes" };

            JObject o = JObject.Parse(stringJson);
            string otype = o.Type.ToString();
            foreach (JProperty property in o.Properties())
            {
                if (!ignoreNames.Contains(property.Name))
                {
                    JObject o2 = JObject.Parse(property.Value.ToString());
                    foreach (JProperty property2 in o2.Properties())
                    {
                        if ( property2.Name == fieldName)
                            returnValue = property2.Value.ToString();
                    }
                }
            }

            return returnValue;
        }
    }
}
