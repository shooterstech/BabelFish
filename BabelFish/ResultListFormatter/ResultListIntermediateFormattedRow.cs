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

namespace Scopos.BabelFish.ResultListFormatter {
    public abstract class ResultListIntermediateFormattedRow {

        /// <summary>
        /// List of feild values that are always included (the user doesn't have to define these in their definition).
        /// these values are pulled from the .Participant dictionary within the Result COF.
        /// </summary>
        public static readonly IList<string> StandardParticipantAttributeFields = new ReadOnlyCollection<string>( new List<string> { "Rank", 
            "DisplayName", 
            "DisplayNameShort", 
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
            "Status",
            "TargetCollectionName"
        } );

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
                        fields[fieldName] = GetParticipantAttribute( source.Name.ToString() );
                        break;

                    case ResultFieldMethod.ATTRIBUTE:
                        fields[fieldName] = GetAttributeValue( source );
                        break;

                    case ResultFieldMethod.SCORE:
                        fields[fieldName] = GetScore( source );
                        break;
                    default:
                        //Should never get here
                        break;
                }
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
            "DisplayName", 
            "DisplayNameShort", 
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

            //Fields that are unique to the child Match ID that generated them
            "MatchLocation", 
            "MatchID", 
            "Creator", //"Orion Scoring System version 2.20.5.2"
            "Owner", //"OrionAcct000001"
            "Status", //"INTERMEDIATE"
            "TargetCollectionName"
            */

			ResultListMetadata metadata;

			switch (source) {
				case "Rank":
					if (resultEvent.Rank > 0)
						return resultEvent.Rank.ToString();
					else
						return "";
				
                case "DisplayName":
                    //Try the non depreciated field first
                    var dn = resultEvent.Participant.DisplayName;
                    if (!string.IsNullOrEmpty( dn ))
                        return dn;

                    //If not there, try thedeprecated field
                    dn = resultEvent.DisplayName;
                    if (!string.IsNullOrEmpty( dn ))
                        return dn;

                    return "Unknown";

                case "DisplayNameShort":
                    //Try the non depreciated field first
                    var dns = resultEvent.Participant.DisplayNameShort;
                    if (!string.IsNullOrEmpty( dns ))
                        return dns;

                    //If not there, try thedeprecated field
                    dns = resultEvent.DisplayName;
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

				case "Status":
					if (TryGetResultListMetadata( resultEvent.MatchID, out metadata ))
						return metadata.Status.Description();
					else
						return "";

				case "TargetCollectionName":
					if (TryGetResultListMetadata( resultEvent.MatchID, out metadata ))
						return metadata.TargetCollectionName;
					else
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
                        var fields = av.AttributeValue.GetDefintionFields();
                        var firstField = fields[0];
                        return av.AttributeValue.GetFieldValue( firstField.FieldName );
                    } catch ( Exception ex ) {
                        logger.Error( ex, "Likely casued by the user specifying an Attribute that is not a Simple Attribute." );
                        return "";
                    }
                }
            }

            //Couldn't find the attribute the user was asking for.
            return "";

        }

        private string GetScore( FieldSource source ) {
            /*
             * source is a dictionary. For an Attribute should be
             * {
                "Name": "Prone",
                "ScoreFormat": "Event"
                "ScoreConfigName" : "Integer" //If not listed, uses the variable scoreConfigDefaultName
             * }
             * Likely augmented later to support more than just simple attributes.
             */

            string scoreConfigName = resultListFormatted.ResultList.ScoreConfigName;

            Score score = GetScore( (string)source.Name );
            string scoreFormat = resultListFormatted.GetScoreFormat( source.ScoreFormat );

            return Scopos.BabelFish.Helpers.StringFormatting.FormatScore( scoreFormat, score );
        }

        private Score GetScore( string eventName ) {

            //.EventScores is the updated way (as of Jan 2024) to learn about a competitors score. Try using it first.
            EventScore scoreToReturn;
            if (resultEvent.EventScores != null && resultEvent.EventScores.TryGetValue( eventName, out scoreToReturn)) {
                return scoreToReturn.Score;
            }

            //.Score is the deprecated method, use it second.
            if (eventName == resultListFormatted.ResultList.EventName)
                return resultEvent.Score;

            foreach (var childEvent in resultEvent.Children) {
                if (childEvent.EventName == eventName) {
                    return childEvent.Score;
                }
            }

            //If we get here couldn't find it in the Result Event.
            //TODO search the ResultCOF for the score. 

            //For now return an empty Score data object
            return new Score();
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

            var source = (string)column.Body;
            var value = source.Replace( fields );

            var classes = new List<string>();
            foreach (var c in column.BodyClassList)
                classes.Add( (string)c );

            var cellValues = new CellValues( value, classes );

            //Check if the Column definition requires us to link to another page.
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
                returnValue = new CellValues();
                return false;
            }
        }

        /// <summary>
        /// Returns a list of RowLinkToData instances. Each instance references an external pages to link to from this row.
        /// Currently supports ResultCOF and PublicProfile. 
        /// NOTE: Event if the definition specifiefes PublciProfiel, if the user (represented
        /// in this row) does not have a PublicProfile then the option will not be included.
        /// </summary>
        public List<RowLinkToData> GetLinkToDataList() {

            List<RowLinkToData> list = new List<RowLinkToData>();

            foreach( var rowLinkTo in GetLinkToList() ) {
                var rowLinkToData = new RowLinkToData();
                switch( rowLinkTo ) {
                    case LinkToOption.ResultCOF:
                        rowLinkToData.LinkTo = rowLinkTo;
                        rowLinkToData.LinkToData = fields["ResultCOFID"];
                        list.Add( rowLinkToData );
                        break;

                    case LinkToOption.PublicProfile:
                        var userId = fields["UserID"];

                        if (!string.IsNullOrEmpty( userId ) && resultListFormatted.UserProfileLookup.HasPublicProfile( userId )) {
                            rowLinkToData.LinkTo = rowLinkTo;
                            rowLinkToData.LinkToData = resultListFormatted.UserProfileLookup.AccountURLLookUp( userId );
                            list.Add( rowLinkToData );
                        }

                        break;

                    default:
                        break;
                }
            }

            return list;
        }

        /// <summary>
        /// Returns a list of LinkToOption, which references external pages that this row should link to.
        /// Detaisl of the data can be learned from the function GetLinkToDataList()
        /// These values are defined in the RESULT LIST FORMAT definition. 
        /// </summary>
        /// <returns></returns>
        public abstract List<LinkToOption> GetLinkToList();

        /// <summary>
        /// Returns a list of CSS classes that should be applied to the row.
        /// These values are defined in the RESULT LIST FORMAT definition. 
        /// </summary>
        /// <returns></returns>
        public abstract List<string> GetClassList();
    }
}
