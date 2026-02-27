using System.Collections.Concurrent;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A SetName is a unique identifier for a Definition file within a definition type.
    /// It has three parts: the version number, namespace, and proper name.
    /// <para>The most common way to construct a SetName is to use the Parse() method.</para>
    /// <example>
    /// <code>
    /// var setName = Parse("v1.0:orion:Default", false);
    /// </code>
    /// </example>
    /// </summary>
    [G_STJ_SER.JsonConverter( typeof( G_BF_STJ_CONV.SetNameConverter ) )]
    [G_NS.JsonConverter( typeof( G_BF_NS_CONV.SetNameConverter ) )]
    public class SetName : IEquatable<SetName>, IEquatable<HierarchicalName> {


        private int _majorVersion = 0;
        private int _minorVersion = 0;
        private string _nameSpace = "";
        private string _properName = "";

        private static Logger _logger = LogManager.GetCurrentClassLogger();
        private static ConcurrentDictionary<string, SetName> _cache = new ConcurrentDictionary<string, SetName>();
        public static readonly SetName DEFAULT = SetName.Parse( "v1.0:orion:Default" );

        /// <summary>
        /// Default constructor. Should only be used in conjunction with a TryParse method.
        /// <para>The most common way to construct a SetName is to use the SetName.Parse() static method.</para>
        /// <example>
        /// <code>
        /// var setName = SetName.Parse( "v1.0:orion:Default", false);
        /// </code>
        /// </example>
        /// </summary>
        public SetName() { }

        /// <summary>
        /// Helper method to parse a Version string, e.g. "v1.0"
        /// </summary>
        /// <param name="version"></param>
        /// <param name="majorVersion"></param>
        /// <param name="minorVersion"></param>
        /// <returns></returns>
        public static bool TryParseVersion( string version, out int majorVersion, out int minorVersion ) {

            try {
                var bar = version.StartsWith( "v" ) ? version.Substring( 1 ).Split( '.' ) : version.Split( '.' );
                majorVersion = int.Parse( bar[0] );
                minorVersion = int.Parse( bar[1] );
                return true;
            } catch (Exception ex) {
                var msg = $"Unable to parse the version string '{version}'";
                _logger.Error( msg, ex );
                majorVersion = 1;
                minorVersion = 1;
                return false;
            }
        }

        /// <summary>
        /// Parses the passed in setName string, and returns a SetName object.
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="throwExceptionOnError">Determines what do if matchId could not be parsed. If true, throw an ArgumentException, if false, return the default SetName (v1.0:orion:default).</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if the passed in version string could not be parsed.</exception>
        public static SetName Parse( string setName, bool throwExceptionOnError = false ) {

            //Look up in cache first
            SetName sn;
            if (_cache.TryGetValue( setName, out sn ))
                return sn;

            try {
                sn = new SetName();
                var foo = setName.Split( ':' );
                string version = foo[0];
                sn._nameSpace = foo[1];
                sn._properName = foo[2];

                var bar = version.Substring( 1 ).Split( '.' );
                sn._majorVersion = int.Parse( bar[0] );
                sn._minorVersion = int.Parse( bar[1] );

                _cache.TryAdd( setName, sn );

                return sn;
            } catch (Exception ex) {
                var msg = $"Unable to parse the set name string '{setName}'";
                _logger.Error( msg, ex );

                if (throwExceptionOnError)
                    throw new ArgumentException( msg, ex );

                return DEFAULT;
            }
        }

        /// <summary>
        /// Attempts to pase the passed in string into a SetName. Returns true or false if it was successful.
        /// </summary>
        /// <param name="setName"></param>
        /// <param name="sn"></param>
        /// <returns></returns>
        public static bool TryParse( string setName, out SetName sn ) {

            sn = Parse( setName );
            if (sn is null) {
                var msg = $"Unable to parse the set name string '{setName}'";
                _logger.Error( msg );
                return false;
            }
            return true;
        }

        public static bool TryParse( string version, string hierarchicalName, out SetName sn ) {
            try {
                sn = new SetName();
                HierarchicalName hn;
                if (!HierarchicalName.TryParse( hierarchicalName, out hn ))
                    return false;
                sn._nameSpace = hn.Namespace;
                sn._properName = hn.ProperName;

                var bar = version.StartsWith( "v" ) ? version.Substring( 1 ).Split( '.' ) : version.Split( '.' );
                sn._majorVersion = int.Parse( bar[0] );
                sn._minorVersion = int.Parse( bar[1] );
                return true;
            } catch (Exception ex) {
                var msg = $"Unable to parse the version string '{version}'";
                _logger.Error( msg, ex );
                sn = null;
                return false;
            }
        }

        /// <summary>
        /// Constructor. Should only be used in conjunction with a TryParse method.
        /// <para>Note, the most common way to construct a SetName is to use the SetName.Parse() static method.</para>
        /// <example>
        /// <code>
        /// var setName = SetName.Parse( "v1.0:orion:Default", false);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="properName"></param>
        public SetName( string nameSpace, string properName ) {
            _majorVersion = 0;
            _minorVersion = 0;
            this._nameSpace = nameSpace;
            this._properName = properName;
        }

        /// <summary>
        /// Constructor. Should only be used in conjunction with a TryParse method.
        /// <para>Note, the most common way to construct a SetName is to use the SetName.Parse() static method.</para>
        /// <example>
        /// <code>
        /// var setName = SetName.Parse( "v1.0:orion:Default", false);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="properName"></param>
        /// <param name="majorVersion"></param>
        public SetName( string nameSpace, string properName, int majorVersion ) {
            this._majorVersion = majorVersion;
            _minorVersion = 0;
            this._nameSpace = nameSpace;
            this._properName = properName;
        }

        /// <summary>
        /// Constructor. Should only be used in conjunction with a TryParse method.
        /// <para>Note, the most common way to construct a SetName is to use the SetName.Parse() static method.</para>
        /// <example>
        /// <code>
        /// var setName = SetName.Parse( "v1.0:orion:Default", false);
        /// </code>
        /// </example>
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="name"></param>
        /// <param name="majorVersion"></param>
        /// <param name="minorVersion"></param>
        public SetName( string nameSpace, string name, int majorVersion, int minorVersion ) {
            this._majorVersion = majorVersion;
            this._minorVersion = minorVersion;
            this._nameSpace = nameSpace;
            this._properName = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameSpace"></param>
        /// <param name="name"></param>
        /// <param name="version">Must be in form v[major].[minor]</param>
        /// <exception cref="ArgumentException">Thrown if the passed in version string could not be parsed.</exception>
        public SetName( string nameSpace, string name, string version ) {
            this._nameSpace = nameSpace;
            this._properName = name;

            try {
                var bar = version.Substring( 1 ).Split( '.' );
                this._majorVersion = int.Parse( bar[0] );
                this._minorVersion = int.Parse( bar[1] );
            } catch (Exception ex) {
                var msg = $"Unable to parse the version string '{version}'";
                _logger.Error( msg, ex );
                throw new ArgumentException( msg, ex );
            }
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="copy"></param>
        public SetName( SetName copy ) {
            this._majorVersion = copy._majorVersion;
            this._minorVersion = copy._minorVersion;
            this._nameSpace = copy._nameSpace;
            this._properName = copy._properName;
        }

        /// <summary>
        /// The Major version number of the SetName. The major version is intended to be incremented when there are substancne changes to the reference definition.
        /// For example, when the three-position course of fire changed from prone-standing-kneeling to kneeling-prone-standing.
        /// </summary>
        public int MajorVersion {
            get { return _majorVersion; }
        }

        /// <summary>
        /// The Minor version number of the SetName. The minor version is intended to be incremented when there are small changes or corrections to the reference definition.
        /// </summary>
        public int MinorVersion {
            get { return _minorVersion; }
        }

        /// <summary>
        /// The namespace value of the SetName. The namespace is intended to be a unique identifier for the organization that created the reference definition.
        /// For example, Scopos uses the "orion" namespace.
        /// </summary>
        public string Namespace {
            get { return _nameSpace; }
        }

        /// <summary>
        /// The propername value of the SetName. The proper name is intended to be a unique identifier for the reference definition within the namespace.
        /// </summary>
        public string ProperName {
            get { return _properName; }
        }

        /// <summary>
        /// Returns a boolean indicating if this SetName is the default value. which is "v1.0:orion:Default"
        /// </summary>
        public bool IsDefault {
            get {
                return _nameSpace == "orion" && _properName == "Default";
            }
        }

        /// <inheritdoc/>
        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append( 'v' );
            sb.Append( _majorVersion );
            sb.Append( '.' );
            sb.Append( _minorVersion );
            sb.Append( ':' );
            sb.Append( _nameSpace );
            sb.Append( ':' );
            sb.Append( _properName );
            return sb.ToString();
        }

        public string FileName {
            get {
                StringBuilder sb = new StringBuilder();
                sb.Append( 'v' );
                sb.Append( _majorVersion );
                sb.Append( '.' );
                sb.Append( _minorVersion );
                sb.Append( ' ' );
                sb.Append( _nameSpace );
                sb.Append( ' ' );
                sb.Append( _properName );
                sb.Append( ".json" );
                return sb.ToString();

            }
        }

        public string ToMostRecentString() {
            StringBuilder sb = new StringBuilder();
            sb.Append( 'v' );
            sb.Append( 0 );
            sb.Append( '.' );
            sb.Append( 0 );
            sb.Append( ':' );
            sb.Append( _nameSpace );
            sb.Append( ':' );
            sb.Append( _properName );
            return sb.ToString();
        }

        public string ToMostRecentMajorVersionString() {
            StringBuilder sb = new StringBuilder();
            sb.Append( 'v' );
            sb.Append( _majorVersion );
            sb.Append( '.' );
            sb.Append( 0 );
            sb.Append( ':' );
            sb.Append( _nameSpace );
            sb.Append( ':' );
            sb.Append( _properName );
            return sb.ToString();
        }

        public string ToHierarchicalNameString() {
            StringBuilder sb = new StringBuilder();
            sb.Append( _nameSpace );
            sb.Append( ':' );
            sb.Append( _properName );
            return sb.ToString();
        }

        public bool Equals( SetName other ) {
            return this.ToString().Equals( other.ToString() );
        }

        public override bool Equals( object obj ) {
            if (obj == null || !(obj is SetName))
                return false;
            return base.Equals( (SetName)obj );
        }

        public override int GetHashCode() {
            return ToString().GetHashCode();
        }

        public bool Equals( HierarchicalName other ) {
            return this.ToHierarchicalNameString() == other.ToString();
        }

        public static void ClearCache() {
            _cache.Clear();
        }
    }
}
