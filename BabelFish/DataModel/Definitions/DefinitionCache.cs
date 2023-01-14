using System.Text;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Reflection;

namespace Scopos.BabelFish.DataModel.Definitions
{
    [Serializable]
    public class DefinitionCache
    {
        [JsonProperty(Order = 12)]
        [DefaultValue(null)]
        public SetName? SetName { get; set; } = null;

        [JsonProperty(Order = 13)]
        [DefaultValue(null)]
        public DefinitionType? DefinitionType { get; set; } = null;

        [JsonProperty(Order = 14)]
        [DefaultValue("")]
        public DateTime LastUpdated { get; set; } = new DateTime();

        [JsonProperty(Order = 15)]
        [DefaultValue("")]
        public string DefinitionJSON { get; set; } = string.Empty;

        public DefinitionCache DeepCopy(DefinitionCache newDefinitionCache)
        {
            DefinitionCache returnDefinitionCache = new DefinitionCache();
            object obj = newDefinitionCache;
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (var p in properties)
                returnDefinitionCache.GetType().GetProperty(p.Name).SetValue(returnDefinitionCache, p.GetValue(obj), null);

            return returnDefinitionCache;
        }

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("Definition cache for ");
            foo.Append(SetName);
            return foo.ToString();
        }

    }
}
