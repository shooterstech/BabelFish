using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BabelFish.Requests;
using Newtonsoft.Json.Linq;
using NLog;

namespace BabelFish {
    public abstract class APIClient {

        [JsonConverter(typeof(StringEnumConverter))]
        public enum APIStage {
            [Description("alpha")] [EnumMember(Value = "alpha")]
            ALPHA,

            [Description("beta")] [EnumMember(Value = "beta")]
            BETA,

            [Description("production")] [EnumMember(Value = "production")]
            PRODUCTION
        }

        private HttpClient httpClient = new HttpClient();
        private JsonSerializer serializer = new JsonSerializer();
        private Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public APIClient(string xapikey) {
            this.XApiKey = xapikey;
            ApiStage = APIStage.PRODUCTION;
        }

        public string XApiKey { get; set; }

        public APIStage ApiStage { get; set; }

        protected void CallAPI<T>(Request request, Response<T> response) {
            //TODO make this function async

            //TODO make the hostname a variable
            string uri =
                $"https://api-stage.orionscoringsystem.com/{ApiStage}{request.RelativePath}?{request.QueryString}#{request.Fragment}";

            HttpResponseMessage responseMessage = null;
            string responseString = "";
            try {
                responseMessage = httpClient.GetAsync(uri).Result;
                response.StatusCode = responseMessage.StatusCode;

                //Convert the returned body to an object of type T
                if (responseMessage.IsSuccessStatusCode) {
                    using (Stream s = responseMessage.Content.ReadAsStreamAsync().Result)
                    using (StreamReader sr = new StreamReader(s))
                    using (JsonReader reader = new JsonTextReader(sr)) {
                        var returnedJson = JObject.ReadFrom(reader);

                        response.Value = returnedJson.ToObject<T>();

                        //TODO set response.Body
                    }
                } else {
                    throw new NotImplementedException("Handle the case the response code is not a success");
                }
            } catch (Exception ex) {
                throw new NotImplementedException("Handle the case the GetAsync method returns an error");
            }
        }
    }
}