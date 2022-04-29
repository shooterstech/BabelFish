using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BabelFish.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DefinitionFieldTypeEnums { OPEN, CLOSED, SUGGEST, DERIVED };

    [JsonConverter(typeof(StringEnumConverter))]
    public enum VisibilityOption { PRIVATE, PUBLIC, PROTECTED };

    /// <summary>
    /// UserSettings named Auth Enums
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum AuthEnums
    {
        [Description("")]
        [EnumMember(Value = "")]
        BLANK,
        [Description("User Name")]
        [EnumMember(Value = "username")]
        UserName,
        [Description("Password")]
        [EnumMember(Value = "password")]
        PassWord,
        [Description("AWS Access Key")]
        [EnumMember(Value = "accesskey")]
        AccessKey,
        [Description("AWS Secret Key")]
        [EnumMember(Value = "secretkey")]
        SecretKey,
        [Description("AWS Session Token")]
        [EnumMember(Value = "sessiontoken")]
        SessionToken,
        [Description("AWS Refresh Token")]
        [EnumMember(Value = "refreshtoken")]
        RefreshToken,
        [Description("AWS IdToken")]
        [EnumMember(Value = "idtoken")]
        IdToken,
        [Description("AWS Access Token")]
        [EnumMember(Value = "accesstoken")]
        AccessToken,
        [Description("AWS Device Token")]
        [EnumMember(Value = "devicetoken")]
        DeviceToken,
    }

    public static class EnumHelper
    {
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
        /// Perform a deep Copy of the object, using Json as a serialization method. NOTE: Private members are not cloned using this method.
        /// Code copied from https://stackoverflow.com/questions/78536/deep-cloning-objects
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The copied object.</returns>
        public static T Clone<T>( this T source ) {
            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals( source, null )) return default;

            // initialize inner objects individually
            // for example in default constructor some list property initialized with some values,
            // but in 'source' these items are cleaned -
            // without ObjectCreationHandling.Replace default constructor values will be added to result
            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };

            return JsonConvert.DeserializeObject<T>( JsonConvert.SerializeObject( source ), deserializeSettings );
        }
    }
}
