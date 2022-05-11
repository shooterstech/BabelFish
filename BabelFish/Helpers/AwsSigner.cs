using System;
using System.Web;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BabelFish.DataModel;
using BabelFish.DataModel.Authentication.Credentials;

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

        public AwsSigner(string uri, StringContent messageContent = null, DateTime? continuationToken = null) {
            URI = uri;
            if ( continuationToken != null)
                Credentials.ContinuationToken = continuationToken;

            // Request Content for msg.Content in signing
            if ( messageContent != null )
                MessageContent = messageContent;
        }

        #region Properties

        private HttpResponseMessage responseError = new HttpResponseMessage();

        private Credential Credentials = new Credential();

        public DateTime? ContinuationToken { get { return Credentials.ContinuationToken; } }

        /// <summary>
        /// Endpoint URI for hashing algorithm
        /// </summary>
        private string URI { get; set; } = string.Empty;

        private StringContent MessageContent { get; set; } = new StringContent("");
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
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                bool checkTokens = await DeterminePermTempTokens().ConfigureAwait(false);
                if (!checkTokens)
                    response = responseError;
                else
                {
                    if ( this.URI != "" )
                        response = await AwsSignatureV4RichW().ConfigureAwait(false);
                    else
                    {
                        response = httpClient.GenerateHttpResponseMessage(HttpStatusCode.OK, $"\"RefreshToken\":\"" + Credentials.RefreshToken + "\", \"IdToken\":\"" + Credentials.IdToken + "\", \"AccessToken\":\"" + Credentials.AccessToken + "\", \"DeviceToken\":\"" + Credentials.DeviceToken + "\", }");
                    }
                }
            }
            catch (Exception ex)
            {
                response = httpClient.GenerateHttpResponseError(HttpStatusCode.OK, "401", "Get Signature Error", ex.Message);
            }

            return response;
        }

        /// <summary>
        /// RichW's custom code (URL requires privs to Shooter's Tech GitHub)
        /// https://github.com/shooterstech/Genesis/blob/36be2492dc137d03b1e74c06c14ddbdcf71ce6b7/Genesis.Prime/Services/MatchSearchService.cs0
        /// </summary>
        /// <returns>HttpResponseMessage object to parse</returns>
        private async Task<HttpResponseMessage> AwsSignatureV4RichW()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {

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
                msg.Headers.Add("x-amz-date", amzLongDate);
                if ( ! Credentials.IsPermToken() )
                    msg.Headers.Add("X-Amz-Security-Token",Credentials.SessionToken);
                // Add Body Content
                msg.Content = MessageContent;
                var payloadHash = Hash(msg.Content.ReadAsByteArrayAsync().Result);
                msg.Headers.Add("x-amz-content-sha256", payloadHash);

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
                canonicalRequest.Append(payloadHash);
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
            }
            catch (Exception ex)
            {
                response = httpClient.GenerateHttpResponseError(HttpStatusCode.OK, "401", "Get Signature Error", ex.Message);
            }
            return response;
        }
        private static string GetCanonicalQueryParams(HttpRequestMessage request)
        {
            var values = new SortedDictionary<string, string>();

            //AKS-20220324 Refactor for multiple values per key
            var querystring = HttpUtility.ParseQueryString(request.RequestUri.Query);
            foreach (var key in querystring.AllKeys) {
                if (key == null) { // Handles keys without values
                    values.Add(Uri.EscapeDataString(querystring[key]), $"{Uri.EscapeDataString(querystring[key])}=");
                } else {
                    foreach (var value in querystring.GetValues(key)) {
                        // Escape to upper case. Required.
                        values.Add(Uri.EscapeDataString(key)+value.ToString(), $"{Uri.EscapeDataString(key)}={Uri.EscapeDataString(value)}");
                    }
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

        private async Task<bool> DeterminePermTempTokens() {
            
            // Set Temporary Tokens if we don't receive perm
            if ( !Credentials.IsPermToken() ) {
                // swap null to default date so system knows to use temp tokens on first round
                if (Credentials.ContinuationToken == null)
                    Credentials.ContinuationToken = new DateTime();

                bool validTempCreds = await Credentials.GetTempCredentials().ConfigureAwait(false);
                if (!validTempCreds)
                {
                    responseError = httpClient.GenerateHttpResponseError(HttpStatusCode.OK, "401", "Auth Err", Credentials.LastException);
                    return false;
                }
            }
            else {
                // Set Permanent Tokens (AccessKey,SecretKey) passed in from UserSettings
                // Treating set AccessKey, SecretKey as permanent tokens
                Credentials.AccessKey = SettingsHelper.UserSettings[AuthEnums.AccessKey.ToString()];
                Credentials.SecretKey = SettingsHelper.UserSettings[AuthEnums.SecretKey.ToString()];
                Credentials.ContinuationToken = null;
            }
            return true;
        }
        #endregion Methods
    }
}