using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using NLog;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    /// <summary>
    /// Represents a string of an Athena Version number.
    /// Is in the form of x.y.z.b.
    /// x is the Major Version
    /// y is the Minor Version
    /// z is the Patch Version
    /// p is the Build version. Defaults to 1 if not included in the string.
    /// </summary>
    public class AthenaVersion : IComparable<AthenaVersion>, IEquatable<AthenaVersion>
    {

        private NLog.Logger logger;
        private bool buildVersionIncluded = false;

        /// <summary>
        /// Constructor that takes in a version string (e.g. 1.2.3b4) and parses
        /// it for the Major, Minor, Patch, and Build Version
        /// </summary>
        /// <param name="versionString"></param>
        public AthenaVersion(string versionString)
        {
            this.logger = NLog.LogManager.GetCurrentClassLogger();

            TryParse(versionString);
        }

        private bool TryParse(string versionString)
        {

            //versionString might be false, if it got passed in from the RangeControl tab, before the state got updated.
            if (versionString == "UNKNOWN")
            {
                MajorVersion = 0;
                MinorVersion = 0;
                PatchVersion = 0;
                BuildVersion = 0;
                return false;
            }

            try
            {
                //Break the string down into it's components and assign it to the appropriate value.
                var components = versionString.Split(new[] { '.', 'b' });
                Debug.Assert(components.Length == 3 || components.Length == 4, "Expected an Athena Version of either 3 or 4 components.");

                MajorVersion = int.Parse(components[0]);
                MinorVersion = int.Parse(components[1]);
                PatchVersion = int.Parse(components[2]);
                if (components.Length == 4)
                {
                    BuildVersion = int.Parse(components[3]);
                    buildVersionIncluded = true;
                }
                else
                {
                    BuildVersion = 0;
                    buildVersionIncluded = false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MajorVersion = 0;
                MinorVersion = 0;
                PatchVersion = 0;
                BuildVersion = 0;
                logger.Error($"Unable to parse AthenaVersion string {versionString}. Received error {ex.Message}.");
                return false;
            }
        }

        /// <summary>
        /// Returns -1 if this instance precedes other.
        /// Returns 0 if this instance is equal to other.
        /// Returns 1 if this instance follows other, or other is null
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(AthenaVersion other)
        {
            //Compare Major Versions
            if (this.MajorVersion > other.MajorVersion)
                return -1;
            if (this.MajorVersion < other.MajorVersion)
                return 1;

            //Compare Minor Versions
            if (this.MinorVersion > other.MinorVersion)
                return -1;
            if (this.MinorVersion < other.MinorVersion)
                return 1;

            //Compare PatchVersions
            if (this.PatchVersion > other.PatchVersion)
                return -1;
            if (this.PatchVersion < other.PatchVersion)
                return 1;

            //Compare BuildVersion
            if (this.BuildVersion > other.BuildVersion)
                return -1;
            if (this.BuildVersion < other.BuildVersion)
                return 1;

            //Must be equal if we get here
            return 0;
        }

        /// <summary>
        /// Returns true if this instance is equal to the passed in other AthenaVersion
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(AthenaVersion other)
        {
            if (other == null)
                return false;

            return this.MajorVersion == other.MajorVersion
                   && this.MinorVersion == other.MinorVersion
                   && this.PatchVersion == other.PatchVersion
                   && this.BuildVersion == other.BuildVersion;
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is AthenaVersion))
                return false;
            return (this.Equals((AthenaVersion)obj));
        }

        #region Operator Overloads
        /// <summary>
        /// Greater than operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >(AthenaVersion a, AthenaVersion b) => a.CompareTo(b) < 0;

        /// <summary>
        /// Less than operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <(AthenaVersion a, AthenaVersion b) => a.CompareTo(b) > 0;

        /// <summary>
        /// Greater than or equal operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >=(AthenaVersion a, AthenaVersion b) => a.CompareTo(b) <= 0;

        /// <summary>
        /// Less than or equal operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <=(AthenaVersion a, AthenaVersion b) => a.CompareTo(b) >= 0;

        /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(AthenaVersion a, AthenaVersion b) => a.Equals(b);

        /// <summary>
        /// Inequality operator 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(AthenaVersion a, AthenaVersion b) => !a.Equals(b);
        #endregion

        /// <summary>
        /// Major Version number.
        /// Representing significant changes to the code.
        /// </summary>
        public int MajorVersion { get; private set; }

        /// <summary>
        /// Minor Version number.
        /// Representing new features to the code.
        /// </summary>
        public int MinorVersion { get; private set; }

        /// <summary>
        /// Patch Version number.
        /// Representing bug fixes.
        /// </summary>
        public int PatchVersion { get; private set; }

        /// <summary>
        /// Build Version number.
        /// Representing internal builds, not for customer release.
        /// </summary>
        public int BuildVersion { get; private set; }

        /// <summary>
        /// Returns a string in the form x.y.z.b
        /// Or x.y.z
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (buildVersionIncluded)
                return $"{MajorVersion}.{MinorVersion}.{PatchVersion}.{BuildVersion}";

            //else
            return $"{MajorVersion}.{MinorVersion}.{PatchVersion}";
        }

        /// <summary>
        /// Returns a has code of the ToString() value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}