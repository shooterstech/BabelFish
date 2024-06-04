using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Shot
{
    /// <summary>
    /// Represents the cartisian coordiantes of a shot.
    /// </summary>
	[Serializable]
	public class Location
    {
        /// <summary>
        /// The X cartesian coordinate, measured in mm
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The Y cartesian coordinate, measured in mm
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// The radius value squred, measured in mm^2
        /// </summary>
        /// <returns></returns>
        public double GetRadiusSquared() {
            return X * X + Y * Y;
        }

        /// <summary>
        /// The radius value, measured in mm. No rounding or truncation is done on this value.
        /// DO NOT display this value to the public, ISSF rules forbid it.
        /// </summary>
        /// <returns></returns>
        public double GetRadius() {
            return Math.Sqrt(GetRadiusSquared());
        }

        /// <summary>
        /// Formats the radial distance of the shot location, according to ISSF guidelines, that may
        /// be displayed to the publiic. The radial distance displayed to two desimal places, 
        /// must, upon look up, match the decimal score according to their look up tables.
        /// </summary>
        /// <returns></returns>
        public string GetRadiusToString() {
            //This is effectively truncating the shot location to 2 decimal places
            return (Math.Truncate( GetRadius() * 100 ) / 100).ToString( "0.00" );
        }

        public string GetXToString() {
            return (X).ToString( "0.00" );
        }

        public string GetYToString() { 
            return (Y).ToString( "0.00" ); 
        }

        /// <summary>
        /// In radians, returns the angle of the shot, as reported by the Unit Circle. All values will be between 0 and 2 PI
        /// </summary>
        /// <returns></returns>
        public double GetAngle() {
            var xAbs = Math.Abs( X );
            var yAbs = Math.Abs( Y );   

            //Edge case of very near center of target
            if (xAbs < .0001 && yAbs < .0001)
                return 0;

            //Edge case of X is very near 0 and Y is positive
            if (xAbs < .0001 && Y > 0)
                return (Math.PI / 2.0d);

            //Edge case of X is very near 0 and Y is negative
            if (xAbs < .0001 && Y < 0)
                return (3d * Math.PI / 2.0d);

            //Edge case of Y is very near 0 and X is positive
            if (yAbs < .0001 && X  > 0)
                return 0;

            //Edge case of X is very near 0 and X is negative
            if (yAbs < .0001 && X < 0)
                return Math.PI;

            if (( X > 0 && Y > 0)
                || (X < 0 && Y > 0))
                return Math.Atan2( Y, X );

            if ((X > 0 && Y < 0)
                || (X < 0 && Y < 0))
                return (Math.Atan2( Y, X ) + 2.0 * Math.PI);

            return 0f;
        }

        /// <summary>
        /// Location formatted as a string. (0.00, 0.00) r 0.00
        /// </summary>
        /// <returns></returns>
		public override string ToString() {
            return $"({GetXToString()}, {GetYToString()})";
        }
	}

}