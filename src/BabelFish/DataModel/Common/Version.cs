using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.Common {
    /// <summary>
    /// Represents a string of a Scopos Version number.
    /// <list type="bullet">Is in the form of x.y.z.b.
    /// <item>x is the Major Version</item>
    /// <item>y is the Minor Version</item>
    /// <item>z is the Patch Version</item>
    /// <item>p is the Build version. Defaults to 0 if not included in the string.</item>
    /// </list>
    /// <para>Use <see cref="Parse(string)"/> or <see cref="TryParse(string, out Version)"/> to create instances of this class.</para>
    /// </summary>
    /// <remarks>
    /// NOTE: this is NOT the same version found within a <see cref="SetName"/>.</remarks>
    [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.VersionConverter ) )]
    [G_NS.JsonConverter( typeof( G_BF_NS_CONV.VersionConverter ) )]
    public class Version : IComparable<Version>, IEquatable<Version> {

        public static readonly Version DEFAULT = Version.Parse( "1.1.1.1" );

        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private bool buildVersionIncluded = false;

        /// <summary>
        /// Private constructor. Use <see cref="Parse(string)"/> or <see cref="TryParse(string, out Version)"/> to create instances of this class."/>
        /// </summary>
        private Version() {
        }

        public static bool TryParse( string versionString, out Version version ) {
            version = Version.DEFAULT;

            //versionString might be false, if it got passed in from the RangeControl tab, before the state got updated.
            if (versionString == "UNKNOWN") {
                return false;
            }

            try {
                version = Version.Parse( versionString, true );
                return true;

            } catch (Exception ex) {
                _logger.Error( $"Unable to parse AthenaVersion string {versionString}. Received error {ex.Message}." );
                return false;
            }
        }

        /// <summary>
        /// Parses the passed  and returns a Version object.
        /// <para>
        /// If the passed in versionString can not be parsed, either a ArgumentException is thrown, or the Default Version is returned (1.1.1.1), depending on the value of throwExceptionOnError.</para>
        /// </summary>
        /// <param name="versionString"></param>
        /// <param name="throwExceptionOnError">Determines what do if versionString could not be parsed. If true, throw an ArgumentException, if false, return the default Version (1.1.1.1).</param>
        /// <exception cref="ArgumentException">Thrown when the input versionString is not in the expected format.</exception>
        public static Version Parse( string versionString, bool throwExceptionOnError = false ) {

            //Break the string down into it's components and assign it to the appropriate value.
            var components = versionString.Split( new[] { '.', 'b' } );

            if (components.Length < 3 || components.Length > 4) {
                var msg = $"The version string {versionString} is not supported. Expecting a string in the format of x.y.z or x.y.z.b.";
                _logger.Error( msg );
                if (throwExceptionOnError)
                    throw new ArgumentException( msg );
                else
                    return DEFAULT;
            }

            try {
                Version v = new Version();
                v.MajorVersion = int.Parse( components[0] );
                v.MinorVersion = int.Parse( components[1] );
                v.PatchVersion = int.Parse( components[2] );
                if (components.Length == 4) {
                    v.BuildVersion = int.Parse( components[3] );
                    v.buildVersionIncluded = true;
                } else {
                    v.BuildVersion = 0;
                    v.buildVersionIncluded = false;
                }

                return v;
            } catch (Exception ex) {
                var msg = $"The version string {versionString} is not supported. Expecting a string in the format of x.y.z or x.y.z.b. Received error {ex.Message}.";
                _logger.Error( msg );
                if (throwExceptionOnError)
                    throw new ArgumentException( msg, ex );
                else
                    return DEFAULT;
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
            if (other is null)
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
        public int MajorVersion { get; private set; } = 1;

        /// <summary>
        /// Minor Version number.
        /// Representing new features to the code.
        /// </summary>
        public int MinorVersion { get; private set; } = 1;

        /// <summary>
        /// Patch Version number.
        /// Representing bug fixes.
        /// </summary>
        public int PatchVersion { get; private set; } = 1;

        /// <summary>
        /// Build Version number.
        /// Representing internal builds, not for customer release.
        /// </summary>
        public int BuildVersion { get; private set; } = 1;

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

        /// <summary>
        /// Returns a boolean indicating if this Version is the default Version value of 1.1.1.1.
        /// </summary>
        public bool IsDefault {
            get {
                return this.MajorVersion == 1 && this.MinorVersion == 1 && this.PatchVersion == 1 && this.BuildVersion == 1;
            }
        }

        /// <summary>
        /// Implicitly converts a Version instance to its string representation.
        /// <para>This conversion returns the default string representation of Version if the provided
        /// instance is null.</para>
        /// </summary>
        /// <remarks>It is safe to mark this as an implicit operator since Version has a well known string representation.</remarks>
        /// <param name="sn">The Version instance to convert. If null, the default string representation is returned.</param>
        public static implicit operator string( Version sn ) {
            if (sn is null)
                return Version.DEFAULT.ToString();
            else
                return sn.ToString();

        }
    }
}
