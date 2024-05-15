using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.Helpers.Extensions;
using Score = Scopos.BabelFish.DataModel.Athena.Score;

namespace Scopos.BabelFish.DataActors.OrionMatch {

    /// <summary>
    /// Compares two IEventScores (e.g. Result COF or Result Event) by the rules defined in a 
    /// Ranking Rule Definition.
    /// </summary>
    public class CompareByRankingDirective : IComparer<IEventScores>, IEqualityComparer<IEventScores> {

        private enum TieBreakRuleList { RULES, LISTONLY };

        private Logger logger = LogManager.GetCurrentClassLogger();

        public CompareByRankingDirective( CourseOfFire courseOfFire, RankingDirective rankingDirective ) {
            this.CourseOfFire = courseOfFire;
            this.RankingDirective = rankingDirective;

            this.EventTree = EventComposite.GrowEventTree( this.CourseOfFire );
        }

        public CourseOfFire CourseOfFire { get; private set; }

        public RankingDirective RankingDirective { get; private set; }

        /// <summary>
        /// The top level Event Composite within the course of fire definition.
        /// </summary>
        public EventComposite EventTree { get; private set; }

        /// <summary>
        /// The ResultStatus of the Result List we are sorting. The default value is OFFICIAL which effectively 
        /// means all TieBreakingRules will be used. TieBreakingRules may be limited if ResultStatus is something
        /// other than OFFICIAL
        /// </summary>
        public ResultStatus ResultStatus { get; set; } = ResultStatus.OFFICIAL;

        /// <summary>
        /// Indicates, if true, that the Projected Score instance should be used instead of the Score instance.
        /// Would be the case if the result list we are sorting has .Projected = true.
        /// The default, false, means to always use the .Score instance.
        /// </summary>
        public bool Projected { get; set; } = false;

        /// <summary>
        /// Compares two IEventScores objects to determine which is greater than the other based on the Ranking Rule 
        /// Definition. The most common use for this is sorting a Result List of type Result Events. This method
        /// uses the definitions' .Rules and .ListOnly TieBreakingRules. Which means, if the definition was written
        /// correctly x should never equal y and a 0 will never be returned.  Note that this is different from 
        /// CompareByRankingRuleDefinition.Equals, that will return true if x and y are considered to have an 
        /// unbreakable tie.
        /// 
        /// Returns >= 1 if x is higher ranked than y
        /// Returns <= -1 if y is higher ranked than x
        /// 
        /// Shouldn't ever return 0 if the Definition was written correctly.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare( IEventScores x, IEventScores y ) {

            var compare = CompareByTieBreakingRuleList( TieBreakRuleList.RULES, x, y );

            if ( compare == 0 ) {
                compare = CompareByTieBreakingRuleList( TieBreakRuleList.LISTONLY, x, y );
            }            

            return compare;
        }

        /// <summary>
        /// Compares two IEventScores objects to determien if they have an unbreakable tie (return value True). 
        /// An unbreakable tie exists if all of the definitions .Rules passed without either x or y coming out ahead.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool Equals( IEventScores x, IEventScores y ) {

            var compare = CompareByTieBreakingRuleList( TieBreakRuleList.RULES, x, y );

            return compare == 0;
        }

        private int CompareByTieBreakingRuleList( TieBreakRuleList tieBreakingRuleType, IEventScores x, IEventScores y ) {

            var compare = 0;
            List<TieBreakingRule> tieBreakingRules;
            if (tieBreakingRuleType == TieBreakRuleList.RULES)
                tieBreakingRules = this.RankingDirective.Rules;
            else
                // TieBreakRuleList.LISTONLY
                tieBreakingRules = this.RankingDirective.ListOnly;

            foreach (var tieBreakingRule in this.RankingDirective.Rules) {

                //Skip this rule if the it specifies a Result Status less than the .ResultStaus of the result list.
                if (CompareResultStatus.COMPARER.Compare( this.ResultStatus, tieBreakingRule.ResultStatus ) >= 0) {

                    switch (tieBreakingRule.Method) {
                        case TieBreakingRuleMethod.SCORE:
                            compare = CompareScore( tieBreakingRule, x, y );
                            break;
                        case TieBreakingRuleMethod.COUNT_OF:
                            compare = CompareCountOf( tieBreakingRule, x, y );
                            break;

                        case TieBreakingRuleMethod.PARTICIPANT_ATTRIBUTE:
                            compare = CompareParticipantAttribute( tieBreakingRule, x, y );
                            break;

                        case TieBreakingRuleMethod.ATTRIBUTE:
                            compare = CompareAttribute( tieBreakingRule, x, y );
                            break;

                        default:
                            throw new NotImplementedException( $"Received an unexpected TieBreakingRuleMethod, '{tieBreakingRule.Method}'." );
                    }
                }
            }

            return compare;
        }

        private int CompareScore( TieBreakingRule rule, IEventScores x, IEventScores y ) {

            Score xScore, yScore;

            int compare = 0;

            if ( TryGetScore( x, rule.EventName, out xScore) &&  TryGetScore( y, rule.EventName, out yScore ) ) {
                //When the rule's .Method is "Score" the .Source shold always be a string. However, checking for it to prevent exceptions being thrown.
                if (rule.Source is string) {
                    string source = (string)rule.Source;
                    switch( source ) {
                        case "I":
                        case "i":
                            compare = xScore.I - yScore.I;
                            break;
                        case "X":
                        case "x":
                            compare = xScore.X - yScore.X;
                            break;
                        case "D":
                        case "d":
                            //To avoid floating point errors, need to evalute jus the first decimal place
                            compare = (int) (( xScore.D - yScore.D ) * 10);
                            break;
                        case "S":
                        case "s":
                            //To avoid floating point errors, need to evalute jus the first decimal place
                            compare = (int)((xScore.S - yScore.S) * 10);
                            break;
                        case "J":
                        case "j":
                            //To avoid floating point errors, need to evalute jus the first three decimal place
                            compare = (int)((xScore.J - yScore.J) * 100);
                            break;
                        case "K":
                        case "k":
                            //To avoid floating point errors, need to evalute jus the first three decimal place
                            compare = (int)((xScore.K - yScore.K) * 100);
                            break;
                        case "L":
                        case "l":
                            //To avoid floating point errors, need to evalute jus the first three decimal place
                            compare = (int)((xScore.L - yScore.L) * 100);
                            break;
                    }
                }
            }

            if (rule.SortOrder == Helpers.SortBy.DESCENDING)
                return compare;

            //Else if sort order is Ascending
            return -1 * compare;

        }

        private int CompareCountOf( TieBreakingRule rule, IEventScores x, IEventScores y ) {
            //EKA May 2024 Choosing not to implement (at least not yet) the CompareCountOf 
            logger.Error( "CompareCountOf() is not implemented. All comparisons are returning 0, meaning they are equal." );
            return 0;
        }

        private int CompareParticipantAttribute( TieBreakingRule rule, IEventScores x, IEventScores y ) {

            int compare = 0;
            if (rule.Source is string) {
                string source = ((string)rule.Source).ToUpper();
                switch (source) {
                    case "FAMILYNAME":
                        if (x.Participant is Individual && y.Participant is Individual)
                            compare = ((Individual)x.Participant).FamilyName.CompareTo( ((Individual)y.Participant).FamilyName );
                        break;

                    case "GIVENNAME":
                        if (x.Participant is Individual && y.Participant is Individual)
                            compare = ((Individual)x.Participant).GivenName.CompareTo( ((Individual)y.Participant).GivenName );
                        break;

                    case "MIDDLENAME":
                        if (x.Participant is Individual && y.Participant is Individual)
                            compare = ((Individual)x.Participant).MiddleName.CompareTo( ((Individual)y.Participant).MiddleName );
                        break;

                    case "COMPETITORNUMBER":
                        if (x.Participant is Individual && y.Participant is Individual)
                            compare = ((Individual)x.Participant).CompetitorNumber.CompareToAsIntegers( ((Individual)y.Participant).CompetitorNumber );
                        break;

                    case "DISPLAYNAME":
                        compare = x.Participant.DisplayName.CompareTo( y.Participant.DisplayName );
                        break;

                    case "DISPLAYNAMESHORT":
                        compare = x.Participant.DisplayNameShort.CompareTo( y.Participant.DisplayNameShort );
                        break;

                    case "HOMETOWN":
                        compare = x.Participant.HomeTown.CompareTo( y.Participant.HomeTown );
                        break;

                    case "COUNTRY":
                        compare = x.Participant.Country.CompareTo( y.Participant.Country );
                        break;

                    case "CLUB":
                        compare = x.Participant.Club.CompareTo( y.Participant.Club );
                        break;

                    default:
                        logger.Error( $"Unexpected Method value in TieBreakingRule '{(string)rule.Source}'." );
                        compare = 0;
                        break;

                }
            }

            if (rule.SortOrder == Helpers.SortBy.DESCENDING)
                return compare;

            //Else if sort order is Ascending
            return -1 * compare;
        }

        private int CompareAttribute( TieBreakingRule rule, IEventScores x, IEventScores y ) {

            int compare = 0;
            if (rule.Source is string) {
                string setName = ((string)rule.Source).ToUpper();

                dynamic xAttrValue = null;
                dynamic yAttrValue = null;

                foreach (var av in x.Participant.AttributeValues) {
                    if (av.AttributeDef == setName) {
                        try {
                            xAttrValue = av.AttributeValue.GetFieldValue();
                        } catch (Exception ex) {
                            logger.Error( ex, "Likely casued by the user specifying an Attribute that is not a Simple Attribute." );
                            return 0;
                        }
                    }
                }

                foreach (var av in y.Participant.AttributeValues) {
                    if (av.AttributeDef == setName) {
                        try {
                            xAttrValue = av.AttributeValue.GetFieldValue();
                        } catch (Exception ex) {
                            logger.Error( ex, "Likely casued by the user specifying an Attribute that is not a Simple Attribute." );
                            return 0;
                        }
                    }
                }

                if (xAttrValue != null && yAttrValue != null) 
                    compare = xAttrValue.CompareTo( yAttrValue );
            }

            if (rule.SortOrder == Helpers.SortBy.DESCENDING)
                return compare;

            //Else if sort order is Ascending
            return -1 * compare;
        }

        private bool TryGetScore( IEventScores eventScores, string eventName, out Score score ) {
            score = null;
            EventScore eventScore;

            if (!string.IsNullOrEmpty(eventName) && eventScores.EventScores.TryGetValue( eventName, out eventScore )) {
                //Try and get the Projected Score instance first, if and only if we're told to use it
                if (Projected && eventScore.Projected != null) {
                    score = eventScore.Projected;
                    return true;
                }

                //Normally, we would get the .Score dictionary
                score = eventScore.Score;
                return true;
            }

            return false;            
        }

        public int GetHashCode( IEventScores obj ) {
            //The IEqualityComparer<IEventScores> interface requires GetHasCode(). Not really sure how to implement it wisely.
            return obj.Participant.GetHashCode() ^ obj.EventScores.GetHashCode();
        }
    }
}
