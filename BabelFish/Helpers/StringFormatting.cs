using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Definitions;
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft;
using NLog;
using System.Runtime.CompilerServices;
using Scopos.BabelFish.APIClients;

namespace Scopos.BabelFish.Helpers {
    /// <summary>
    /// Provides helper functions to format certain data into common string formatting
    /// </summary>
    public static class StringFormatting {

        private static TextInfo textInfo = new CultureInfo( "en-US", false ).TextInfo;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Formats a span of dates, a start and end date, to a common formatted string.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static string SpanOfDates( DateTime startDate, DateTime endDate ) {
            if (startDate == endDate)
                return startDate.ToString( "ddd, dd MMM yyyy" );
            else if (startDate.Year == endDate.Year
                && startDate.Month == endDate.Month)
                return $"{startDate.ToString( "dd" )} - {endDate.ToString( "dd MMM yyyy" )}";
            else if (startDate.Year == endDate.Year)
                return $"{startDate.ToString( "dd MMM" )} - {endDate.ToString( "dd MMM yy" )}";
            else
                return $"{startDate.ToString( "MM/dd/yy" )} - {endDate.ToString( "MM/dd/yy" )}";

        }

        /// <summary>
        /// Formats a span of dates, a start and end date, to a common formatted string.
        /// If either input variable can not be parsed as a DateTime, then today's date is used.
        /// </summary>
        /// <param name="startDateStr"></param>
        /// <param name="endDateStr"></param>
        /// <returns></returns>
        public static string SpanOfDates( string startDateStr, string endDateStr ) {
            DateTime startDate, endDate;
            try {
                startDate = DateTime.ParseExact( startDateStr, DateTimeFormats.DATE_FORMAT, CultureInfo.InvariantCulture );
            } catch {
                startDate = DateTime.Today;
            }

            try {
                endDate = DateTime.ParseExact( endDateStr, DateTimeFormats.DATE_FORMAT, CultureInfo.InvariantCulture );
            } catch {
                endDate = DateTime.Today;
            }

            return SpanOfDates(startDate, endDate);
        }

        /// <summary>
        /// Formats the passed in DateTime into a standard method of displaying dates.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string SingleDate( DateTime date ) {
            return date.ToString( "ddd, dd MMM yyyy" );
        }

        /// Formats the passed in DateTime string into a standard method of displaying dates.
        /// If the passed in dateStr can not be parsed, today's date is used.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string SingleDate( string dateStr ) {
            DateTime date;
            try {
                date = DateTime.ParseExact( dateStr, DateTimeFormats.DATE_FORMAT, CultureInfo.InvariantCulture );
            } catch {
                date = DateTime.Today;
            }

            return SingleDate( date );
        }

        public static string Location( string city, string state, string country="" ) {
            if (country == "" || country == "USA") {
                //In the USA
                if (!string.IsNullOrEmpty( city ) && ! string.IsNullOrEmpty( state ))
                    return $"{city}, {state}";
                if (string.IsNullOrEmpty( state ))
                    return city;
                if (string.IsNullOrEmpty( city ))
                    return state;
                return "";
            } else {
                //Outside the USA
                if (!string.IsNullOrEmpty( city ))
                    return $"{city}, {country}";

                if (!string.IsNullOrEmpty( state ))
                    return $"{state}, {country}";
                return country;
            }
        }

        /// <summary>
        /// Formats to a string, the inputed score, based on the ScoreFormatCollection
        /// </summary>
        /// <param name="scoreFormatCollection">The ScoreFormatCollection definition to use to learn how to format the passed in Score.</param>
        /// <param name="scoreConfigName">The name of the ScoreConfig to use. Valid values will be defined in the ScoreFormatCollection. Typical values include 'Integer' or 'Decimal'</param>
        /// <param name="scoreFormatName">The name of the Scoreformat to use. Valid values will be defined in the ScoreFormatCollection. Typical values include 'Events' or 'Shots'</param>
        /// <param name="score"></param>
        /// <returns></returns>
		public static string FormatScore( ScoreFormatCollection scoreFormatCollection, string scoreConfigName, string scoreFormatName, Scopos.BabelFish.DataModel.Athena.Score score ) {

            if (score == null)
                return "null";

            //This is the "If nothing else matches default value" format
            string format = "{d}";
            string tempFormat;
            string formattedScore = "Unknown";

            if (scoreFormatCollection != null) {
                //Check that the passed in scoreFormatName is valid. If it is not, take the first value in the .ScoreFormats list
                if (scoreFormatCollection.ScoreFormats.Count > 0 && !scoreFormatCollection.ScoreFormats.Contains( scoreFormatName )) {
                    logger.Warn( $"User passed in a scoreFormatName '{scoreFormatName}' that is not recongnized by the ScoreFormatCollection. Will use a default value instead '{scoreFormatCollection.ScoreFormats[0]}'." );
                    scoreFormatName = scoreFormatCollection.ScoreFormats[0];
                }

                //This should be the happy path, finding both the scoreConfig and scoreFormat
                bool foundTheScoreConfig = false;
                foreach (var scoreConfig in scoreFormatCollection.ScoreConfigs) {
                    if (scoreConfig.ScoreConfigName == scoreConfigName) {
                        foundTheScoreConfig = true;
                        if (scoreConfig.ScoreFormats.TryGetValue( scoreFormatName, out tempFormat )) {
                            format = tempFormat;
                            break;
                        }
                    }
                }

                //If the scoreConfig was not found, take the first one in the list
                if (!foundTheScoreConfig && scoreFormatCollection.ScoreConfigs.Count > 0) {
                    logger.Warn( $"User passed in a scoreConfigName '{scoreConfigName}' that is not recongnized by the ScoreFormatCollection. Will use a default value instead." );
                    if (scoreFormatCollection.ScoreConfigs[0].ScoreFormats.TryGetValue( scoreFormatName, out tempFormat )) {
                        format = tempFormat;
                    }
                }
            }

            //At this point, format should be a legit score format string. But to be sure ...
            if (string.IsNullOrEmpty( format )) {
                logger.Warn( "'format' is unexpectedly a null value or empty string. " );
                format = "{d}";
            }

            return FormatScore( format, score );
        }
        
        /// <summary>
         /// Formats to a string, the inputed score, based on the ScoreFormatCollection
         /// </summary>
         /// <param name="scoreFormatCollection">The ScoreFormatCollection definition to use to learn how to format the passed in Score.</param>
         /// <param name="scoreConfigName">The name of the ScoreConfig to use. Valid values will be defined in the ScoreFormatCollection. Typical values include 'Integer' or 'Decimal'</param>
         /// <param name="scoreFormatName">The name of the Scoreformat to use. Valid values will be defined in the ScoreFormatCollection. Typical values include 'Events' or 'Shots'</param>
         /// <param name="score"></param>
         /// <returns></returns>
        [Obsolete( "Deprecated because the input class variable Score is also deprecated." )]
        public static string FormatScore( ScoreFormatCollection scoreFormatCollection, string scoreConfigName, string scoreFormatName, Scopos.BabelFish.DataModel.OrionMatch.Score score ) {

            if (score == null)
                return "null";

            var athenaScore = new BabelFish.DataModel.Athena.Score();
            athenaScore.I = score.I;
            athenaScore.D = score.D;
            athenaScore.X = score.X;
            athenaScore.S = score.S;

            return FormatScore( scoreFormatCollection, scoreConfigName, scoreFormatName, athenaScore );
        }

        /// <summary>
        /// Replaces the special characters in input string format, with the score values defined in score.
        /// Replacement characters are defined at https://support.scopos.tech/index.html?string-formatting-score-format.html
        /// </summary>
        /// <param name="format"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static string FormatScore( string format, Scopos.BabelFish.DataModel.Athena.Score score ) {

            if (score == null)
                return "null";

            //The first value in the Tuple is the replacement string, e.g. "{x}"
            //The second value is the string to replace it with, e.g. "98"
            List<Tuple<string, string>> listOfReplacements = new List<Tuple<string, string>>();
            listOfReplacements.Add( new Tuple<string, string>( "{i}", score.I.ToString() )); // integer
            listOfReplacements.Add( new Tuple<string, string>( "{d}", score.D.ToString( "F1" ) ) ); // decimal
            listOfReplacements.Add( new Tuple<string, string>( "{x}", score.X.ToString() ) ); // X count
            listOfReplacements.Add( new Tuple<string, string>( "{X}", score.X > 0 ? "*" : "" ) ); //asterisk for inners
            listOfReplacements.Add( new Tuple<string, string>( "{s}", score.S > 0 ? score.S.ToString() : score.D.ToString() ) ); // summed score, if no score, then decimal is here.
            listOfReplacements.Add( new Tuple<string, string>( "{m}", "" ) ); // Attributes of a shot. Currently not supported
            listOfReplacements.Add( new Tuple<string, string>( "{j}", score.J.ToString( "F1" ) ) ); // Special use case score. 
            listOfReplacements.Add( new Tuple<string, string>( "{k}", score.K.ToString( "F1" ) ) ); // Special use case score. 
            listOfReplacements.Add( new Tuple<string, string>( "{l}", score.L.ToString( "F1" ) ) ); // Special use case score. 
            listOfReplacements.Add( new Tuple<string, string>( "{J}", ((int)score.J ).ToString() ) ); // Special use case score. 
            listOfReplacements.Add( new Tuple<string, string>( "{K}", ((int)score.K ).ToString() ) ); // Special use case score. 
            listOfReplacements.Add( new Tuple<string, string>( "{L}", ((int)score.L ).ToString() ) ); // Special use case score. 

            string formattedScore = format;
            int numOfErrors = 0;

            //Look for conditional operators
            //{?i} if score.I == 0, Process only the right half of the string. If score.I != 0, process the left half
            //{?x} if score.X == 0, Process only the right half of the string. If score.X != 0, process the left half
            //{?d} if score.D == 0, Process only the right half of the string. If score.D != 0, process the left half
            //Will build in others, as necessary
            int conditionalIndex = formattedScore.IndexOf( "{?i}" );
            if (conditionalIndex >= 0) {
                
                if (score.I == 0) {
                    formattedScore = formattedScore.Substring( conditionalIndex + 4 ); //The +4 is to account for the length of "{?i}"
                } else {
                    formattedScore = formattedScore.Substring( 0, conditionalIndex );
                }
            }

            conditionalIndex = formattedScore.IndexOf( "{?x}" );
            if (conditionalIndex >= 0) {

                if (score.X == 0) {
                    formattedScore = formattedScore.Substring( conditionalIndex + 4 ); //The +4 is to account for the length of "{?i}"
                } else {
                    formattedScore = formattedScore.Substring( 0, conditionalIndex );
                }
            }

            conditionalIndex = formattedScore.IndexOf( "{?d}" );
            if (conditionalIndex >= 0) {

                if (score.D == 0) {
                    formattedScore = formattedScore.Substring( conditionalIndex + 4 ); //The +4 is to account for the length of "{?i}"
                } else {
                    formattedScore = formattedScore.Substring( 0, conditionalIndex );
                }
            }

            foreach ( var replacement in listOfReplacements ) {
                try {
                    formattedScore = formattedScore.Replace( replacement.Item1, replacement.Item2 );
                } catch (Exception e ) {
                    logger.Error( e, $"Unable to replace string '{replacement.Item1}' with '{replacement.Item2}' using format '{format}'." );
                    numOfErrors++;

                    //Because exceptions can take a long time to deal with (for a computer at least), track the number of exceptions and give up after the second one.
                    if (numOfErrors > 1)
                        break;
                }
            }

			return formattedScore;
		}


		/// <summary>
		/// Returns the input string in Title Case, and converts ordinals to lower case.
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string TitleCase( string input ) {
            var titleCase = textInfo.ToTitleCase( input );
            return ConvertOrdinalsToLowerCase( titleCase );
        }

        /// <summary>
        /// Converts the ordinal values to lower case.
        /// 1ST => 1st, or 2ND => 2nd
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ConvertOrdinalsToLowerCase( string input ) {
            // Define a regex pattern to match ordinals (e.g., 1st, 2nd, 3rd, 4th)
            var pattern = @"\b(\d+)(st|nd|rd|th)\b";

            // Use regex replace to convert matched ordinals to lowercase
            var output = Regex.Replace( input, pattern, m => m.Value.ToLower(), RegexOptions.IgnoreCase );

            return output;
        }

        /// <summary>
        /// Formats city, state and country is a standard format.
        /// </summary>
        /// <param name="hometown"></param>
        /// <returns></returns>
        public static string Hometown( string city, string state, string country ) {
            if (string.IsNullOrWhiteSpace( country ) || country == "USA" || country == "US") {
                if (!string.IsNullOrWhiteSpace( state )) {
                    if (!string.IsNullOrWhiteSpace( city )) {
                        return $"{city}, {state}";
                    } else {
                        return city;
                    }
                } else {
                    if (!string.IsNullOrWhiteSpace( city )) {
                        return city;
                    } else {
                        return "UNKNOWN";
                    }
                }
            } else {
                if (!string.IsNullOrWhiteSpace( state )) {
                    if (!string.IsNullOrWhiteSpace( city )) {
                        return "{city}, {country}";
                    } else {
                        return "{state}, {country}";
                    }
                } else {
                    if (!string.IsNullOrWhiteSpace( city )) {
                        return "{city}, {country}";
                    } else {
                        return country;
                    }
                }
            }
        }
    }
}
