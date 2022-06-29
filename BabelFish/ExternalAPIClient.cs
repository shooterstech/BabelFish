using ShootersTech.Requests;

namespace ShootersTech.External {
    public class ExternalAPIClient : APIClient {

        public ExternalAPIClient( string zipcodeapikey ) : base( zipcodeapikey ) { }

        /// <summary>
        /// Get location data from US zip code
        /// </summary>
        /// <param name="zipcode"></param>
        /// <returns></returns>
        public async Task<GetExternalResponse> GetLocationDataFromZip( string zipcode ) {

            if (int.TryParse( zipcode, out int verifyzip )) //break this out into a helper function
            {
                GetExternalRequest requestParameters = new GetExternalRequest( XApiKey, zipcode );

                GetExternalResponse response = new GetExternalResponse( requestParameters );

                await this.CallAPI( requestParameters, response ).ConfigureAwait( false );

                return response;
            } else {
                throw new RequestException( $"Zipcode in unexpected format. Received '{zipcode}'." );
            }
        }
    }

    public class GetExternalRequest : Request {
        public GetExternalRequest( string apikey = "", string zipcode = "" ) {
            ApiKey = apikey;
            ZipCode = zipcode;
            IsShootersTechURI = false;
        }
        public string ZipCode { get; set; } = string.Empty;

        public string ApiKey { get; set; } = string.Empty;

        public override string RelativePath {
            get { return $"https://zipcodeapi.com/rest/{ApiKey}/info.json/{ZipCode}/degrees"; }
        }
    }
    public class GetExternalResponse : ShootersTech.Responses.Response<ZipCodeApi> {

        public GetExternalResponse( GetExternalRequest request ) :base() {
            this.Request = request;
        }

        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public ZipCodeApi ZipCodeApi {
            get { return Value; }
        }
    }
    public class ZipCodeApi {
        public string zip_code { get; set; } = string.Empty;
        public double lat { get; set; } = 0;
        public double lng { get; set; } = 0;
        public string city { get; set; } = string.Empty;
        public string state { get; set; } = string.Empty;
        public TimeZoneObject timezone { get; set; } = new TimeZoneObject();
        public List<string> acceptable_city_names { get; set; } = new List<string>();
        public List<int> area_codes { get; set; } = new List<int>();
    }
    public class TimeZoneObject {
        public string timezone_identifier { get; set; } = string.Empty;
        public string timezone_abbr { get; set; } = string.Empty;
        public string utc_offset_sec { get; set; } = string.Empty;
        public string is_dst { get; set; } = string.Empty;
    }
}
