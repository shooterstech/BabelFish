using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShootersTech.DataModel.Definitions {
    public class SetName {

        private int majorVersion = 0;
        private int minorVersion = 0;
        private string nameSpace = "";
        private string properName = "";

        private SetName() { }

        /*
        [JsonConstructor]
        public SetName(string setName) {
            try {
                var foo = setName.Split(':');
                string version = foo[0];
                this.nameSpace = foo[1];
                this.properName = foo[2];

                var bar = version.Substring(1).Split('.');
                this.majorVersion = int.Parse(bar[0]);
                this.minorVersion = int.Parse(bar[1]);
            } catch (Exception ex) {
                majorVersion = 0;
                minorVersion = 0;
                nameSpace = "";
                properName = "";
            }

        }
        */

        public static bool TryParseVersion(string version, out int majorVersion, out int minorVersion) {

            try {
                var bar = version.StartsWith("v") ? version.Substring(1).Split('.') : version.Split('.');
                majorVersion = int.Parse(bar[0]);
                minorVersion = int.Parse(bar[1]);
                return true;
            } catch(Exception ex) {
                majorVersion = 1;
                minorVersion = 1;
                return false;
            }
        }

        public static SetName Parse(string setName) {

            SetName sn = new SetName();
            var foo = setName.Split(':');
            string version = foo[0];
            sn.nameSpace = foo[1];
            sn.properName = foo[2];

            var bar = version.Substring(1).Split('.');
            sn.majorVersion = int.Parse(bar[0]);
            sn.minorVersion = int.Parse(bar[1]);

            return sn;
        }

        public static bool TryParse(string setName, out SetName sn) {
            try {
                sn = Parse(setName);
                return true;
            } catch(Exception ex) {
                sn = null;
                return false;
            }
        }

        public static bool TryParse(string version, string hierarchicalName, out SetName sn) {
            try {
                sn = new SetName();
                HierarchicalName hn;
                if (!HierarchicalName.TryParse(hierarchicalName, out hn))
                    return false;
                sn.nameSpace = hn.Namespace;
                sn.properName = hn.ProperName;

                var bar = version.StartsWith("v") ? version.Substring(1).Split('.') : version.Split('.');
                sn.majorVersion = int.Parse(bar[0]);
                sn.minorVersion = int.Parse(bar[1]);
                return true;
            } catch (Exception ex) {
                sn = null;
                return false;
            }
        }

        public SetName(string nameSpace, string properName) {
            majorVersion = 0;
            minorVersion = 0;
            this.nameSpace = nameSpace;
            this.properName = properName;
        }

        public SetName(string nameSpace, string properName, int majorVersion) {
            this.majorVersion = majorVersion;
            minorVersion = 0;
            this.nameSpace = nameSpace;
            this.properName = properName;
        }

        public SetName(string nameSpace, string name, int majorVersion, int minorVersion) {
            this.majorVersion = majorVersion;
            this.minorVersion = minorVersion;
            this.nameSpace = nameSpace;
            this.properName = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="name"></param>
        /// <param name="version">Must be in form v[major].[minor]</param>
        public SetName(string nameSpace, string name, string version) {
            this.nameSpace = nameSpace;
            this.properName = name;

            try {
                var bar = version.Substring(1).Split('.');
                this.majorVersion = int.Parse(bar[0]);
                this.minorVersion = int.Parse(bar[1]);
            } catch (Exception ex) {
                this.majorVersion = int.MinValue;
                this.minorVersion = int.MinValue;
            }
        }

        public int MajorVersion {
            get { return majorVersion; }
        }

        public int MinorVersion {
            get { return minorVersion; }
        }

        public string Namespace {
            get { return nameSpace; }
        }

        public string ProperName {
            get { return properName; }
        }

        private StringBuilder sb = new StringBuilder();
        public override string ToString() {
            sb.Clear();
            sb.Append('v');
            sb.Append(majorVersion);
            sb.Append('.');
            sb.Append(minorVersion);
            sb.Append(':');
            sb.Append(nameSpace);
            sb.Append(':');
            sb.Append(properName);
            return sb.ToString();
        }

        public string FileName {
            get {
                sb.Clear();
                sb.Append('v');
                sb.Append(majorVersion);
                sb.Append('.');
                sb.Append(minorVersion);
                sb.Append(' ');
                sb.Append(nameSpace);
                sb.Append(' ');
                sb.Append(properName);
                sb.Append(".json");
                return sb.ToString();

            }
        }

        public string ToMostRecentString() {
            sb.Clear();
            sb.Append('v');
            sb.Append(0);
            sb.Append('.');
            sb.Append(0);
            sb.Append(':');
            sb.Append(nameSpace);
            sb.Append(':');
            sb.Append(properName);
            return sb.ToString();
        }

        public string ToMostRecentMajorVersionString() {
            sb.Clear();
            sb.Append('v');
            sb.Append(majorVersion);
            sb.Append('.');
            sb.Append(0);
            sb.Append(':');
            sb.Append(nameSpace);
            sb.Append(':');
            sb.Append(properName);
            return sb.ToString();
        }

        public string ToHierarchicalNameString() {
            sb.Clear();
            sb.Append(nameSpace);
            sb.Append(':');
            sb.Append(properName);
            return sb.ToString();
        }
    }
}
