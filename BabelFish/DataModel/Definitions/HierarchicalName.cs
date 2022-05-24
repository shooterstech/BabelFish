using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Definitions {
    /// <summary>
    /// HierarchicalNames are the full names to Events, Stages, Rulebooks, Attributes, etc. They are separated into two parts
    /// Namespace and ProperName. Namespace represents the organization who came up with the entity its named after. For example
    /// orion, nra, usas, etc. ProperName is a more human readable name for the entity.
    /// </summary>
    [Serializable]
    public class HierarchicalName : IEquatable<HierarchicalName>, IComparable<HierarchicalName> {

        private string nameSpace = "";
        private string properName = "";

        public HierarchicalName() {

        }

        public HierarchicalName(string name) {
            FullName = name;
        }

        public HierarchicalName(string nameSpace, string name) {
            ProperName = name;
            Namespace = nameSpace;
        }

        /// <summary>
        /// Human readable name of the entity.
        /// </summary>
        public string ProperName {
            get { return properName; }
            set {
                properName = value.Trim();
            }
        }

        /// <summary>
        /// The organization who defined the entity. e.g. orion, nra, usas.
        /// </summary>
        public string Namespace {
            get { return nameSpace; }
            set { nameSpace = value.Trim(); }
        }

        [JsonIgnore]
        public string FullName {
            get { return ToString(); }
            set {
                var n = Decode(value);
                Namespace = n.Item1;
                ProperName = n.Item2;
            }
        }

        [JsonIgnore]
        public bool IsBlank {
            get {
                return (nameSpace == ""
                    && properName == "");
            }

        }
        
        public override string ToString() {
            if (nameSpace == "")
                return properName;

            StringBuilder sb = new StringBuilder();
            sb.Append(nameSpace);
            sb.Append(':');
            sb.Append(properName);
            return sb.ToString();
        }

        public static bool TryParse(string nameWithNamespace, out HierarchicalName hn) {
            try {
                hn = new HierarchicalName();
                hn.FullName = nameWithNamespace;
                return true;
            } catch (Exception ex) {
                hn = null;
                return false;
            }
        }

        private Tuple<string, string> Decode(string nameWithNamespace) {
            string[] words = { "" };
            char[] delimiterChars = { ':' };
            words = nameWithNamespace.Split(delimiterChars);
            if (words.Length == 2)
                return new Tuple<string, string>(words[0], words[1]);
            else if (words.Length == 1)
                return new Tuple<string, string>("", words[0]);
            else
                throw new FormatException("Unexpected HierarchialName format for " + nameWithNamespace);
        }

        #region IComparable<HierarchicalName> Members

        public int CompareTo(HierarchicalName other) {
            return this.ToString().CompareTo(other.ToString());
        }

        #endregion

        #region IEquatable<HierarchicalName> Members

        public bool Equals(HierarchicalName other) {
            return this.ToString() == other.ToString();
        }

        public override int GetHashCode() {
            int temp = this.ToString().GetHashCode();
            return temp;
        }
        #endregion
    }
}
