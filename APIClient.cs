using BabelFish.Requests;

namespace BabelFish {
    public abstract class APIClient  {

        public APIClient( string xapikey ) {
            this.XApiKey = xapikey;
        }

        public string XApiKey { get; set; }
    }
}