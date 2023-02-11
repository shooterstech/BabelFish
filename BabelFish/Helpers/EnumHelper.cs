using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Scopos.BabelFish.Helpers {

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum AttributeValueActionEnums { EMPTY, UPDATE, DELETE };

    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum AttributeValueKeyEnum {
        [Description( "attribute-values" )]
        [EnumMember( Value = "attribute-values" )]
        ATTRIBUTEVALUEKEY
    }

    /// <summary>
    /// UserSettings named Auth Enums
    /// </summary>
    [JsonConverter( typeof( StringEnumConverter ) )]
    public enum AuthEnums {
        [Description( "" )]
        [EnumMember( Value = "" )]
        BLANK,
        [Description( "User Name" )]
        [EnumMember( Value = "username" )]
        UserName,
        [Description( "Password" )]
        [EnumMember( Value = "password" )]
        PassWord,
        [Description( "AWS Access Key" )]
        [EnumMember( Value = "accesskey" )]
        AccessKey,
        [Description( "AWS Secret Key" )]
        [EnumMember( Value = "secretkey" )]
        SecretKey,
        [Description( "AWS Session Token" )]
        [EnumMember( Value = "sessiontoken" )]
        SessionToken,
        [Description( "AWS Refresh Token" )]
        [EnumMember( Value = "refreshtoken" )]
        RefreshToken,
        [Description( "AWS IdToken" )]
        [EnumMember( Value = "idtoken" )]
        IdToken,
        [Description( "AWS Access Token" )]
        [EnumMember( Value = "accesstoken" )]
        AccessToken,
        [Description( "AWS Device Token" )]
        [EnumMember( Value = "devicetoken" )]
        DeviceToken,
        [Description( "XAPIKey" )]
        [EnumMember( Value = "xapikey" )]
        XApiKey,
    }



    public static class EnumHelper {
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example><![CDATA[string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;]]></example>
        public static T GetAttributeOfType<T>( this Enum enumVal ) where T : System.Attribute {
            var type = enumVal.GetType();
            var memInfo = type.GetMember( enumVal.ToString() );
            var attributes = memInfo[0].GetCustomAttributes( typeof( T ), false );
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        // This method creates a specific call to the above method, requesting the
        // Description MetaData attribute.
        /// <summary>
        /// Returns the Description attribute of an enum. If Description is not an attribute
        /// returns the .toString() value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Description( this Enum value ) {
            var attribute = value.GetAttributeOfType<DescriptionAttribute>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        /*
         * To use the above method on an enum, need to declare the enum like the following
         * public enum MyEnum {
         *   [Description("value with a space") valuewithaspace
         * }
         */


        /// <summary>
        /// Retrieve <T>Enum matching Description text
        /// https://stackoverflow.com/questions/10955517/get-enum-value-by-description
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseEnumByDescription<T>( this string value ) {
            T returnEnum = default( T );
            foreach (var field in typeof( T ).GetFields()) {
                var attr = Attribute.GetCustomAttribute( field, typeof( DescriptionAttribute ) ) as DescriptionAttribute;
                if (attr != null) {
                    if (attr.Description == value) {
                        returnEnum = (T)field.GetValue( null );
                        break;
                    }
                }
            }
            return returnEnum;
        }
    }
}
