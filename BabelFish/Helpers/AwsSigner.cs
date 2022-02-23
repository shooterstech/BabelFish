using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime;
using Aws4RequestSigner;
using AwsSignatureVersion4;

namespace BabelFish.Helpers
{
    public class AwsSigner
    {
        public AwsSigner(string accessKey, string secretKey, string uri, string sessiontoken = "")
        {
            AccessKey = accessKey;
            SecretKey = secretKey;
            SessionToken = sessiontoken;
            URI = uri;
        }

        #region Properties
        private string AccessKey { get; set; } = string.Empty;
        private string SecretKey { get; set; } = string.Empty;
        private string SessionToken { get; set; } = string.Empty; 
        private string URI { get; set; } = string.Empty;
        #endregion Properties

        #region Methods
        /// <summary>
        /// Entry point to call different Signature generators
        /// </summary>
        /// <param name="which">Aws4RequestSigner, AwsSignatureVersion4</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> GetAws4Signature(string which = "")
        {
            string SignatureToRun= which;
            try
            {
                switch (SignatureToRun)
                {
                    case "Aws4RequestSigner":
                        return await Aws4RequestSigner().ConfigureAwait(false);
                    case "AwsSignatureVersion4":
                        return await AwsSignatureVersion4().ConfigureAwait(false);
                }
            }
            finally { }
            return new HttpResponseMessage();
        }

        private async Task<HttpResponseMessage> Aws4RequestSigner()
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage();
                var signer = new AWS4RequestSigner(this.AccessKey, this.SecretKey);
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var signrequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(this.URI),
                    Content = null
                };
                request = await signer.Sign(signrequest, "execute-api", "us-east-1").ConfigureAwait(false);
                //return request;

                //HttpResponseMessage response = await httpClient.client.SendAsync(request).ConfigureAwait(false);
                HttpResponseMessage response = await httpClient.client.SendAsync(new HttpRequestMessage(HttpMethod.Get, this.URI)).ConfigureAwait(false);
                return response;
            }
            finally { }
        }

        private async Task<HttpResponseMessage> AwsSignatureVersion4()
        {
            try
            {
                var credentials = new ImmutableCredentials(this.AccessKey, this.SecretKey, this.SessionToken);

                var client = new HttpClient();
                var response = await client.GetAsync(
                    this.URI,
                    regionName: "us-east-1",
                    serviceName: "execute-api",
                    credentials: credentials).ConfigureAwait(false);

                return response;
                //var returnedJson = response.Content.ReadAsStringAsync().Result;
            }
            finally { }
        }

        #endregion Methods
        }
}
