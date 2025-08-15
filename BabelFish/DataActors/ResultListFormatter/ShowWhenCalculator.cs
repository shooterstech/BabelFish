using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Scopos.BabelFish.DataActors.ResultListFormatter {
    public class ShowWhenCalculator {

        public ResultListIntermediateFormatted RLF { get; private set; }
        public MatchID MatchID { get; private set; }

        public ShowWhenCalculator( ResultListIntermediateFormatted rlf ) {
            RLF = rlf;

            try {
                var matchId = RLF.RLIFList.ParentID;
                this.MatchID = new MatchID( matchId );
            } catch (Exception ex) {
                //We shouldn't ever get here
                this.MatchID = new MatchID( "1.1.1.0" );
            }
        }

        public bool Show( ShowWhenBase showWhen ) {
            if (showWhen is ShowWhenVariable)
                return Show( (ShowWhenVariable)showWhen, null );

            if (showWhen is ShowWhenSegmentGroup)
                return Show( (ShowWhenSegmentGroup)showWhen, null );

            //else showWhen is a ShowWhenEquation

            return Show( (ShowWhenEquation)showWhen, null );
        }

        public bool Show( ShowWhenBase showWhen, IParticipant ? participant ) {
            /*
             * TODO: Liam
             * 1. Implement this and the other overridden version of Show() that includes the IParticipant. 
             * 2. Make sure to deal with the case that participant is null.
             * 3. Implement new ShowWhenConditions
             *      HAS_REMARK_DNS
             *      HAS_REMARK_DNF
             *      HAS_REMARK_DSQ
             *      HAS_REMARK_BUBBLE
             *      HAS_REMARK_ELIMINATED
             *      What others to include?
             * 4. Document in H&M as part of ShowWhenCondition
             * 5. Write Unit Tests
             */
            if (showWhen is ShowWhenVariable)
                return Show((ShowWhenVariable)showWhen, participant);

            if (showWhen is ShowWhenSegmentGroup)
                return Show((ShowWhenSegmentGroup)showWhen, participant);

            //else showWhen is a ShowWhenEquation

            return Show((ShowWhenEquation)showWhen, participant);
        }

        public bool Show( ShowWhenVariable showWhen, IParticipant ? participant ) {
            bool answer = true;

            switch (showWhen.Condition) {
                case ShowWhenCondition.TRUE:
                    answer = true;
                    break;

                case ShowWhenCondition.FALSE:
                    answer = true;
                    break;

                case ShowWhenCondition.ENGAGEABLE:
                    answer = RLF.Engagable;
                    break;

                case ShowWhenCondition.NOT_ENGAGEABLE:
                    answer = ! RLF.Engagable;
                    break;

                case ShowWhenCondition.SUPPLEMENTAL:
                    answer = RLF.ShowSupplementalInformation;
                    break;

                case ShowWhenCondition.NOT_SUPPLEMENTAL:
                    answer = ! RLF.ShowSupplementalInformation;
                    break;

                case ShowWhenCondition.DIMENSION_SMALL:
                    answer = RLF.ResolutionWidth >= 576;
                    break;

                case ShowWhenCondition.DIMENSION_MEDIUM:
                    answer = RLF.ResolutionWidth >= 768;
                    break;

                case ShowWhenCondition.DIMENSION_LARGE:
                    answer = RLF.ResolutionWidth >= 992;
                    break;

                case ShowWhenCondition.DIMENSION_EXTRA_LARGE:
                    answer = RLF.ResolutionWidth >= 1200;
                    break;

                case ShowWhenCondition.DIMENSION_EXTRA_EXTRA_LARGE:
                    answer = RLF.ResolutionWidth >= 1400;
                    break;

                case ShowWhenCondition.DIMENSION_LT_SMALL:
                    answer = RLF.ResolutionWidth < 576;
                    break;

                case ShowWhenCondition.DIMENSION_LT_MEDIUM:
                    answer = RLF.ResolutionWidth < 768;
                    break;

                case ShowWhenCondition.DIMENSION_LT_LARGE:
                    answer = RLF.ResolutionWidth < 992;
                    break;

                case ShowWhenCondition.DIMENSION_LT_EXTRA_LARGE:
                    answer = RLF.ResolutionWidth < 1200;
                    break;

                case ShowWhenCondition.DIMENSION_LT_EXTRA_EXTRA_LARGE:
                    answer = RLF.ResolutionWidth < 1400;
                    break;

                case ShowWhenCondition.MATCH_TYPE_LOCAL:
                    answer = this.MatchID.LocalMatch;
                    break;

                case ShowWhenCondition.MATCH_TYPE_TOURNAMENT:
                    answer = this.MatchID.MatchGroup;
                    break;

                case ShowWhenCondition.MATCH_TYPE_VIRTUAL:
                    answer = this.MatchID.VirtualMatch;
                    break;

                case ShowWhenCondition.SHOT_ON_EST:
                    if (RLF.ResultList is null)
                        return false;

                    //If one or more of the VM locations have ESTs, then this will evaluate to true.
                    foreach( var md in RLF.ResultList.Metadata.Values ) {
                        if ( md.ScoringSystemType == ScoringSystem.EST ) {
                            return true;
                        }
                    }
                    return false;

                case ShowWhenCondition.SHOT_ON_PAPER:
					if (RLF.ResultList is null)
						return false;
					
                    //If one or more of the VM locations have PAPER, then this will evaluate to true.
					foreach (var md in RLF.ResultList.Metadata.Values) {
                        if (md.ScoringSystemType == ScoringSystem.PAPER) {
                            return true;
                        }
                    }
                    return false;

                case ShowWhenCondition.RESULT_STATUS_FUTURE:
					if (RLF.ResultList is null)
						return false;
					
                    answer = RLF.ResultList.Status == ResultStatus.FUTURE;
                    break;

                case ShowWhenCondition.RESULT_STATUS_INTERMEDIATE:
					if (RLF.ResultList is null)
						return false;
					
                    answer = RLF.ResultList.Status == ResultStatus.INTERMEDIATE;
                    break;

                case ShowWhenCondition.RESULT_STATUS_UNOFFICIAL:
					if (RLF.ResultList is null)
						return false;
					
                    answer = RLF.ResultList.Status == ResultStatus.UNOFFICIAL;
                    break;

                case ShowWhenCondition.RESULT_STATUS_OFFICIAL:
					if (RLF.ResultList is null)
						return false;
					
                    answer = RLF.ResultList.Status == ResultStatus.OFFICIAL;
                    break;

                case ShowWhenCondition.HAS_ANY_SHOWN_REMARK:
					
                    if ( participant == null || participant.Participant == null ) {
                        foreach ( var p in this.RLF.RLIFList.GetAsIRLItemsList() ) {
                            if ( p.Participant.RemarkList.HasAnyShownParticipantRemark) {
                                return true;
                            }
                        }
                        return false;
                    } else {
                        return participant.Participant.RemarkList.HasAnyShownParticipantRemark;
                    }


				case ShowWhenCondition.HAS_SHOWN_REMARK_LEADER:
                    if (participant == null || participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsShowingParticipantRemark(ParticipantRemark.LEADER);
                    break;


                case ShowWhenCondition.HAS_SHOWN_REMARK_FIRST:
                    if (participant == null || participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsShowingParticipantRemark( ParticipantRemark.FIRST);
                    break;


                case ShowWhenCondition.HAS_SHOWN_REMARK_SECOND:
                    if (participant == null || participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsShowingParticipantRemark( ParticipantRemark.SECOND);
                    break;


                case ShowWhenCondition.HAS_SHOWN_REMARK_THIRD:
                    if (participant == null || participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsShowingParticipantRemark( ParticipantRemark.THIRD);
                    break;


                case ShowWhenCondition.HAS_SHOWN_REMARK_DNS:
                    if (participant == null || participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsShowingParticipantRemark(ParticipantRemark.DNS);
                    break;

                case ShowWhenCondition.HAS_SHOWN_REMARK_DNF:
                    if (participant == null || participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsShowingParticipantRemark(ParticipantRemark.DNF);
                    break;

                case ShowWhenCondition.HAS_SHOWN_REMARK_DSQ:
                    if (participant == null || participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsShowingParticipantRemark(ParticipantRemark.DSQ);
                    break;

                case ShowWhenCondition.HAS_SHOWN_REMARK_BUBBLE:
                    if (participant == null || participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsShowingParticipantRemark(ParticipantRemark.BUBBLE);
                    break;

                case ShowWhenCondition.HAS_SHOWN_REMARK_ELIMINATED:
                    if (participant == null || participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsShowingParticipantRemark(ParticipantRemark.ELIMINATED);
                    break;

                case ShowWhenCondition.HAS_SHOWN_REMARK_QUALIFIED:
                    if (participant == null || participant.Participant == null) {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsShowingParticipantRemark( ParticipantRemark.QUALIFIED );
                    break;


                default:
                    //Shouldnt' get here, as it means a value got added to the ShowWhenCondition enum, but not added here.
                    answer = true;
                    break;
            }

            return answer;
        }

        public bool Show( ShowWhenEquation showWhen, IParticipant ? participant ) {

            bool answer = true;
            bool first = true;
            bool apployNot = false;
            bool breakForeach = false;

            foreach (var argument in showWhen.Arguments) {
                if (first) {
                    answer = Show( argument, participant );
                    first = false;
                } else {
                    switch (showWhen.Boolean) {
                        case ShowWhenBoolean.AND:
                            answer &= Show( argument, participant );
                            //If the answer is already false, we can stop evaluating
                            if (!answer)
                                breakForeach = true;
                            break;
                        case ShowWhenBoolean.OR:
                            answer |= Show( argument, participant );
                            //If the answer is already true, we can stop evaluating
                            if (answer)
                                breakForeach = true;
                            break;
                        case ShowWhenBoolean.XOR:
                            answer ^= Show( argument, participant );
                            break;
                        case ShowWhenBoolean.NAND:
                            answer &= Show( argument, participant );
                            apployNot = true;
                            break;
                        case ShowWhenBoolean.NOR:
                            answer |= Show( argument, participant );
                            apployNot = true;
                            break;
                        case ShowWhenBoolean.NXOR:
                            answer ^= Show( argument, participant );
                            apployNot = true;
                            break;
                    }
                }

                if (breakForeach)
                    break;
            }

            return answer ^ apployNot;
        }

        public bool Show( ShowWhenSegmentGroup showWhen, IParticipant ? participant ) {

            ResultListMetadata metadata;
            if ( this.RLF.ResultList.Metadata.TryGetValue( this.MatchID.ToString(), out metadata ) ) {
                return showWhen.SegmentGroupName.Equals( metadata.SegmentGroupName );
            }

            return false;
        }

        /// <summary>
        /// Bootstrap 5 breakpoints
        /// </summary>
        public static Dictionary<ShowWhenCondition, int> BreakPoints = new Dictionary<ShowWhenCondition, int>() {
            { ShowWhenCondition.DIMENSION_LT_SMALL, 0 },
			{ ShowWhenCondition.DIMENSION_SMALL, 576 },
			{ ShowWhenCondition.DIMENSION_MEDIUM, 768 },
			{ ShowWhenCondition.DIMENSION_LARGE, 992 },
			{ ShowWhenCondition.DIMENSION_EXTRA_LARGE, 1200 },
			{ ShowWhenCondition.DIMENSION_EXTRA_EXTRA_LARGE, 1400 }};

        /// <summary>
        /// Helper method to return the screen resolution necessary to evaluate all
        /// show wehn operations "DIMENSION_SIZE" to true. 
        /// </summary>
        /// <param name="showWhen"></param>
        /// <param name="largestResolutionSoFar"></param>
        /// <returns></returns>
        public static int GetLargestShowWhenResolution( ShowWhenBase showWhen, int largestResolutionSoFar = 0 ) {
            //Uses recusion

            if (showWhen is ShowWhenVariable showWhenVariable) {
                //This is the stop condition
                if (BreakPoints.TryGetValue( showWhenVariable.Condition, out int result ) && 
                    result > largestResolutionSoFar) {
                    return result;
				}
            } else if ( showWhen is ShowWhenEquation showWhenEquation) {
                //Use recusion to learn the largest resultion of each of the arguments
                foreach( var sw in showWhenEquation.Arguments) {
                    var result = GetLargestShowWhenResolution( sw );
                    if (result > largestResolutionSoFar) {
                        largestResolutionSoFar = result;
                    }
                }
            }
            //NOTE no need to evaluate ShowWHenSegmentGroup as they don't involve screen resolutions.

            //Also a stop condition
            return largestResolutionSoFar;
        }
    }
}
