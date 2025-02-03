using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Helpers {
    public static class Initializer {

        /// <summary>
        /// Sets the x-api-key used thorughout Babelfish, and initializes the JSON serializers and deserializers.
        /// </summary>
        /// <remarks>The Serializers and Deserializers found in Scopos.BabelFish.Helpers.SerializerOptions class. </remarks>
        /// <param name="xApiKey"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Initialize( string xApiKey ) {

            if (string.IsNullOrEmpty( xApiKey )) {
                throw new ArgumentNullException( "The x-api-key may not be empty or null." );
            }

            Scopos.BabelFish.Runtime.Settings.XApiKey = xApiKey;

            SerializerOptions.InitSerializers();
        }
    }
}
