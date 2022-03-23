using System;
using System.Web;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime;
using Aws4RequestSigner;
using AwsSignatureVersion4;
using BabelFish.DataModel;
using BabelFish.DataModel.Credentials;

namespace BabelFish.Helpers
{
    public class AwsSigner
    {
        /////////////
        // https://docs.aws.amazon.com/general/latest/gr/signing_aws_api_requests.html
        // https://docs.aws.amazon.com/general/latest/gr/signature-version-4.html
        // https://docs.aws.amazon.com/general/latest/gr/sigv4-create-canonical-request.html
        // You can use temporary security credentials provided by the AWS Security Token Service(AWS STS)
        //  to sign a request.The process is the same as using long-term credentials, but when you add
        //  signing information to the query string you must add an additional query parameter for the security token.
        //  The parameter name is X-Amz-Security-Token, and the parameter's value is the URI-encoded session token
        //  (the string you received from AWS STS when you obtained temporary security credentials).
        /////////////

        public AwsSigner(string uri) {
            URI = uri;
        }

        #region Properties

        private DataModel.Credentials.Credential Credentials = new Credential();

        /// <summary>
        /// Endpoint URI for hashing algorithm
        /// </summary>
        private string URI { get; set; } = string.Empty;
        
        #endregion Properties

        #region Methods

        /// <summary>
        /// Entry point to call different Signature generators
        /// </summary>
        /// <param name="which">Aws4RequestSigner, AwsSignatureVersion4, AwsSignatureV4RichW</param>
        /// <returns>HttpResponseMessage object to parse</returns>
        public async Task<HttpResponseMessage> GetAws4Signature(string which = "")
        {
            string SignatureToRun = which;
            try
            {
                DeterminePermTempTokens();

                switch (SignatureToRun)
                {
                    case "Aws4RequestSigner":
                        return await Aws4RequestSigner().ConfigureAwait(false);
                    case "AwsSignatureVersion4":
                        return await AwsSignatureVersion4().ConfigureAwait(false);
                    case "AwsSignatureV4RichW":
                        return await AwsSignatureV4RichW().ConfigureAwait(false);
                }
            }
            finally { }

            return new HttpResponseMessage();
        }

        /// <summary>
        /// AWS4RequestSigner
        /// Sign HttpRequestMessage using AWS Signature v4 using request information and credentials.
        /// https://github.com/tsibelman/aws-signer-v4-dot-net
        /// https://www.nuget.org/packages/Aws4RequestSigner/
        /// </summary>
        /// <returns>HttpResponseMessage object to parse</returns>
        private async Task<HttpResponseMessage> Aws4RequestSigner()
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                HttpRequestMessage request = new HttpRequestMessage();
                var signer = new AWS4RequestSigner(Credentials.AccessKey, Credentials.SecretKey);
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var signrequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(this.URI),
                    Content = null
                };
                request = await signer.Sign(signrequest, "execute-api", "us-east-1").ConfigureAwait(false);

                response = httpClient.SendAsync(request).Result;
                
                return response;
            }
            finally { }
        }

        //second one Adam found with SessionToken
        //https://github.com/FantasticFiasco/aws-signature-version-4
        private async Task<HttpResponseMessage> AwsSignatureVersion4()
        {
            try
            {
                var credentials = new ImmutableCredentials(Credentials.AccessKey, Credentials.SecretKey, Credentials.SessionToken);
                //var credentials = new ImmutableCredentials(Credentials.AccessKey, Credentials.SecretKey, null);
                var client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(
                    this.URI,
                    regionName: "us-east-1",
                    serviceName: "execute-api",
                    credentials: credentials).ConfigureAwait(false);

                return response;
                //var returnedJson = response.Content.ReadAsStringAsync().Result;
            }
            finally { }
        }

        /// <summary>
        /// RichW's custom code (URL requires privs to Shooter's Tech GitHub)
        /// https://github.com/shooterstech/Genesis/blob/36be2492dc137d03b1e74c06c14ddbdcf71ce6b7/Genesis.Prime/Services/MatchSearchService.cs0
        /// </summary>
        /// <returns>HttpResponseMessage object to parse</returns>
        private async Task<HttpResponseMessage> AwsSignatureV4RichW()
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();

                string awsRegion = "us-east-1";
                string awsServiceName = "execute-api";
                string awsSecretKey = Credentials.SecretKey;
                DateTimeOffset utcNowSaved = DateTimeOffset.UtcNow;
                string amzLongDate = utcNowSaved.ToString("yyyyMMddTHHmmssZ");
                string amzShortDate = utcNowSaved.ToString("yyyyMMdd");
                string url = this.URI;
                // Prepare request message
                HttpRequestMessage msg = new HttpRequestMessage(HttpMethod.Get, url);
                msg.Headers.Host = msg.RequestUri.Host;
                // Add to headers. 
                msg.Headers.Add("x-amz-date", amzLongDate);
                if ( !Credentials.IsPermToken())
                    msg.Headers.Add("X-Amz-Security-Token",Credentials.SessionToken);
                // Add Body Content
                msg.Content = new StringContent("");
                // Create Canonical Request
                var canonicalRequest = new StringBuilder();
                canonicalRequest.Append(msg.Method + "\n");
                canonicalRequest.Append(string.Join("/", msg.RequestUri.AbsolutePath.Split('/').Select(Uri.EscapeDataString)) + "\n");
                canonicalRequest.Append(GetCanonicalQueryParams(msg) + "\n"); // Query params to do.
                List<string> headersToBeSigned = new List<string>();
                foreach (var header in msg.Headers.OrderBy(a => a.Key.ToLowerInvariant(), StringComparer.OrdinalIgnoreCase))
                {
                    canonicalRequest.Append(header.Key.ToLowerInvariant());
                    canonicalRequest.Append(":");
                    canonicalRequest.Append(string.Join(",", header.Value.Select(s => s.Trim())));
                    canonicalRequest.Append("\n");
                    headersToBeSigned.Add(header.Key.ToLowerInvariant());
                }
                canonicalRequest.Append("\n");
                var signedHeaders = string.Join(";", headersToBeSigned);
                canonicalRequest.Append(signedHeaders + "\n");
                canonicalRequest.Append(Hash(msg.Content.ReadAsByteArrayAsync().Result));
                // String to sign
                string stringToSign = "AWS4-HMAC-SHA256" + "\n" + amzLongDate +
                                      "\n" + amzShortDate + "/" + awsRegion + "/" +
                                      awsServiceName + "/aws4_request" + "\n" +
                                      Hash(Encoding.UTF8.GetBytes(canonicalRequest.ToString()));
                var dateKey = HmacSha256(Encoding.UTF8.GetBytes("AWS4" + awsSecretKey), amzShortDate);
                var dateRegionKey = HmacSha256(dateKey, awsRegion);
                var dateRegionServiceKey = HmacSha256(dateRegionKey, awsServiceName);
                var signingKey = HmacSha256(dateRegionServiceKey, "aws4_request");
                var signature = ToHexString(HmacSha256(signingKey, stringToSign));
                var credentialScope = amzShortDate + "/" + awsRegion + "/" + awsServiceName + "/aws4_request";
                msg.Headers.TryAddWithoutValidation("Authorization", "AWS4-HMAC-SHA256 Credential=" + Credentials.AccessKey +
                    "/" + credentialScope + ", SignedHeaders=" + signedHeaders + ", Signature=" + signature);
                response = httpClient.SendAsync(msg).Result;

                return response;
            }
            finally { }
        }
        private static string GetCanonicalQueryParams(HttpRequestMessage request)
        {
            var values = new SortedDictionary<string, string>();

            var querystring = HttpUtility.ParseQueryString(request.RequestUri.Query);
            foreach (var key in querystring.AllKeys)
            {
                if (key == null) // Handles keys without values
                {
                    values.Add(Uri.EscapeDataString(querystring[key]), $"{Uri.EscapeDataString(querystring[key])}=");
                }
                else
                {
                    // Escape to upper case. Required.
                    values.Add(Uri.EscapeDataString(key), $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(querystring[key])}");
                }
            }
            // Put in order - this is important.
            var queryParams = values.Select(a => a.Value);
            return string.Join("&", queryParams);
        }
        private static string Hash(byte[] bytesToHash)
        {
            return ToHexString(SHA256.Create().ComputeHash(bytesToHash));
        }
        private static string ToHexString(IReadOnlyCollection<byte> array)
        {
            var hex = new StringBuilder(array.Count * 2);
            foreach (var b in array)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
        private static byte[] HmacSha256(byte[] key, string data)
        {
            return new HMACSHA256(key).ComputeHash(Encoding.UTF8.GetBytes(data));
        }

        private async void DeterminePermTempTokens() {
            
            // Set Temporary Tokens if we don't receive perm
            if (!Credentials.IsPermToken()) {
                // swap null to default date so system knows to use temp tokens on first round
                if (Credentials.ContinuationToken == null)
                    Credentials.ContinuationToken = new DateTime();

                Credentials.Username = SettingsHelper.UserSettings[AuthEnums.UserName.ToString()];
                Credentials.Password = SettingsHelper.UserSettings[AuthEnums.PassWord.ToString()];
                bool validTempCreds = await Credentials.GetTempCredentials();
            }
            else {
                // Set Permanent Tokens (AccessKey,SecretKey) passed in from UserSettings
                // Treating set AccessKey, SecretKey as permanent tokens
                Credentials.AccessKey = SettingsHelper.UserSettings[AuthEnums.AccessKey.ToString()];
                Credentials.SecretKey = SettingsHelper.UserSettings[AuthEnums.SecretKey.ToString()];
                Credentials.ContinuationToken = null;
            }
        }
        #endregion Methods
    }
}