using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BabelFish.Requests;
using BabelFish.Components;
using Newtonsoft.Json.Linq;
using NLog;

namespace BabelFish {
    public abstract class APIClient {

        [JsonConverter( typeof( StringEnumConverter ) )]
        public enum APIStage {
            [Description( "alpha" )] [EnumMember( Value = "alpha" )] ALPHA,
            [Description( "beta" )] [EnumMember( Value = "beta" )] BETA,
            [Description( "production" )] [EnumMember( Value = "production" )] PRODUCTION
        }

        private JsonSerializer serializer = new JsonSerializer();
        private Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public APIClient( string xapikey ) {
            this.XApiKey = xapikey;
            ApiStage = APIStage.PRODUCTION;
        }

        public string XApiKey { get; set; }

        public APIStage ApiStage { get; set; }

        protected async void CallAPI<T>(Request request, Response<T> response)
        {
            //TODO make the hostname a variable
            string uri =
                $"https://api-stage.orionscoringsystem.com/{ApiStage.ToString().ToLower()}{request.RelativePath}?{request.QueryString}#{request.Fragment}";

            try
            {
                HttpResponseMessage responseMessage = 
                    await httpClient.GetAsyncWithHeaders(uri,
                    new Dictionary<string, string>() { { "x-api-key", XApiKey } });
                //TODO: Put headers in Request so more can be appended?, threw Dict here to get basics running

                //HttpResponseMessage responseMessage = httpClient.GetAsync(uri).Result; //make it err, no x-api-key header
                response.StatusCode = responseMessage.StatusCode;

                //Convert the returned body to an object of type T
                if (responseMessage.IsSuccessStatusCode)
                {
                    using (Stream s = responseMessage.Content.ReadAsStreamAsync().Result)
                    using (StreamReader sr = new StreamReader(s))
                    using (JsonReader reader = new JsonTextReader(sr))
                    {
                        var returnedJson = JObject.ReadFrom(reader);

                        response.Value = returnedJson.ToObject<T>();

                        //TODO set response.Body
                    }

                    response.Body = responseMessage.Content.Headers.ToString();
                }
                else
                {
                    response.Body = responseMessage.ReasonPhrase;
                    //TODO: add nLog error here
                }
            }
            catch (Exception ex)
            {
                //TODO: add nLog error here
                //Add response.StatusCode, response.Body error message here?
                throw new NotImplementedException("Handle the case the GetAsync method returns an error");
            }
        }
    }
}