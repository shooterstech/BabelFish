using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions {
    public class DefinitionVersion: IComparable<DefinitionVersion>, IEquatable<DefinitionVersion> {

        private NLog.Logger logger;
        private bool buildVersionIncluded = false;

        /// <summary>
        /// Constructor that takes in a version string (e.g. 1.2) and parses
        /// it for the Major, Minor
        /// </summary>
        /// <param name="versionString"></param>
        /// <exception cref="ArgumentException">Thrown if the inputer versionString can not be parsed to format x.y</exception>
        public DefinitionVersion( string versionString ) {
            this.logger = NLog.LogManager.GetCurrentClassLogger();

            string message = string.Empty;
            if ( ! TryParse( versionString, out message ) ) {
                logger.Error( message );
                throw new ArgumentException( message );
            }
        }

        private bool TryParse( string versionString, out string msg ) {
            msg = string.Empty;

            try {
                //Break the string down into it's components and assign it to the appropriate value.
                var components = versionString.Split( new[] { '.' } );

                MajorVersion = int.Parse( components[0] );
                MinorVersion = int.Parse( components[1] );

                if (MajorVersion <= 0) {
                    msg = $"Unable to satisfactory parse version string {versionString} as the Major Version value is 0, which is reserved.";
                    return false;
                } else if (MinorVersion <= 0) {
                    msg = $"Unable to satisfactory parse version string {versionString} as the Minor Version value is 0, which is reserved." ;
                    return false;
                }

                return true;

            } catch (Exception ex) {
                MajorVersion = 0;
                MinorVersion = 0;
                msg = $"Unable to parse {versionString} as a version in form x.y";
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
        public int CompareTo( DefinitionVersion other ) {
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

            //Must be equal if we get here
            return 0;
        }

        /// <summary>
        /// Returns true if this instance is equal to the passed in other DefinitionVersion
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals( DefinitionVersion other ) {

            return this.MajorVersion == other.MajorVersion
                   && this.MinorVersion == other.MinorVersion;
        }

        /// <summary>
        /// Equals operator
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals( object obj ) {
            if (obj == null)
                return false;
            if (!(obj is DefinitionVersion))
                return false;
            return (this.Equals( (DefinitionVersion)obj ));
        }

        #region Operator Overloads
        /// <summary>
        /// Greater than operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >( DefinitionVersion a, DefinitionVersion b ) => a.CompareTo( b ) < 0;

        /// <summary>
        /// Less than operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <( DefinitionVersion a, DefinitionVersion b ) => a.CompareTo( b ) > 0;

        /// <summary>
        /// Greater than or equal operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator >=( DefinitionVersion a, DefinitionVersion b ) => a.CompareTo( b ) <= 0;

        /// <summary>
        /// Less than or equal operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator <=( DefinitionVersion a, DefinitionVersion b ) => a.CompareTo( b ) >= 0;

        /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==( DefinitionVersion a, DefinitionVersion b ) => a.Equals( b );

        /// <summary>
        /// Inequality operator 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=( DefinitionVersion a, DefinitionVersion b ) => !a.Equals( b );
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
        /// Returns a string in the form x.y.z.b
        /// Or x.y.z
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return $"{MajorVersion}.{MinorVersion}";
        }

        /// <summary>
        /// Returns a has code of the ToString() value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Helper method. 
        /// Returns a constant string version 0.0, which is the code for the most recent version overall of a Definition.
        /// </summary>
        /// <returns></returns>
        public string GetAsMostRecentVersion() {
            return "0.0";
        }

        /// <summary>
        /// Helper method. 
        /// Returns a string version value n.0, where n is the MajorVersion value. Which is code for the most recent major version. 
        /// </summary>
        /// <returns></returns>
        public string GetAsMostRecentMajorVersion() {
            return $"{this.MajorVersion}.0";
        }

    }
}
