using System.Reflection;
using System.IO;
using NLog;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.Responses;
using Scopos.BabelFish.Requests;
using System.Net;
using System.Text.Json;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.APIClients
{
    public class ResponseCache {
		private readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

		/// <summary>
		/// This is where previously called response/request objects
		/// will be stored in memory. The key is specific to the request. The value is the 
		/// Response object (which contains the Request object).
		/// </summary>
		private Dictionary< string, ResponseIntermediateObject> cachedRequests = new Dictionary< string, ResponseIntermediateObject>();

        private object mutex = new object();

        public static ResponseCache CACHE= new ResponseCache();

		private ResponseCache() {

        }

        /// <summary>
        /// The directory that BabelFish may use to store cached responses. 
        /// </summary>
        [Obsolete("Replaced with API Client's File System Cache")]
        public DirectoryInfo? LocalStoreDirectory { get; set; } = null;

        /// <summary>
        /// Attempts to retreive a Response from memory based on the passed in request value. 
        /// Returns true if there was a cache hit. 
        /// False is returned if either the response value is not cached or the response
        /// value has expired.
        /// </summary>
        /// <param name="definitionType"></param>
        /// <param name="setName"></param>
        /// <param name="definition"></param>
        /// <returns></returns>
        public bool TryGetResponse( Request request, out ResponseIntermediateObject value ) {
            var key = request.GetRequestCacheKey();
            lock (mutex) {
                if (cachedRequests.TryGetValue( key, out value )) {
                    if (value.ValidUntil > DateTime.UtcNow) {
                        return true;
                    } else {
                        cachedRequests.Remove( key );
                    }
                }
            }
            value = null;
            return false;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="definition"></param>
        public void SaveResponse(  ResponseIntermediateObject response ) {

            var request = response.Request;
            if (response.ValidUntil > DateTime.UtcNow) {
                lock (mutex) {
                    cachedRequests[request.GetRequestCacheKey()] = response;

                    if ( LocalStoreDirectory != null ) {
                        string relativePath = request.GetRequestCacheKey();
                        relativePath = relativePath.Replace( ':', ' ' );

						string filename = $"{LocalStoreDirectory.FullName}\\{relativePath}.json";

						FileInfo file = new FileInfo( filename );
                        file.Directory.Create();

                        var json = JsonSerializer.Serialize( response, SerializerOptions.APIClientSerializer );

						using (StreamWriter sw = File.CreateText( file.FullName )) {
							sw.WriteLine( json );
						}

					}
                }
            }
        }

        /// <summary>
        /// Returns the number of requests that are cached.
        /// </summary>
        public int Count {
            get {
                return cachedRequests.Count;
            }
        }

        /// <summary>
        /// Removes cached items that are no longer valid. 
        /// </summary>
        public void CleanUp() {
            List<string> keysToRemove = new List<string>();
            lock (mutex) {
                foreach( var item in cachedRequests ) {
                    if( item.Value.ValidUntil < DateTime.UtcNow ) {
                        keysToRemove.Add( item.Key );
                    }
                }

                foreach( var key in keysToRemove ) {
                    cachedRequests.Remove( key );
                }
            }
        }

        /// <summary>
        /// Removes all items from cache
        /// </summary>
        public void Clear() {
            lock(mutex) {
                cachedRequests.Clear();
            }
        }
    }

    public class ResponseIntermediateObject {


        public Request Request { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public MessageResponse MessageResponse { get; set; }

        public JsonDocument Body { get; set; }

        public DateTime ValidUntil { get; set; }
    }
}