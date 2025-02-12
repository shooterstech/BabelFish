using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Helpers.Extensions;
using NLog;
using Score = Scopos.BabelFish.DataModel.Athena.Score;
using Scopos.BabelFish.Helpers;
using System.Security.Cryptography;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {
    public abstract class ResultListIntermediateFormattedRow {

        /// <summary>
        /// List of feild values that are always included (the user doesn't have to define these in their definition).
        /// these values are pulled from the .Participant dictionary within the Result COF.
        /// </summary>
        public static readonly IList<string> StandardParticipantAttributeFields = new ReadOnlyCollection<string>( new List<string> { 
            "Rank", 
            "RankOrder",
            "Empty",
            "DisplayName", 
            "DisplayNameShort", 
            "DisplayNameAbbreviated",
            "FamilyName",
            "GivenName",
            "MiddleName",
            "HomeTown",
            "Country", 
            "Club",
            "CompetitorNumber", 
            "MatchLocation", 
            "MatchID", //If this is a Virtual Match, will differ from the ParentID
			"LocalDate", 
            "ResultCOFID", 
            "UserID",
            "Creator",
            "Owner",
            "TargetCollectionName",
            "Status",
            "LastShot" //Only avaliable on individual result lists
        } );

        public static readonly Dictionary<string, string> AliasEventNames = new Dictionary<string, string>() {
            {"Qualifcation", "Individual" },
            {"Individual", "Qualification" },
            {"Team", "Qualification" },
        };

        protected readonly Dictionary<string, string> fields = new Dictionary<string, string>();
        protected readonly ResultEvent resultEvent;
        protected readonly ResultListIntermediateFormatted resultListFormatted; //This row's parent container
        protected Logger logger = LogManager.GetCurrentClassLogger();

        public ResultListIntermediateFormattedRow( ResultListIntermediateFormatted rlf, ResultEvent re ) {

            resultListFormatted = rlf;
            resultEvent = re;

            //Pull in the standard ParticipantAttribute Fields that are always included
            foreach (var field in StandardParticipantAttributeFields) {
                fields[field] = GetParticipantAttribute( field );
            }

            foreach (var field in resultListFormatted.ResultListFormat.Fields) {
                var fieldName = (string)field.FieldName;
                var source = field.Source;

                //Ignore if we already have a field with the same name
                if (fields.ContainsKey( fieldName ))
                    continue;

                switch (field.Method) {
                    case ResultFieldMethod.PARTICIPANT_ATTRIBUTE :
                        fields[fieldName] = GetTruncatedValue( source, GetParticipantAttribute( source.Name.ToString() ) );
                        break;

                    case ResultFieldMethod.ATTRIBUTE:
                        fields[fieldName] = GetTruncatedValue( source, GetAttributeValue( source ) );
                        break;

                    case ResultFieldMethod.SCORE:
                        fields[fieldName] = GetScore( source, false );
                        break;

                    case ResultFieldMethod.PROJECTED_SCORE:
                        fields[fieldName] = GetScore( source, true );
                        break;

                    case ResultFieldMethod.GAP:
                        fields[fieldName] = GetGap( source );
                        break;

                    default:
                        //Should never get here
                        break;
                }
            }
        }

        /// <summary>
        /// Method is responsible for implementing the .TrucateAt rule in the FieldSource.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="untruncatedValue"></param>
        /// <returns></returns>
        private string GetTruncatedValue( FieldSource source , string untruncatedValue ) {
            if ( source.TruncateAt > 3 && untruncatedValue.Length > source.TruncateAt)
            {
                return $"{untruncatedValue.Substring( 0, source.TruncateAt - 3 )}...";
            } else {
                return untruncatedValue;
            }
        }

        /// <summary>
        /// Static version of GetRuncatedValue. Truncates a string at 24 characters.
        /// </summary>
        /// <param name="untruncatedValue"></param>
        /// <returns></returns>
        private string GetTruncatedValue( string untruncatedValue ) {
            if (untruncatedValue.Length > 23) {
                return $"{untruncatedValue.Substring( 0, 20 )}...";
            } else {
                return untruncatedValue;
            }

        }

        /// <summary>
        /// Returns the specified ParticipantAttribute
        /// </summary>
        /// <param name="source">The name of the ParticipantAttribute to return.</param>
        /// <returns></returns>
        private string GetParticipantAttribute( string source ) {
			/*
			//Fields that are unique to the Participant 
            "Rank", 
            "RankOrder",
            "Empty",
            "DisplayName", 
            "DisplayNameShort", 
            "DisplayNameAbbreviated",
            "FamilyName",
            "GivenName",
            "MiddleName",
            "HomeTown",
            "Country", 
            "Club",
            "CompetitorNumber",
			"ResultCOFID", 
            "UserID",
            "LocalDate", 
            "Status",
            "LastShot"

            //Fields that are unique to the child Match ID that generated them
            "MatchLocation", 
            "MatchID", 
            "Creator", //"Orion Scoring System version 2.20.5.2"
            "Owner", //"OrionAcct000001"
            "TargetCollectionName"
            */

			ResultListMetadata metadata;

			switch (source) {
				case "Rank":
                    int rank = GetRank();

                    if (rank > 0)
                        return rank.ToString();

					return "";

                case "RankOrder":
                    int rankOrder = GetRankOrder();

                    if (rankOrder > 0)
                        return rankOrder.ToString();

                    return "";

                case "Empty":
                    return "";

                case "DisplayName":
                    var dn = resultEvent.Participant.DisplayName;
                    if (!string.IsNullOrEmpty( dn ))
                        return dn;

                    return "Unknown";

                case "DisplayNameAbbreviated":
                    //First try the regular display name
                    var dna = resultEvent.Participant.DisplayName;
                    if (!string.IsNullOrEmpty( dna ) && dna.Length <= 20)
                        return dna;

                    //if that's too long, try the display name short, if it exists
                    dna = resultEvent.Participant.DisplayNameShort;
                    if (!string.IsNullOrEmpty( dna ) && dna.Length <= 20)
                        return dna;
                    
                    //If that's too long, go back to the regular display name and truncate it
                    dna = resultEvent.Participant.DisplayName;
                    if (!string.IsNullOrEmpty( dna ))
                        return GetTruncatedValue( dna );

                    return "Unknown";

                case "DisplayNameShort":
                    var dns = resultEvent.Participant.DisplayNameShort;
                    if (!string.IsNullOrEmpty( dns ))
                        return dns;

                    return "Unknown";

                case "FamilyName":
                    if (resultEvent.Participant is Individual)
                        return ((Individual)resultEvent.Participant).FamilyName;
                    else
                        return "";

				case "GivenName":
					if (resultEvent.Participant is Individual)
						return ((Individual)resultEvent.Participant).GivenName;
					else
						return "";

				case "MiddleName":
					if (resultEvent.Participant is Individual)
						return ((Individual)resultEvent.Participant).MiddleName;
					else
						return "";

                case "HomeTown":
                    return resultEvent.Participant.HomeTown;

                case "Country":
                    return resultEvent.Participant.Country;

                case "Club":
                    return resultEvent.Participant.Club;

				case "CompetitorNumber":
					if (resultEvent.Participant is Individual)
						return ((Individual)resultEvent.Participant).CompetitorNumber;
					else
						return "";

				case "ResultCOFID":
                    return resultEvent.ResultCOFID;

                case "UserID":
					if (resultEvent.Participant is Individual)
						return ((Individual)resultEvent.Participant).UserID;
					else
						return "";

                case "LocalDate":
                    return resultEvent.LocalDate.ToString();

				case "MatchID":
					//This is the local match id, which likely different from the Parent ID in a virtual match
					return resultEvent.MatchID;

				case "MatchLocation":
                    if (TryGetResultListMetadata( resultEvent.MatchID, out metadata ) )
                        return metadata.MatchLocation;
                    else 
                        return "";

				case "Creator":
					if (TryGetResultListMetadata( resultEvent.MatchID, out metadata ))
						return metadata.Creator;
					else
						return "";

				case "Owner":
                case "OwnerId":
                case "OwnerID":
					if (TryGetResultListMetadata( resultEvent.MatchID, out metadata ))
						return metadata.OwnerId;
					else
						return "";

				case "TargetCollectionName":
					if (TryGetResultListMetadata( resultEvent.MatchID, out metadata ))
						return metadata.TargetCollectionName;
					else
						return "";

                case "Status":
                    return GetStatus().Description();

                case "LastShot": 
                    var lastShot = resultEvent.LastShot;
                    if (lastShot != null && ( DateTime.UtcNow - lastShot.TimeScored.ToUniversalTime()).TotalSeconds < 300) {

                        string scoreFormat = resultListFormatted.GetScoreFormat( "Shots" );

                        return StringFormatting.FormatScore( scoreFormat, lastShot.Score );
                    }
                    return "";

                default:
                    return "UNKNOWN";
            }
        }

        private bool TryGetResultListMetadata( string matchID, out ResultListMetadata metadata ) {
		    metadata = null;

			if (resultListFormatted.ResultList.Metadata == null
                || string.IsNullOrEmpty( matchID) )
                return false;

            return resultListFormatted.ResultList.Metadata.TryGetValue(matchID, out metadata);

        }

		/// <summary>
		/// Gets/Sets boolean indicating if this row is considered a child row
		/// A child row is, for example, a team member row under a team result row.
		/// </summary>
		public bool IsChildRow { get; protected set; } = false;

        private string GetAttributeValue( FieldSource source ) {
            /*
             * Source is a dictionary. For an Attribute Value will look like
             * {
             *   "Name": "v1.0:ntparc:Three-Position Air Rifle Type"
             * }
             * Currently only supported with a Simple Attribute. As such, our code is
             * written to expect only Attributer Values with only one field.
             */

            foreach ( var av in resultEvent.Participant.AttributeValues ) { 
                if (av.AttributeDef == source.Name) {
                    try {
                        return av.AttributeValue.GetFieldValue( );
                    } catch ( Exception ex ) {
                        logger.Error( ex, "Likely casued by the user specifying an Attribute that is not a Simple Attribute." );
                        return "";
                    }
                }
            }

            //Couldn't find the attribute the user was asking for.
            return "";

        }

        private string GetScore( FieldSource source, bool tryAndUseProjected = false ) {
            /*
             * source is a dictionary. For an Score or ProjectedScore should be
                "Source": {
                    "ScoreFormat": "Events",
                    "Name": "Qualification"
                }
             */

            Score score = GetScore( (string)source.Name, tryAndUseProjected );
            string scoreFormat = resultListFormatted.GetScoreFormat( source.ScoreFormat );

            return Scopos.BabelFish.Helpers.StringFormatting.FormatScore( scoreFormat, score );
        }

        private Score GetScore( string eventName, bool tryAndUseProjected = false ) {

            EventScore scoreToReturn;

            if (resultEvent.EventScores != null) {
                if (resultEvent.EventScores.TryGetValue( eventName, out scoreToReturn )) {

                    if (tryAndUseProjected 
                        && scoreToReturn.Projected != null
                        && ( scoreToReturn.Status == ResultStatus.FUTURE || scoreToReturn.Status == ResultStatus.INTERMEDIATE) ) {
                        //If the Projected Score is known, try and return it
                        return scoreToReturn.Projected;
                    } else {
                        //Else return the regular,, good old fashion, .Score instance
                        return scoreToReturn.Score;
                    }
                }

                /*
                 * EKA Note June 2024: As we migrate more and more orion events over to the Reconfigurable Rulebook COFs
                 * there is a mix of old and new event names. The idea behind using the .aliasEventNames is to address this
                 * changing of event names. Hopefully, this chunk of code can be removed soon.
                 */
                var aliasEventName = "";
                if (AliasEventNames.TryGetValue( eventName , out aliasEventName )) {
                    if (resultEvent.EventScores.TryGetValue( aliasEventName, out scoreToReturn )) {
                        if (tryAndUseProjected 
                            && scoreToReturn.Projected != null
                            && (scoreToReturn.Status == ResultStatus.FUTURE || scoreToReturn.Status == ResultStatus.INTERMEDIATE)) {
                            //If the Projected Score is known, try and return it
                            return scoreToReturn.Projected;
                        } else {
                            //Else return the regular,, good old fashion, .Score instance
                            return scoreToReturn.Score;
                        }
                    }
                }
            }

            //For now return an empty Score data object
            return new Score();
        }

        private string GetGap( FieldSource source ) {
            if (IsChildRow) {
                return "";
            }

            int myRank = GetRankOrder();

            //Determine the rank of the participant to compare against
            int otherRank = source.Value;
            if (otherRank < 0)
                otherRank = myRank - 1;

            //If the other participant is lower ranked than this participant, then there is no gap and we return an empty string
            if (otherRank > myRank)
                return "";

            //Retreive the other Result Event (Row). Test to make sure it exists.
            var otherRow = this.resultListFormatted.GetRowAtRankOrder( otherRank );
            if (otherRow == null)
                return "";

            //Calculate the difference between this score and the score to compare against
            Score myScore = GetScore( source.Name, true );
            Score otherScore = otherRow.GetScore( source.Name, true );

            if (myScore.IsZero)
                return "";

            Score scoreDifference = new() {
                I = otherScore.I - myScore.I,
                D = otherScore.D - myScore.D,
                X = otherScore.X - myScore.X,
                S = otherScore.S - myScore.S,
                NumShotsFired = otherScore.NumShotsFired - myScore.NumShotsFired
                //Choosing not to include J, K, L, as they are special purpose and hard to know what to do with them
            };

            //Format the score
            string scoreFormat = resultListFormatted.GetScoreFormat( source.ScoreFormat );
            return StringFormatting.FormatScore( scoreFormat, scoreDifference );
        }

        /// <summary>
        /// Returns the value of the Rank field for this Result Event. If the Result List
        /// is in the INTERMEDIATE status, then the Projected Rank is returned.
        /// </summary>
        /// <returns></returns>
        public int GetRank() {
            if (this.resultListFormatted.ResultList.Projected
                && this.resultListFormatted.ResultList.Status == ResultStatus.INTERMEDIATE
                && resultEvent.ProjectedRank > 0)
                return resultEvent.ProjectedRank;

            if (resultEvent.Rank > 0)
                return resultEvent.Rank;

            return 0;
        }


        /// <summary>
        /// Returns the value of the Rank sort order field for this Result Event. If the Result List
        /// is in the INTERMEDIATE status, then the Projected Rank sort order is returned.
        /// </summary>
        /// <returns></returns>
        public int GetRankOrder() {
            if (this.resultListFormatted.ResultList.Projected
                && this.resultListFormatted.ResultList.Status == ResultStatus.INTERMEDIATE) {
                if (resultEvent.ProjectedRankOrder > 0)
                    return resultEvent.ProjectedRankOrder;
                if (resultEvent.ProjectedRank > 0)
                    return resultEvent.ProjectedRank;
            }

            if (resultEvent.RankOrder > 0)
                return resultEvent.RankOrder;

            return 0;
        }

        public ResultStatus GetStatus() {

            EventScore topLevelScore;
            string topLevelEventName = this.resultListFormatted.ResultList.EventName;

            if (resultEvent.EventScores != null) {
                if (resultEvent.EventScores.TryGetValue( topLevelEventName, out topLevelScore )) {

                    return topLevelScore.Status;
                }
            }

            ResultListMetadata metadata;
            if (TryGetResultListMetadata( resultEvent.MatchID, out metadata ))
                return metadata.Status;

            //Shouldn't ever get here
            return ResultStatus.OFFICIAL;
        }

        /// <summary>
        /// Returns the field value for the passed in field name.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Thrown if fieldName is not known.</exception>"
        public string GetFieldValue( string fieldName ) {
            return fields[fieldName];
        }

        /// <summary>
        /// Returns true if the passed in fieldName is known, value is the field value.
        /// If false is returned, the fieldName is not known, value will be an empty string. 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetFieldValue( string fieldName, out string value ) {
            try {
                value = fields[fieldName];
                return true;
            } catch (Exception ex) {
                logger.Error( ex );
                value = "";
                return false;
            }
        }

        /// <summary>
        /// Returns the displayed text value for the cell, and the list of CSS classes to mark the cell with.
        /// </summary>
        /// <param name="index">0 based index value for the column to return.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the index value is outside the range of columns.</exception>"
        public CellValues GetColumnBodyCell( int index ) {

            var column = resultListFormatted.ResultListFormat.Format.Columns[index];

            string source = column.Body;
            if (this.IsChildRow && ! string.IsNullOrEmpty( column.Child ))
                source = column.Child;
            string value = source.Replace( fields );

            var classes = new List<string>();
            foreach (var c in column.ClassList)
                classes.Add( (string)c );

            //NOTE .BodyClassList is deprecated
            foreach (var c in column.BodyClassList)
                classes.Add( (string)c );

            var cellValues = new CellValues( this.resultListFormatted, value, classes );
            cellValues.Body = column.Body;
            cellValues.Child = column.Child;

            //Check if the Column definition requires us to link to another page.
            if (this.resultListFormatted.Engagable) {
                switch (column.BodyLinkTo.ToString()) {
                    case "ResultCOF":
                        cellValues.LinkTo = LinkToOption.ResultCOF;
                        cellValues.LinkToData = fields["ResultCOFID"];
                        break;

                    case "PublicProfile":
                        var userId = fields["UserID"];

                        if (!string.IsNullOrEmpty( userId ) && resultListFormatted.UserProfileLookup.HasPublicProfile( userId )) {
                            //User had a public profile
                            cellValues.LinkTo = LinkToOption.PublicProfile;
                            cellValues.LinkToData = resultListFormatted.UserProfileLookup.AccountURLLookUp( userId );
                        } else {
                            //This is the case the user does not have a profile, or the profile is marked private
                            cellValues.LinkTo = LinkToOption.None;
                            cellValues.LinkToData = "";
                        }
                        break;

                    default:
                        cellValues.LinkTo = LinkToOption.None;
                        cellValues.LinkToData = "";
                        break;
                }
            } else {
                //If the RLF is not engagable, don't include the links
                cellValues.LinkTo = LinkToOption.None;
                cellValues.LinkToData = "";
            }

            return cellValues;

        }

        /// <summary>
        /// Returns the displayed text value for the cell, and the list of CSS classes to mark the cell with.
        /// </summary>
        /// <param name="index">0 based index value for the column to return.</param>
        /// <returns></returns>
        public bool TryGetColumnBodyValue( int index, out CellValues returnValue ) {
            try {
                returnValue = GetColumnBodyCell( index );
                return true;
            } catch (Exception ex) {
                returnValue = new CellValues( this.resultListFormatted );
                return false;
            }
        }

        /// <summary>
        /// Returns a list of CSS classes that should be applied to the row.
        /// These values are defined in the RESULT LIST FORMAT definition. 
        /// </summary>
        /// <remarks>
        /// Common values include:
        /// <list type="bullet">
        /// <item>rlf-row-header</item>
        /// <item>rlf-row-athlete</item>
        /// <item>rlf-row-team</item>
        /// <item>rlf-row-child</item>
        /// <item>rlf-row-footer</item>
        /// </list>
        /// </remarks>
        /// <returns></returns>
        public abstract List<string> GetClassList();
    }
}
