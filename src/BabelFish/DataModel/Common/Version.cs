using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Scopos.BabelFish.DataModel.Common {
    /// <summary>
    /// Represents a string of a Scopos Version number.
    /// Is in the form of x.y.z.b.
    /// x is the Major Version
    /// y is the Minor Version
    /// z is the Patch Version
    /// p is the Build version. Defaults to 0 if not included in the string.
    /// </summary>
    public class Version : IComparable<Version>, IEquatable<Version> {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private bool buildVersionIncluded = false;

        public Version() { }

        /// <summary>
        /// Constructor that takes in a version string (e.g. 1.2.3b4) and parses
        /// it for the Major, Minor, Patch, and Build Version
        /// </summary>
        /// <param name="versionString"></param>
        /// <exception cref="ArgumentException">Thrown when the input versionString is not in the expected format.</exception>
        public Version( string versionString ) {
            this.Parse( versionString );
        }

        public static bool TryParse( string versionString, out Version version ) {
            version = new Version();

            //versionString might be false, if it got passed in from the RangeControl tab, before the state got updated.
            if (versionString == "UNKNOWN") {
                version.MajorVersion = 0;
                version.MinorVersion = 0;
                version.PatchVersion = 0;
                version.BuildVersion = 0;
                return false;
            }

            try {
                version.Parse( versionString );
                return true;

            } catch (Exception ex) {
                version.MajorVersion = 0;
                version.MinorVersion = 0;
                version.PatchVersion = 0;
                version.BuildVersion = 0;
                logger.Error( $"Unable to parse AthenaVersion string {versionString}. Received error {ex.Message}." );
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionString"></param>
        /// <exception cref="ArgumentException">Thrown when the input versionString is not in the expected format.</exception>
        private void Parse(string versionString) {

            //Break the string down into it's components and assign it to the appropriate value.
            var components = versionString.Split( new[] { '.', 'b' } );

            if (components.Length < 3 || components.Length > 4) {
                var msg = $"The version string {versionString} is not supported. Expecting a string in the format of x.y.z or x.y.z.b.";
                logger.Error( msg );
                throw new ArgumentException( msg );
            }

            MajorVersion = int.Parse( components[0] );
            MinorVersion = int.Parse( components[1] );
            PatchVersion = int.Parse( components[2] );
            if (components.Length == 4) {
                BuildVersion = int.Parse( components[3] );
                buildVersionIncluded = true;
            } else {
                BuildVersion = 0;
                buildVersionIncluded = false;
            }
        }

        /// <summary>
        /// Returns -1 if this instance precedes other.
        /// Returns 0 if this instance is equal to other.
        /// Returns 1 if this instance follows other, or other is null
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo( Version other ) {
            if ( other is null )
                return 1;

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
        public bool Equals( Version other ) {
            if (other is null)
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
        public override bool Equals( object obj ) {
            if (obj is null)
                return false;
            if (!(obj is Version))
                return false;
            return (this.Equals( (Version)obj ));
        }

        #region Operator Overloads
        /// <summary>
        /// Greater than operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >( Version a, Version b ) => a.CompareTo( b ) < 0;

        /// <summary>
        /// Less than operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <( Version a, Version b ) => a.CompareTo( b ) > 0;

        /// <summary>
        /// Greater than or equal operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >=( Version a, Version b ) => a.CompareTo( b ) <= 0;

        /// <summary>
        /// Less than or equal operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <=( Version a, Version b ) => a.CompareTo( b ) >= 0;

        /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==( Version a, Version b ) => a.Equals( b );

        /// <summary>
        /// Inequality operator 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=( Version a, Version b ) => !a.Equals( b );
        #endregion

        /// <summary>
        /// Major Version number.
        /// Representing significant changes to the code.
        /// </summary>
        public int MajorVersion { get; private set; } = 0;

        /// <summary>
        /// Minor Version number.
        /// Representing new features to the code.
        /// </summary>
        public int MinorVersion { get; private set; } = 0;

        /// <summary>
        /// Patch Version number.
        /// Representing bug fixes.
        /// </summary>
        public int PatchVersion { get; private set; } = 0;

        /// <summary>
        /// Build Version number.
        /// Representing internal builds, not for customer release.
        /// </summary>
        public int BuildVersion { get; private set; } = 0;

        /// <summary>
        /// Returns a string in the form x.y.z.b
        /// Or x.y.z
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            if (buildVersionIncluded)
                return $"{MajorVersion}.{MinorVersion}.{PatchVersion}.{BuildVersion}";

            //else
            return $"{MajorVersion}.{MinorVersion}.{PatchVersion}";
        }

        /// <summary>
        /// Returns a has code of the ToString() value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return ToString().GetHashCode();
        }
    }
}
