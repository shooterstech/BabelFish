using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Helpers.Extensions {
    public static class StringComparison {

        /// <summary>
        /// Compares two strings, as if they were integers. 
        /// This is often times needed when a relay value, or a firing point value is represented by a string, but is
        /// human readable as an integer. For example "Relay 01" or "Firing Poing 08".
        /// If a string and a integer (e.g. "A" vs "01") is compared, the integer has the higher value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int CompareToAsIntegers( this string x, string y ) {

            if (int.TryParse( x, out int Xint )) {
                //X is an integer
                if (int.TryParse( y, out int Yint )) {
                    //both x and y are ints
                    return Xint.CompareTo( Yint );
                } else {
                    //x is int, y is not
                    //Returning that X, an integer, is greater than y, a string.
                    return 1;
                }
            } else {
                //x is not an int
                if (int.TryParse( y, out int Yint )) {
                    //x not int, y is int
                    //Returning that X, a string, is less than Y, an integer.
                    return -1;
                } else {
                    //both x and y are strings
                    return x.CompareTo( y );
                }
            }

        }
    }
}
