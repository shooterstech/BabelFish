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
using Scopos.BabelFish.Requests.OrionMatchAPI;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {

    /// <summary>
    /// Delegate to allow the overload of GetParticipantAttribute for the common fields.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="resultEvent"></param>
    /// <param name="rlf"></param>
    /// <returns></returns>
    public delegate string ParticipantAttributeOverload( IRLIFItem item, ResultListIntermediateFormatted rlf );

    public abstract class ResultListIntermediateFormattedRow {

        /// <summary>
        /// List of feild values that are always included (the user doesn't have to define these in their definition).
        /// these values are pulled from the .Participant dictionary within the Result COF.
        /// </summary>
        public static readonly IList<string> StandardParticipantAttributeFields = new ReadOnlyCollection<string>( new List<string> {
            "Rank",
            "RankOrder",
            "RankDelta", //NOTE Externally, this is known as Rank Trends
            "Empty",
            "DisplayName",
            "DisplayNameShort",
            "DisplayNameAbbreviated", //Deprecated
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
            "LastShot",    //Only avaliable on individual result lists
            "Remark",
            "RankOrSquadding",  //Avaliable only with Squadding information
            "Squadding",        //Avaliable only with Squadding information
            "Relay",            //Avaliable only with Squadding information
            "FiringPoint",      //Avaliable only with Squadding information
            "FiringOrder",      //Avaliable only with Squadding information
            "Squad",            //Avaliable only with Squadding information
            "Bank",             //Avaliable only with Squadding information
            "Range",            //Avaliable only with Squadding information
            "Reentry"           //Avaliable only with Squadding information
        } );

        public static readonly Dictionary<string, string> AliasEventNames = new Dictionary<string, string>() {
            {"Qualifcation", "Individual" },
            {"Individual", "Qualification" },
            {"Team", "Qualification" },
        };

        protected readonly Dictionary<string, string> _fields = new Dictionary<string, string>();
        protected readonly IRLIFItem _item;
        protected readonly ResultEvent _resultEvent; //When _item comes from a ResultList object, then _item is a ResultEvent. Which is rather common. So we will have a special helper variable to use it.
        protected readonly ResultListIntermediateFormatted _resultListFormatted; //This row's parent container
        protected Logger _logger = LogManager.GetCurrentClassLogger();

        public ResultListIntermediateFormattedRow( ResultListIntermediateFormatted rlf, IRLIFItem re ) {

            _resultListFormatted = rlf;
            _item = re;

            //When _item comes from a ResultList object, then _item is a ResultEvent. Which is rather common. So we will have a special helper variable to use it.
            if (_item is ResultEvent)
                _resultEvent = (ResultEvent)_item;
            else
                _resultEvent = null;

            //Pull in the standard ParticipantAttribute Fields that are always included
            foreach (var field in StandardParticipantAttributeFields) {
                _fields[field] = GetParticipantAttribute( field );
            }

            foreach (var field in _resultListFormatted.ResultListFormat.Fields) {
                var fieldName = (string)field.FieldName;
                var source = field.Source;

                //Ignore if we already have a field with the same name
                if (_fields.ContainsKey( fieldName ))
                    continue;

                switch (field.Method) {
                    case ResultFieldMethod.PARTICIPANT_ATTRIBUTE:
                        _fields[fieldName] = GetTruncatedValue( source, GetParticipantAttribute( source.Name.ToString() ) );
                        break;

                    case ResultFieldMethod.ATTRIBUTE:
                        _fields[fieldName] = GetTruncatedValue( source, GetAttributeValue( source ) );
                        break;

                    case ResultFieldMethod.SCORE:
                        _fields[fieldName] = GetScore( source, false );
                        break;

                    case ResultFieldMethod.PROJECTED_SCORE:
                        _fields[fieldName] = GetScore( source, true );
                        break;

                    case ResultFieldMethod.GAP:
                        _fields[fieldName] = GetGap( source );
                        break;

                    case ResultFieldMethod.COMPLETION:
                        //NOT IMPLEMENTED YET. Exists for future expansion.
                        _fields[fieldName] = string.Empty;
                        break;

                    case ResultFieldMethod.RECORD:
                        //NOT IMPLEMENTED YET. Exists for future expansion.
                        _fields[fieldName] = string.Empty;
                        break;

                    default:
                        //Should never get here
                        break;
                }
            }
        }

        /// <summary>
        /// Recalculates the standard participant attribute fields (e.g. rank, display name, club, etc). Is useful
        /// to call if the user overrode one of the GetParticipantAttribute Pointers
        /// </summary>
        public void RefreshStandardParticipantAttributeFields() {
            //Pull in the standard ParticipantAttribute Fields that are always included
            foreach (var field in StandardParticipantAttributeFields) {
                _fields[field] = GetParticipantAttribute( field );
            }
        }

        /// <summary>
        /// Helper method that basically indicates if the standard fields should be truncated because the screen resolution is shorten. 
        /// </summary>
        private bool LessThanLarge {
            get {
                return this._resultListFormatted.ResolutionWidth < 992;
            }
        }

        /// <summary>
        /// Method is responsible for implementing the .TrucateAt rule in the FieldSource.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="untruncatedValue"></param>
        /// <returns></returns>
        private string GetTruncatedValue( FieldSource source, string untruncatedValue ) {
            if (source.TruncateAt > 3 && untruncatedValue.Length > source.TruncateAt) {
                return $"{untruncatedValue.Substring( 0, source.TruncateAt - 3 )}...";
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

            ResultListMetadata metadata;

            switch (source) {
                case "Rank":
                    if (_resultListFormatted.GetParticipantAttributeRankPtr != null)
                        return _resultListFormatted.GetParticipantAttributeRankPtr( this._item, this._resultListFormatted );

                    if (_resultEvent is null)
                        return "";

                    int rank = GetRank();

                    if (rank > 0)
                        return rank.ToString();

                    return "";

                case "RankOrder":
                    if (_resultListFormatted.GetParticipantAttributeRankOrderPtr != null)
                        return _resultListFormatted.GetParticipantAttributeRankOrderPtr( this._item, this._resultListFormatted );

                    if (_resultEvent is null)
                        return "";

                    int rankOrder = GetRankOrder();

                    if (rankOrder > 0)
                        return rankOrder.ToString();

                    return "";


                case "RankDelta":
                    if (_resultListFormatted.GetParticipantAttributeRankDeltaPtr != null)
                        return _resultListFormatted.GetParticipantAttributeRankDeltaPtr( this._item, this._resultListFormatted );

                    if (_resultEvent is null)
                        return "";

                    if (this._resultEvent.CurrentlyCompetingOrRecentlyDone()
                        && this._resultListFormatted.ShowSupplementalInformation) {
                        int rankDelta = GetRankDelta();

                        if (rankDelta > 0)
                            return $"+{rankDelta}";
                        if (rankDelta < 0)
                            return rankDelta.ToString();
                    }

                    return "";

                case "RankOrSquadding":
                    if (_resultListFormatted.GetParticipantAttributeRankOrSquaddingPtr != null)
                        return _resultListFormatted.GetParticipantAttributeRankOrSquaddingPtr( this._item, this._resultListFormatted );

                    if (_resultEvent is null)
                        return GetParticipantAttribute( "Squadding" );

                    //Return squadding if the athlete hasn't started. Return rank if they have.

                    if (_resultEvent.GetStatus() == ResultStatus.FUTURE)
                        return GetParticipantAttribute( "Squadding" );

                    return GetParticipantAttribute( "Rank" );


                case "Empty":
                    if (_resultListFormatted.GetParticipantAttributeEmptyPtr != null)
                        return _resultListFormatted.GetParticipantAttributeEmptyPtr( this._item, this._resultListFormatted );

                    return "";

                case "DisplayName":
                case "DisplayNameAbbreviated": //Deprecated

                    if (_resultListFormatted.GetParticipantAttributeDisplayNamePtr != null)
                        return _resultListFormatted.GetParticipantAttributeDisplayNamePtr( this._item, this._resultListFormatted );

                    if (this.LessThanLarge) {

                        //First try the regular display name
                        var dna = _item.Participant.DisplayName;
                        if (!string.IsNullOrEmpty( dna ) && dna.Length <= 20)
                            return dna;

                        //if that's too long, try the display name short, if it exists
                        dna = _item.Participant.DisplayNameShort;
                        if (!string.IsNullOrEmpty( dna ) && dna.Length <= 20)
                            return dna;

                        //If that's too long, go back to the regular display name and truncate it
                        dna = _item.Participant.DisplayName;
                        if (!string.IsNullOrEmpty( dna ))
                            return StringFormatting.GetTruncatedString( dna );

                    } else {
                        var dn = _item.Participant.DisplayName;
                        if (!string.IsNullOrEmpty( dn ))
                            return dn;
                    }

                    return "Unknown";

                case "DisplayNameShort":
                    if (_resultListFormatted.GetParticipantAttributeDisplayNameShortPtr != null)
                        return _resultListFormatted.GetParticipantAttributeDisplayNameShortPtr( this._item, this._resultListFormatted );

                    var dns = _item.Participant.DisplayNameShort;
                    if (!string.IsNullOrEmpty( dns ))
                        return dns;

                    return "Unknown";

                case "FamilyName":
                    if (_resultListFormatted.GetParticipantAttributeFamilyNamePtr != null)
                        return _resultListFormatted.GetParticipantAttributeFamilyNamePtr( this._item, this._resultListFormatted );

                    if (_item.Participant is Individual)
                        return ((Individual)_item.Participant).FamilyName;
                    else
                        return "";

                case "GivenName":
                    if (_resultListFormatted.GetParticipantAttributeGivenNamePtr != null)
                        return _resultListFormatted.GetParticipantAttributeGivenNamePtr( this._item, this._resultListFormatted );

                    if (_item.Participant is Individual)
                        return ((Individual)_item.Participant).GivenName;
                    else
                        return "";

                case "MiddleName":
                    if (_resultListFormatted.GetParticipantAttributeMiddleNamePtr != null)
                        return _resultListFormatted.GetParticipantAttributeMiddleNamePtr( this._item, this._resultListFormatted );

                    if (_item.Participant is Individual)
                        return ((Individual)_item.Participant).MiddleName;
                    else
                        return "";

                case "HomeTown":
                    if (_resultListFormatted.GetParticipantAttributeHomeTownPtr != null)
                        return _resultListFormatted.GetParticipantAttributeHomeTownPtr( this._item, this._resultListFormatted );

                    return _item.Participant.HomeTown;

                case "Country":
                    if (_resultListFormatted.GetParticipantAttributeCountryPtr != null)
                        return _resultListFormatted.GetParticipantAttributeCountryPtr( this._item, this._resultListFormatted );

                    return _item.Participant.Country;

                case "Club":
                    if (_resultListFormatted.GetParticipantAttributeClubPtr != null)
                        return _resultListFormatted.GetParticipantAttributeClubPtr( this._item, this._resultListFormatted );
                    return _item.Participant.Club;

                case "CompetitorNumber":
                    if (_resultListFormatted.GetParticipantAttributeCompetitorNumberPtr != null)
                        return _resultListFormatted.GetParticipantAttributeCompetitorNumberPtr( this._item, this._resultListFormatted );

                    if (_item.Participant is Individual)
                        return ((Individual)_item.Participant).CompetitorNumber;
                    else
                        return "";

                case "ResultCOFID":
                    if (_resultListFormatted.GetParticipantAttributeResultCOFIDPtr != null)
                        return _resultListFormatted.GetParticipantAttributeResultCOFIDPtr( this._item, this._resultListFormatted );

                    if (_resultEvent is null)
                        return "";

                    return _resultEvent.ResultCOFID;

                case "UserID":
                    if (_resultListFormatted.GetParticipantAttributeUserIDPtr != null)
                        return _resultListFormatted.GetParticipantAttributeUserIDPtr( this._item, this._resultListFormatted );

                    if (_item.Participant is Individual)
                        return ((Individual)_item.Participant).UserID;
                    else
                        return "";

                case "LocalDate":
                    if (_resultListFormatted.GetParticipantAttributeLocalDatePtr != null)
                        return _resultListFormatted.GetParticipantAttributeLocalDatePtr( this._item, this._resultListFormatted );

                    if (_resultEvent is null)
                        return "";

                    return _resultEvent.LocalDate.ToString();

                case "MatchID":
                    if (_resultListFormatted.GetParticipantAttributeMatchIDPtr != null)
                        return _resultListFormatted.GetParticipantAttributeMatchIDPtr( this._item, this._resultListFormatted );

                    if (_resultEvent is null)
                        return "";

                    //This is the local match id, which likely different from the Parent ID in a virtual match
                    return _resultEvent.MatchID;

                case "MatchLocation":
                case "MatchLocationAbbreviation": //Deprecated
                    if (_resultListFormatted.GetParticipantAttributeMatchLocationPtr != null)
                        return _resultListFormatted.GetParticipantAttributeMatchLocationPtr( this._item, this._resultListFormatted );

                    if (_resultEvent is null)
                        return "";

                    if (TryGetResultListMetadata( _resultEvent.MatchID, out metadata )) {
                        if (this.LessThanLarge) {
                            return StringFormatting.GetTruncatedString( metadata.MatchLocation );
                        } else {
                            return metadata.MatchLocation;
                        }
                    }

                    return "";

                case "Creator":
                    if (_resultListFormatted.GetParticipantAttributeCreatorPtr != null)
                        return _resultListFormatted.GetParticipantAttributeCreatorPtr( this._item, this._resultListFormatted );

                    if (_resultEvent is null)
                        return "";

                    if (TryGetResultListMetadata( _resultEvent.MatchID, out metadata ))
                        return metadata.Creator;
                    else
                        return "";

                case "Owner":
                case "OwnerId":
                case "OwnerID":
                    if (_resultListFormatted.GetParticipantAttributeOwnerPtr != null)
                        return _resultListFormatted.GetParticipantAttributeOwnerPtr( this._item, this._resultListFormatted );

                    if (_resultEvent is null)
                        return "";

                    if (TryGetResultListMetadata( _resultEvent.MatchID, out metadata ))
                        return metadata.OwnerId;
                    else
                        return "";

                case "TargetCollectionName":
                    if (_resultListFormatted.GetParticipantAttributeTargetCollectionNamePtr != null)
                        return _resultListFormatted.GetParticipantAttributeTargetCollectionNamePtr( this._item, this._resultListFormatted );

                    if (_resultEvent is null)
                        return "";

                    if (TryGetResultListMetadata( _resultEvent.MatchID, out metadata ))
                        return metadata.TargetCollectionName;
                    else
                        return "";

                case "Status":
                    if (_resultListFormatted.GetParticipantAttributeStatusPtr != null)
                        return _resultListFormatted.GetParticipantAttributeStatusPtr( this._item, this._resultListFormatted );

                    if (_resultEvent is null)
                        return "";

                    return GetStatus().Description();

                case "LastShot":
                    if (_resultListFormatted.GetParticipantAttributeLastShotPtr != null)
                        return _resultListFormatted.GetParticipantAttributeLastShotPtr( this._item, this._resultListFormatted );

                    if (_resultEvent is null)
                        return "";

                    var lastShot = _resultEvent.LastShot;
                    if (lastShot != null && (DateTime.UtcNow - lastShot.TimeScored.ToUniversalTime()).TotalSeconds < 300) {

                        string scoreFormat = _resultListFormatted.GetScoreFormat( "Shots" );

                        return StringFormatting.FormatScore( scoreFormat, lastShot.Score );
                    }
                    return "";

                case "Remark":
                    if (_resultListFormatted.GetParticipantAttributeRemarkPtr != null)
                        return _resultListFormatted.GetParticipantAttributeRemarkPtr( this._item, this._resultListFormatted );

                    return GetRemarks( this.LessThanLarge );

                case "Squadding":
                    if (_resultListFormatted.GetParticipantAttributeSquaddingPtr != null)
                        return _resultListFormatted.GetParticipantAttributeSquaddingPtr( this._item, this._resultListFormatted );

                    return GetSquaddingAssignment()?.ToString( this.LessThanLarge ) ?? "";


                case "Relay":
                    if (_resultListFormatted.GetParticipantAttributeRelayPtr != null)
                        return _resultListFormatted.GetParticipantAttributeRelayPtr( this._item, this._resultListFormatted );

                    var sa = GetSquaddingAssignment();

                    if (sa != null && sa is SquaddingAssignmentFiringPoint safp)
                        return safp.Relay;
                    if (sa != null && sa is SquaddingAssignmentBank sab)
                        return sab.Relay;

                    return "";

                case "FiringPoint":
                    if (_resultListFormatted.GetParticipantAttributeFiringPointPtr != null)
                        return _resultListFormatted.GetParticipantAttributeFiringPointPtr( this._item, this._resultListFormatted );

                    var sa1 = GetSquaddingAssignment();

                    if (sa1 != null && sa1 is SquaddingAssignmentFiringPoint safp1)
                        return safp1.FiringPoint;

                    return "";

                case "FiringOrder":
                    if (_resultListFormatted.GetParticipantAttributeFiringOrderPtr != null)
                        return _resultListFormatted.GetParticipantAttributeFiringOrderPtr( this._item, this._resultListFormatted );

                    var sa2 = GetSquaddingAssignment();

                    if (sa2 != null)
                        return sa2.FiringOrder.ToString();

                    return "";

                case "Squad":
                    if (_resultListFormatted.GetParticipantAttributeSquadPtr != null)
                        return _resultListFormatted.GetParticipantAttributeSquadPtr( this._item, this._resultListFormatted );

                    var sa3 = GetSquaddingAssignment();

                    if (sa3 != null && sa3 is SquaddingAssignmentSquad sas3)
                        return sas3.Squad;

                    return "";

                case "Bank":
                    if (_resultListFormatted.GetParticipantAttributeBankPtr != null)
                        return _resultListFormatted.GetParticipantAttributeBankPtr( this._item, this._resultListFormatted );

                    var sa4 = GetSquaddingAssignment();

                    if (sa4 != null && sa4 is SquaddingAssignmentBank sab4)
                        return sab4.Bank;

                    return "";

                case "Range":
                    if (_resultListFormatted.GetParticipantAttributeRangePtr != null)
                        return _resultListFormatted.GetParticipantAttributeRangePtr( this._item, this._resultListFormatted );

                    var sa5 = GetSquaddingAssignment();

                    if (sa5 != null)
                        return sa5.Range;

                    return "";

                case "Reentry":
                    if (_resultListFormatted.GetParticipantAttributeReentryPtr != null)
                        return _resultListFormatted.GetParticipantAttributeReentryPtr( this._item, this._resultListFormatted );

                    var sa6 = GetSquaddingAssignment();

                    if (sa6 != null)
                        return sa6.ReentryTag;

                    return "";

                default:
                    return "UNKNOWN";
            }
        }

        private bool TryGetResultListMetadata( string matchID, out ResultListMetadata metadata ) {
            metadata = null;

            if (_resultListFormatted.ResultList == null
                || _resultListFormatted.ResultList.Metadata == null
                || string.IsNullOrEmpty( matchID ))
                return false;

            return _resultListFormatted.ResultList.Metadata.TryGetValue( matchID, out metadata );

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

            foreach (var av in _item.Participant.AttributeValues) {
                if (av.AttributeDef == source.Name) {
                    try {
                        return av.AttributeValue.GetFieldValue();
                    } catch (Exception ex) {
                        _logger.Error( ex, "Likely casued by the user specifying an Attribute that is not a Simple Attribute." );
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

            var eventName = (string)source.Name;
            Score score = GetScore( eventName, tryAndUseProjected );
            string scoreFormat = _resultListFormatted.GetScoreFormat( source.ScoreFormat );

            var formattedScore = StringFormatting.FormatScore( scoreFormat, score );

            //If we are returning a projected score, demarcate it with "(P)".
            //EKA NOTE: November 2025: I would really like how a projected score is demarcated to be configurable.
            //Just not sure how best to do that right now. One possibility is to use ResultFieldMethod.Completion in 
            //conjunction with the projected score.
            if ( tryAndUseProjected
                && _resultEvent.EventScores.TryGetValue( eventName, out EventScore scoreToReturn )
                && scoreToReturn.Projected != null
                && scoreToReturn.Status == ResultStatus.INTERMEDIATE) {
                formattedScore += "(P)";
            }

            return formattedScore;
        }

        public Score GetScore( string eventName, bool tryAndUseProjected = false ) {

            if (_resultEvent is null)
                return new Score();

            EventScore scoreToReturn;

            if (_resultEvent.EventScores != null) {
                if (_resultEvent.EventScores.TryGetValue( eventName, out scoreToReturn )) {

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

                /*
                 * EKA Note June 2024: As we migrate more and more orion events over to the Reconfigurable Rulebook COFs
                 * there is a mix of old and new event names. The idea behind using the .aliasEventNames is to address this
                 * changing of event names. Hopefully, this chunk of code can be removed soon.
                 */
                var aliasEventName = "";
                if (AliasEventNames.TryGetValue( eventName, out aliasEventName )) {
                    if (_resultEvent.EventScores.TryGetValue( aliasEventName, out scoreToReturn )) {
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


            if (_resultEvent.ResultCofScores != null) {
                if (_resultEvent.ResultCofScores.TryGetValue( eventName, out scoreToReturn )) {

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

            //For now return an empty Score data object
            return new Score();
        }

        public Participant GetParticipant() {
            return this._item.Participant;
        }

        private string GetGap( FieldSource source ) {
            if (IsChildRow
                || this._item.Participant.RemarkList.HasNonCompletionRemark) {
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
            var otherRow = this._resultListFormatted.GetRowAtRankOrder( otherRank );
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
            string scoreFormat = _resultListFormatted.GetScoreFormat( source.ScoreFormat );
            return StringFormatting.FormatScore( scoreFormat, scoreDifference );
        }

        /// <summary>
        /// Returns the value of the Rank field for this Result Event. If the Result List
        /// is in the INTERMEDIATE status, then the Projected Rank is returned.
        /// </summary>
        /// <returns></returns>
        public int GetRank() {

            if (_resultEvent is null)
                return 0;

            if (this._resultListFormatted.ResultList.Projected
                && this._resultListFormatted.ResultList.Status == ResultStatus.INTERMEDIATE
                && _resultEvent.ProjectedRank > 0)
                return _resultEvent.ProjectedRank;

            if (_resultEvent.Rank > 0)
                return _resultEvent.Rank;

            return 0;
        }


        /// <summary>
        /// Returns the value of the Rank sort order field for this Result Event. If the Result List
        /// is in the INTERMEDIATE status, then the Projected Rank sort order is returned.
        /// </summary>
        /// <returns></returns>
        public int GetRankOrder() {

            if (_resultEvent is null)
                return 0;

            if (this._resultListFormatted.ResultList.Projected
                && this._resultListFormatted.ResultList.Status == ResultStatus.INTERMEDIATE) {
                if (_resultEvent.ProjectedRankOrder > 0)
                    return _resultEvent.ProjectedRankOrder;
                if (_resultEvent.ProjectedRank > 0)
                    return _resultEvent.ProjectedRank;
            }

            if (_resultEvent.RankOrder > 0)
                return _resultEvent.RankOrder;

            return 0;
        }

        public int GetRankDelta() {

            if (_resultEvent is null)
                return 0;

            return _resultEvent.RankDelta;
        }

        public ResultStatus GetStatus() {

            if (_resultEvent is null)
                return ResultStatus.FUTURE; //Not sure what lese to use ? 

            return _resultEvent.GetStatus();
        }

        /// <summary>
        /// Returns the field value for the passed in field name.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException">Thrown if fieldName is not known.</exception>"
        public string GetFieldValue( string fieldName ) {
            return _fields[fieldName];
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
                value = _fields[fieldName];
                return true;
            } catch (Exception ex) {
                _logger.Error( ex );
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

            var column = _resultListFormatted.ResultListFormat.Format.Columns[index];

            string source = column.Body;
            if (this.IsChildRow && !string.IsNullOrEmpty( column.Child ))
                source = column.Child;
            string value = source.Replace( _fields );

            var classes = new List<string>();

            foreach (var c in column.ClassSet) {
                if (_resultListFormatted.ShowWhenCalculator.Show( c.ShowWhen )) {
                    classes.Add( (string)c.Name );
                }
            }

            var cellValues = new CellValues( this._resultListFormatted, value, classes );
            cellValues.Body = column.Body;
            cellValues.Child = column.Child;

            //Check if the Column definition requires us to link to another page.
            if (this._resultListFormatted.Engagable) {
                switch (column.BodyLinkTo.ToString()) {
                    case "ResultCOF":
                        cellValues.LinkTo = LinkToOption.ResultCOF;
                        cellValues.LinkToData = _fields["ResultCOFID"];
                        break;

                    case "PublicProfile":
                        var userId = _fields["UserID"];

                        if (!string.IsNullOrEmpty( userId ) && _resultListFormatted.UserProfileLookup.HasPublicProfile( userId )) {
                            //User had a public profile
                            cellValues.LinkTo = LinkToOption.PublicProfile;
                            cellValues.LinkToData = _resultListFormatted.UserProfileLookup.AccountURLLookUp( userId );
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
                returnValue = new CellValues( this._resultListFormatted );
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

        /// <summary>
        /// Returns a string, that tries and encapsulates all of the Remarks that a participant has. 
        /// </summary>
        /// <returns></returns>
        public string GetRemarks( bool useAbbreviation ) {
            return this._item.Participant.RemarkList.GetSummary( useAbbreviation );
        }

        public void SetSquaddingAssignment( SquaddingAssignment squadding ) {
            if (squadding != null)
                this._item.SquaddingAssignment = squadding;
        }

        public SquaddingAssignment GetSquaddingAssignment() {
            return this._item.SquaddingAssignment;
        }

		/// <summary>
		/// Returns a boolean indicating if this row should be shown based on the RLIF's .ShowRanks property.
		/// </summary>
        /// <remarks>If true, this property overrides the other "Show" properties for the body rows</remarks>
		/// <returns></returns>
		public virtual bool ShowRowBasedOnShowRanks() {

            //If _resultEvent is null,, then this row is from a SquaddingList, and there are no rankings
            if (_resultEvent is null)
                return false;

            return this._resultListFormatted.ShowRanks >= this.GetRank();

        }

		/// <summary>
		/// Returns a boolean indicating if this row should be shown based on the RLIF's .ShowNumberOfChildren property.
		/// </summary>
		/// <returns></returns>
		public abstract bool ShowRowBasedOnShowNumberOfChildren();

        /// <summary>
        /// Returns a boolean indicating if this row should be shown based on teh RLIF's .ShowNumberOfBodyRows property
        /// </summary>
        /// <returns></returns>
        public abstract bool ShowRowBasedOnShowNumberOfBodies();

        /// <summary>
        /// Returns true if this row has a status that is part of the RLF's ShowStatuses property.
        /// OR if INTERMEDIATE is in ShowStatuses, will also return true if this rwo recently just finished
        /// shooting.
        /// </summary>
        /// <returns></returns>
        public virtual bool ShowRowBasedOnShownStatus() {

            if (_resultListFormatted.ShowStatuses == null)
                return false;

            if (_resultListFormatted.ShowStatuses.Contains( this.GetStatus() ))
                return true;

            //This is a special carve out for marksmen who just finished shooting. So techncially they would be UNOFFICIAL
            //but as far as displaying them in shown rows we want to pretend they are INTERMEDIATE for a bit longer.
            if (_resultListFormatted.ShowStatuses.Contains( ResultStatus.INTERMEDIATE )
                && _resultEvent != null && _resultEvent.CurrentlyCompetingOrRecentlyDone())
                return true;

            return false;
        }

        /// <summary>
        /// Returns a boolean indicating if this row should be shown based on the RLIF's .ShowRelay property.
        /// Which provides the name of a relay to match, in order to show.
        /// </summary>
        /// <returns></returns>
        public virtual bool ShowRowBasedOnShowRelay() {

            //Always show, if .ShowRelay does not have a filter on it (aka empty string)
            if (this._resultListFormatted.ShowRelay == string.Empty)
                return true;

            //If we don't have squadding, then don't apply the filter.
            if (_item.SquaddingAssignment == null)
                return true;

            if (_item.SquaddingAssignment is SquaddingAssignmentFiringPoint safp
                && safp.Relay == this._resultListFormatted.ShowRelay)
                return true;

            if (_item.SquaddingAssignment is SquaddingAssignmentBank sab
                && sab.Relay == this._resultListFormatted.ShowRelay)
                return true;

            //SquaddingAssignmentSquad does not use relays

            return false;
        }

		/// <summary>
		/// Returns a boolean indicating if this row should be shown based on the RLIF's .ShowZeroScoresBeforeOFFICIAL and .ShowZeroScoresWithOFFICIAL properties.
		/// </summary>
		/// <returns></returns>
		public virtual bool ShowRowBasedZeroScores() {
            //if _resutlEvent is null, then it's from a squadding list
            if (this._resultEvent is null)
                return true;

            //Check if the score is zero
            if (this.GetScore( this._resultListFormatted.ResultList.EventName, false ).IsZero) {
                //If we get here, the score is zero.

                if (this._resultListFormatted.ShowZeroScoresWithOFFICIAL
                    && this._resultListFormatted.ResultList.Status == ResultStatus.OFFICIAL)
                    return true;

                if (this._resultListFormatted.ShowZeroScoresBeforeOFFICIAL
                    && (this._resultListFormatted.ResultList.Status == ResultStatus.FUTURE
                        || this._resultListFormatted.ResultList.Status == ResultStatus.INTERMEDIATE
                        || this._resultListFormatted.ResultList.Status == ResultStatus.UNOFFICIAL) )
                    return true;

                //We do show this row if the score is zero due to DNS, DNF, or DSQ
                if (GetParticipant().RemarkList.IsShowingParticipantRemark( ParticipantRemark.DNS )
                    || GetParticipant().RemarkList.IsShowingParticipantRemark( ParticipantRemark.DNF )
                    || GetParticipant().RemarkList.IsShowingParticipantRemark( ParticipantRemark.DSQ )) {
                    return true;
                } 

                return false;

            } else {
                //If we get here, the score is above zero
                return true;
            }
        }

        internal virtual void ResetNumberOfChildRowsLeftToShow() {
            ; //default behavior is to do nothing, and let the BodyRow implement it;
        }

        public bool RowIsShown { get; internal set; } = false;

	}
}
