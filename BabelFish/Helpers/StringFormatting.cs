using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.Definitions;
using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft;
using NLog;

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

        [Obsolete( "Deprecated because the input class variable Score is also deprecated.")]
        public static string FormatScore( ScoreFormatCollection scoreFormatCollection, string scoreConfigName, string scoreFormatName, Scopos.BabelFish.DataModel.OrionMatch.Score score ) {
            
            var lowerI = score.I.ToString(); // integer
            var lowerD = score.D.ToString("F1"); // decimal
            var lowerX = score.X.ToString(); // X count
            var upperX = score.X > 0 ? "*" : ""; //asterisk for inners
            var lowerS = score.S > 0 ? score.S.ToString() : score.D.ToString(); // summed score, if no score, then decimal is here.
            var lowerV = ((int)score.V).ToString(); // Var score, as int
            var upperV = score.V.ToString(); // Var score as single decimal
            
            string format = "{d}"; //default value
            foreach( var scoreConfig in scoreFormatCollection.ScoreConfigs ) {
                if ( scoreConfig.ScoreConfigName == scoreConfigName ) {
                    if (scoreConfig.ScoreFormats.TryGetValue(scoreFormatName, out format)) {
                        break;
                    }
                }
            }

            format = format.Replace( "{i}", lowerI ).Replace("{d}", lowerD).Replace("{x}", lowerX).Replace("{X}", upperX).Replace("{s}", lowerS).Replace("{v}", lowerV).Replace("{V}", upperV);
            //format = format.Replace( "{d}", lowerD );

            return format;
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

			var lowerI = score.I.ToString(); // integer
			var lowerD = score.D.ToString( "F1" ); // decimal
			var lowerX = score.X.ToString(); // X count
			var upperX = score.X > 0 ? "*" : ""; //asterisk for inners
			var lowerS = score.S > 0 ? score.S.ToString() : score.D.ToString(); // summed score, if no score, then decimal is here.

            //This is the "If nothing else matches default value" format
            string format = "{d}";
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
                        if (scoreConfig.ScoreFormats.TryGetValue( scoreFormatName, out format )) {
                            break;
                        }
                    }
                }

                //If the scoreConfig was not found, take the first one in the list
                if (!foundTheScoreConfig && scoreFormatCollection.ScoreConfigs.Count > 0) {
                    scoreFormatCollection.ScoreConfigs[0].ScoreFormats.TryGetValue( scoreFormatName, out format );
                }
            }

            try {
                formattedScore = format.Replace( "{i}", lowerI ).Replace( "{d}", lowerD ).Replace( "{x}", lowerX ).Replace( "{X}", upperX ).Replace( "{s}", lowerS );
            } catch (Exception ex){
                logger.Error( ex, $"Could not format score with '{format}'." );
                formattedScore = "Unknown";
            }

			return formattedScore;
		}

		public static string FormatScore( string format, Scopos.BabelFish.DataModel.Athena.Score score ) {

			var lowerI = score.I.ToString(); // integer
			var lowerD = score.D.ToString( "F1" ); // decimal
			var lowerX = score.X.ToString(); // X count
			var upperX = score.X > 0 ? "*" : ""; //asterisk for inners
			var lowerS = score.S > 0 ? score.S.ToString() : score.D.ToString(); // summed score, if no score, then decimal is here.

			format = format.Replace( "{i}", lowerI ).Replace( "{d}", lowerD ).Replace( "{x}", lowerX ).Replace( "{X}", upperX ).Replace( "{s}", lowerS );
			//format = format.Replace( "{d}", lowerD );

			return format;
		}

		/// <summary>
		/// Returns the input string in Title Case
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string TitleCase( string input ) {
            return textInfo.ToTitleCase( input );
        }
    }
}
