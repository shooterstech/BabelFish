using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.Helpers
{
    static class StringHelper
    {
        public static string ErrorTextExpanded<T>(T error)
        {
            if (error is HttpStatusCode)
            {
                switch (error)
                {
                    case HttpStatusCode.Forbidden:
                        return "Invalid or missing XApiKey for this operation. Please check the key provided or contact support.";
                    case HttpStatusCode.NotFound:
                        return "The Id received was not found. Please check the ID and try again.";
                    default:
                        return $"An unidentified error occurred: {error.ToString()}. Please contact support if you continue to get this error.";
                }
            }

            return "";
        }

    }
}
