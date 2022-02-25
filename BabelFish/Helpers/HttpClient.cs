using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using BabelFish.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BabelFish.Helpers
{
	static class httpClient
    {
		public static readonly HttpClient client = new HttpClient();

        /// <summary>
        /// GetAsync with no headers
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> GetAsync(string uri)
        {
			return httpClient.client.GetAsync(uri).Result;
		}

		/// <summary>
		/// GetAsyncWithHeaders adds headers to GET request as required by AWS
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="headers"></param>
		/// <returns></returns>
        public static async Task<HttpResponseMessage> GetAsyncWithHeaders(string uri,
            Dictionary<string, string> headers)
        {
			//https://makolyte.com/csharp-how-to-add-request-headers-when-using-httpclient/
			//DefaultRequestHeaders is not thread safe
            HttpRequestMessage newRequestMessage;
			using (newRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri))
            {
				foreach (KeyValuePair<string,string> kvp in headers)
					newRequestMessage.Headers.Add(kvp.Key, kvp.Value);

                return httpClient.client.SendAsync(newRequestMessage).Result;
            }
		}

        public static async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            HttpResponseMessage response = new HttpResponseMessage();

            try
            {
                response = await client.SendAsync(request).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
            }

            return response;
        }
    }
}