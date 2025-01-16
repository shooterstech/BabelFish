using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A SetName is a unique identifier for a Defintion file within a definition type. It has three parts, the version number, namespace, and propername.
    /// </summary>
    public class SetName: IEquatable<SetName>, IEquatable<HierarchicalName> {

        private int majorVersion = 0;
        private int minorVersion = 0;
        private string nameSpace = "";
        private string properName = "";

        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Default constructor. Should only be used in conjunction with a TryParse method.
        /// </summary>
        public SetName() { }

        public static bool TryParseVersion(string version, out int majorVersion, out int minorVersion) {

            try {
                var bar = version.StartsWith("v") ? version.Substring(1).Split('.') : version.Split('.');
                majorVersion = int.Parse(bar[0]);
                minorVersion = int.Parse(bar[1]);
                return true;
            } catch(Exception ex) {
                var msg = $"Unable to parse the version string '{version}'";
                logger.Error(msg, ex);
                majorVersion = 1;
                minorVersion = 1;
                return false;
            }
        }

        /// <summary>
        /// Parses the passed in setName string, and returns a SetName object.
        /// </summary>
        /// <param name="setName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if the passed in version string could not be parsed.</exception>
        public static SetName Parse(string setName) {

            try {
                SetName sn = new SetName();
                var foo = setName.Split( ':' );
                string version = foo[0];
                sn.nameSpace = foo[1];
                sn.properName = foo[2];

                var bar = version.Substring( 1 ).Split( '.' );
                sn.majorVersion = int.Parse( bar[0] );
                sn.minorVersion = int.Parse( bar[1] );

                return sn;
            } catch (Exception ex) {
                var msg = $"Unable to parse the set name string '{setName}'";
                logger.Error( msg, ex );
                throw new ArgumentException( msg, ex );
            }
        }

        public static bool TryParse(string setName, out SetName sn) {
            try {
                sn = Parse(setName);
                return true;
            } catch(Exception ex) {
                var msg = $"Unable to parse the set name string '{setName}'";
                logger.Error( msg, ex );
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
                var msg = $"Unable to parse the version string '{version}'";
                logger.Error( msg, ex );
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
        /// <exception cref="ArgumentException">Thrown if the passed in version string could not be parsed.</exception>
        public SetName(string nameSpace, string name, string version) {
            this.nameSpace = nameSpace;
            this.properName = name;

            try {
                var bar = version.Substring(1).Split('.');
                this.majorVersion = int.Parse(bar[0]);
                this.minorVersion = int.Parse(bar[1]);
            } catch (Exception ex) {
                var msg = $"Unable to parse the version string '{version}'";
                logger.Error( msg, ex );
                throw new ArgumentException( msg, ex );
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

        /// <inheritdoc/>
        public override string ToString() {
            StringBuilder sb = new StringBuilder();
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
                StringBuilder sb = new StringBuilder();
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
            StringBuilder sb = new StringBuilder();
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
            StringBuilder sb = new StringBuilder();
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
            StringBuilder sb = new StringBuilder();
            sb.Append(nameSpace);
            sb.Append(':');
            sb.Append(properName);
            return sb.ToString();
        }

        public bool Equals( SetName other ) {
            return this.ToString().Equals( other.ToString() );
        }

        public override bool Equals( object obj ) {
            if ( obj == null || ! (obj is SetName)) 
                return false;
            return base.Equals( (SetName) obj );
        }

        public override int GetHashCode() {
            return ToString().GetHashCode();
        }

        public bool Equals( HierarchicalName other ) {
            return this.ToHierarchicalNameString() == other.ToString();
        }
    }
}
