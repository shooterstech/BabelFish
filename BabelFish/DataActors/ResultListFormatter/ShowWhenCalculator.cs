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
                var matchId = RLF.ResultList.Metadata.Keys.First();
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

                case ShowWhenCondition.SUPPLIEMENTAL:
                    answer = RLF.ShowSupplementalInformation;
                    break;

                case ShowWhenCondition.NOT_SUPPLIEMENTAL:
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

                case ShowWhenCondition.RESULT_STATUS_FUTURE:
                    answer = RLF.ResultList.Status == ResultStatus.FUTURE;
                    break;

                case ShowWhenCondition.RESULT_STATUS_INTERMEDIATE:
                    answer = RLF.ResultList.Status == ResultStatus.INTERMEDIATE;
                    break;

                case ShowWhenCondition.RESULT_STATUS_UNOFFICIAL:
                    answer = RLF.ResultList.Status == ResultStatus.UNOFFICIAL;
                    break;

                case ShowWhenCondition.RESULT_STATUS_OFFICIAL:
                    answer = RLF.ResultList.Status == ResultStatus.OFFICIAL;
                    break;

                case ShowWhenCondition.HAS_REMARK_FIRST:
                    if (participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.HasRemark(ParticipantRemark.FIRST);
                    break;


                case ShowWhenCondition.HAS_REMARK_SECOND:
                    if (participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.HasRemark(ParticipantRemark.SECOND);
                    break;


                case ShowWhenCondition.HAS_REMARK_THIRD:
                    if (participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.HasRemark(ParticipantRemark.THIRD);
                    break;


                case ShowWhenCondition.HAS_REMARK_DNS:
                    if (participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.HasRemark(ParticipantRemark.DNS);
                    break;

                case ShowWhenCondition.HAS_REMARK_DNF:
                    if (participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.HasRemark(ParticipantRemark.DNF);
                    break;

                case ShowWhenCondition.HAS_REMARK_DSQ:
                    if (participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.HasRemark(ParticipantRemark.DSQ);
                    break;

                case ShowWhenCondition.HAS_REMARK_BUBBLE:
                    if (participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.HasRemark(ParticipantRemark.BUBBLE);
                    break;

                case ShowWhenCondition.HAS_REMARK_ELIMINATED:
                    if (participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.HasRemark(ParticipantRemark.ELIMINATED);
                    break;

                case ShowWhenCondition.LAST_REMARK_DNS:
                    if (participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsLastRemark(ParticipantRemark.DNS, false);
                    break;

                case ShowWhenCondition.LAST_REMARK_DNF:
                    if (participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsLastRemark(ParticipantRemark.DNF, false);
                    break;

                case ShowWhenCondition.LAST_REMARK_DSQ:
                    if (participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsLastRemark(ParticipantRemark.DSQ, false);
                    break;

                case ShowWhenCondition.LAST_REMARK_BUBBLE:
                    if (participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsLastRemark(ParticipantRemark.BUBBLE, false);
                    break;

                case ShowWhenCondition.LAST_REMARK_ELIMINATED:
                    if (participant.Participant == null)
                    {
                        answer = false;
                        break;
                    }
                    answer = participant.Participant.RemarkList.IsLastRemark(ParticipantRemark.ELIMINATED, false);
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
                    answer = Show( argument );
                    first = false;
                } else {
                    switch (showWhen.Boolean) {
                        case ShowWhenBoolean.AND:
                            answer &= Show( argument );
                            //If the answer is already false, we can stop evaluating
                            if (!answer)
                                breakForeach = true;
                            break;
                        case ShowWhenBoolean.OR:
                            answer |= Show( argument );
                            //If the answer is already true, we can stop evaluating
                            if (answer)
                                breakForeach = true;
                            break;
                        case ShowWhenBoolean.XOR:
                            answer ^= Show( argument );
                            break;
                        case ShowWhenBoolean.NAND:
                            answer &= Show( argument );
                            apployNot = true;
                            break;
                        case ShowWhenBoolean.NOR:
                            answer |= Show( argument );
                            apployNot = true;
                            break;
                        case ShowWhenBoolean.NXOR:
                            answer ^= Show( argument );
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
    }
}
