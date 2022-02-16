using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.Helpers
{
    internal class EnumHelper
    {
        /// <summary>
        /// Translate enum to usable value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumVal"></param>
        /// <returns></returns>
        public static T GetAttributeOfType<T>(Enum enumVal) where T : System.Attribute
        {
            //sauce: https://social.msdn.microsoft.com/Forums/en-US/6477d813-9e02-4641-866d-e6a3f06f9eb9/how-to-get-enummember-value
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}
