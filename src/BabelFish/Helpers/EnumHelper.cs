using System.ComponentModel;
using System.Reflection;
using System.Runtime.Serialization;
using Scopos.BabelFish.DataModel.Common;


namespace Scopos.BabelFish.Helpers {

    /// <summary>
    /// Helper class to define methods to return an enum's Description value, as well as parse an enum by it's Description.
    /// </summary>

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

        public static string MemberValue( Enum value ) {
            var type = value.GetType();
            var name = Enum.GetName( type, value );

            if (name == null)
                return value.ToString();

            var field = type.GetField( name );
            var attr = field?.GetCustomAttribute<EnumMemberAttribute>();

            return attr?.Value ?? name;
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

        /// <summary>
        /// Attempts to parse the passed in string into an enum of type <T> by matching the string to the Description attribute of the enum values. Returns true if a match is found, false otherwise.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The string value to match against the Description attributes of the enum values.</param>
        /// <param name="result">The resulting enum value if a match is found.</param>
        /// <returns>True if a match is found, false otherwise.</returns>
        public static bool TryParseEnumByDescription<T>( this string value, out T result ) {
            result = default( T );

            if (string.IsNullOrEmpty( value )) {
                return false;
            }

            foreach (var field in typeof( T ).GetFields()) {
                var attr = Attribute.GetCustomAttribute( field, typeof( DescriptionAttribute ) ) as DescriptionAttribute;
                if (attr != null) {
                    if (attr.Description == value) {
                        result = (T)field.GetValue( null );
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Generates a 32 bit hash code for a list of enums.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enums"></param>
        /// <returns></returns>
        public static int GetHashValue<TEnum>( this IEnumerable<TEnum> enums ) where TEnum : struct, Enum {
            //NOTE: Using GetHashValue since GetHashCode already has a well known meaning
            unchecked {
                int hash = 17;
                foreach (var e in enums) {
                    hash = hash * 31 + Convert.ToInt32( e );
                }
                return hash;
            }
        }

        /// <summary>
        /// Helper method to parse a string into a VisibilityOption enum. Defaults to PRIVATE if no match is found.
        /// <para>Expected values include: "Public", "Internal", "Protected", "Private" (case insensitive)</para>
        /// </summary>
        /// <param name="visibilityOptionStr"></param>
        /// <returns></returns>
        public static VisibilityOption ParseVisibilityOption( string visibilityOptionStr ) {
            switch (visibilityOptionStr) {
                case "Public":
                case "PUBLIC":
                    return VisibilityOption.PUBLIC;
                case "Internal":
                case "INTERNAL":
                    return VisibilityOption.INTERNAL;
                case "Protected":
                case "PROTECTED":
                    return VisibilityOption.PROTECTED;
                case "Private":
                case "PRIVATE":
                default:
                    return VisibilityOption.PRIVATE;
            }
        }

        /// <summary>
        /// Helper method to parse an integer into a Visibility Option enum. Defaults to PRIVATE if no match is made.
        /// </summary>
        /// <param name="visibilityOption"></param>
        /// <returns></returns>
        public static VisibilityOption ParseVisibilityOption( int visibilityOption ) {
            switch (visibilityOption) {
                case 4:
                    return VisibilityOption.PUBLIC;
                case 3:
                    return VisibilityOption.INTERNAL;
                case 2:
                    return VisibilityOption.PROTECTED;
                case 1:
                default:
                    return VisibilityOption.PRIVATE;
            }
        }
    }
}
