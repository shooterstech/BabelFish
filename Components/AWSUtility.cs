using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace BabelFish.Components
{
    static class AWSUtility
    {

        public enum ProtocolEnum
        {
            https,
            http
        }

        public enum RequestMethodEnum
        {
            GET,
            POST
        }
        public static string ValidRequestMethodEnum(string requestMethod)
        {
            if (Enum.IsDefined(typeof(RequestMethodEnum), requestMethod))
                return requestMethod;
            else
                return String.Empty;
        }

        public enum ResponseTypeEnum
        {
            json,
            xml,
            text
        }
        public static string ValidResponseTypeEnum(string responseType)
        {
            if (Enum.IsDefined(typeof(ResponseTypeEnum), responseType))
                return responseType;
            else
                return String.Empty;
        }

        public const string DomainName = "orionscoringsystem.com";


        private static readonly List<string> Subdomain = new List<string>(){"api", "api-stage"};
        public static string ValidateSubdomain(string subdomain)
        {
            if (Subdomain.Contains(subdomain))
                return subdomain;
            else
                return String.Empty;
        }
    }
}
