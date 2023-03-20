using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Helpers {
    public static class RandomStringGenerator {

        private static Random random = new Random();

        public static string RandomAlphaNumericString( int length ) {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string( Enumerable.Repeat( chars, length )
                .Select( s => s[random.Next( s.Length )] ).ToArray() );
        }

        public static string RandomAlphaString( int length ) {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string( Enumerable.Repeat( chars, length )
                .Select( s => s[random.Next( s.Length )] ).ToArray() );
        }
        public static string RandomNumericString( int length ) {
            const string chars = "0123456789";
            return new string( Enumerable.Repeat( chars, length )
                .Select( s => s[random.Next( s.Length )] ).ToArray() );
        }
    }
}
