using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using BabelFish.Components.Objects;


namespace BabelFish.Components
{
	static class httpClient
    {
		public static readonly HttpClient client = new HttpClient();

		public static void ResetHttpClient()
        {
			client.DefaultRequestHeaders.Clear();
        }
	}

	public class AWSApi
	{
		#region Properties
		private AWSRequestObject RequestObject = new AWSRequestObject();
		private AWSResponseObject ResponseObject = new AWSResponseObject();
		private List<string> ErrorList = new List<string>();
		private StringBuilder FullURL = new StringBuilder();
		#endregion Properties

		public AWSApi(){}

        #region Methods
		public async Task<AWSResponseObject> GetAsync(AWSRequestObject requestObject)
        {
			ErrorList.Clear();
			httpClient.ResetHttpClient();

			RequestObject = requestObject;
			BuildURL();
			BuildHeaders();

			//SendRequest
			if ( ErrorList.Count == 0 )
            {
                try
                {
					using (HttpResponseMessage response = await httpClient.client.GetAsync(FullURL.ToString()).ConfigureAwait(false))
                    {
						if ( response.IsSuccessStatusCode)
                        {
							using (HttpContent content = response.Content)
							{
								ResponseObject.SerializedResponse = await content.ReadAsStringAsync();
							}
                        }
                        else
                        {
							ErrorList.Add("API status failed with: " + response.StatusCode + ": " + response.ReasonPhrase);
							ResponseObject.Errors = ErrorList;
                        }
					}
				}
				catch (Exception ex)
                {
					ErrorList.Add("API execute failed with: " + ex.Message);
					ResponseObject.Errors = ErrorList;
				}
            }
            else
            {
				ResponseObject.Errors = ErrorList;
			}

			return ResponseObject;
		}

		private void BuildURL()
        {
            try
            {
				FullURL.Clear();

				FullURL.Append(AWSUtility.ProtocolEnum.https); // Force https always?
				FullURL.AppendLine("://");

				if (RequestObject.Subdomain == String.Empty)
					ErrorList.Add("Invalid Subdomain");
				else
					FullURL.AppendLine(RequestObject.Subdomain);

				FullURL.AppendLine(AWSUtility.DomainName);

				if (String.IsNullOrEmpty(RequestObject.Environment))
					ErrorList.Add("Invalid Environment");
				else
					FullURL.AppendLine(RequestObject.Environment);

				FullURL.AppendLine(RequestObject.UrlQuery);
			} 
			catch (Exception ex)
            {
				ErrorList.Append("Error assembling URL for HTTP Request: " + ex.Message);
            }

		}

		private void BuildHeaders()
        {
            try
            {
				foreach (KeyValuePair<String, String> kvp in RequestObject.RequestHeaders)
				{
					httpClient.client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
				}
			}
			catch (Exception ex)
            {
				ErrorList.Add("Error adding Headers: " + ex.Message);
            }
		}
		#endregion Methods
	}
}