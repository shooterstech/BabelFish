
using Amazon.Auth.AccessControlPolicy;
using Scopos.BabelFish.DataModel.Definitions;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// A list of RemarkActions, each holding a ParticiapntRemark, reason, and visibility (show/don't show).
    /// <para>To avoid corrupting the data, use Apply(), AddShowRemark(), HideRemark(), or RemoveRemark()</para>
    /// </summary>
    [Serializable]
    public class RemarkList : List<RemarkAction> {
        //public List<Remark> remarks = new List<Remark>();

        public readonly List<ParticipantRemark> PriorityOfRemarks = new List<ParticipantRemark>() {
                    ParticipantRemark.DSQ,
                    ParticipantRemark.DNS,
                    ParticipantRemark.DNF,
                    ParticipantRemark.FIRST,
                    ParticipantRemark.SECOND,
                    ParticipantRemark.THIRD,
                    ParticipantRemark.ELIMINATED,
                    ParticipantRemark.BUBBLE,
                    ParticipantRemark.LEADER};

        public void Apply( CommandAutomationRemark commandAutomation ) {
            if ( commandAutomation.Action == RemarkVisibility.SHOW ) {
                this.AddShowParticipantRemark( commandAutomation.Condition, string.Empty, commandAutomation.Id );
            } else if ( commandAutomation.Action == RemarkVisibility.HIDE ) {
                this.HideParticipantRemark( commandAutomation.Condition, string.Empty, commandAutomation.Id );
            }
        }

        /// <summary>
        /// Adds a new RemarkAction, with visility SHOW, to this RemarkList.
        /// If this RemarkList is already shwoing a ParticipantRemark of type remark,
        /// then two Remarks are added, one SHOW and one HIDE (which keeps the balance of showing 1 or 0).
        /// </summary>
        /// <param name="remark"></param>
        /// <param name="reason"></param>
		public void AddShowParticipantRemark( ParticipantRemark remark, string reason = "", int actionId = 0 ) {
            bool hasRemarkAlready = this.IsShowingParticipantRemark( remark );

            var remarkActionShow = new RemarkAction() {
                ParticipantRemark = remark,
                Visibility = RemarkVisibility.SHOW,
                Reason = reason,
                ActionId = actionId
            };
            this.Add( remarkActionShow );

            if (hasRemarkAlready) {
                var remarkActionHide = new RemarkAction() {
                    ParticipantRemark = remark,
                    Visibility = RemarkVisibility.HIDE,
                    Reason = reason,
                    ActionId = actionId
                };
                this.Add( remarkActionHide );
            }

        }

        /// <summary>
        /// If this RemarkList is showing a ParticipantRemark of type remark, then a new RemarkAction
        /// is added to hide it. If the RemarkList is not showing a ParticpantRemark of type remark,
        /// then no RemarkAction is added.
        /// </summary>
        /// <param name="remark"></param>
        public void HideParticipantRemark( ParticipantRemark remark, string reason = "", int actionId = 0 ) {
            if (this.IsShowingParticipantRemark( remark )) {
                var remarkAction = new RemarkAction() {
                    ParticipantRemark = remark,
                    Visibility = RemarkVisibility.HIDE,
                    Reason = reason,
                    ActionId = actionId
                };
                this.Add( remarkAction );
            }
        }

        /// <summary>
        /// Removes all RemarkAction items in this RemarkList that have a
        /// command automation id equal to the passed in automationId.
        /// </summary>
        /// <param name="automationId"></param>
        public void RemoveAutomationRemark(int automationId) {
            List<RemarkAction> remarksToRemove = new List<RemarkAction>();
            foreach( var ra in this ) {
                if (ra.ActionId == automationId) {
                    remarksToRemove.Add( ra );
                }
            }

            foreach (var ra in remarksToRemove)
                this.Remove( ra );
        }

        /// <summary>
        /// Returns a boolean indicating if this RemarkList is currently showing a ParticipantRemark
        /// of type remark. 
        /// If this RemarkList has one or more RemarkActions of type remark but they are all hidden, then this 
        /// method will return false.
        /// </summary>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool IsShowingParticipantRemark( ParticipantRemark remark ) {
            int count = 0;
            foreach (var re in this) {
                if (re.ParticipantRemark == remark) {
                    if (re.Visibility == RemarkVisibility.SHOW) {
                        count++;
                    } else {
                        count--;
                    }
                }
            }

            //if everything else is working correclty, count should only ever be 0 or 1
            return count > 0;
        }

        /// <summary>
        /// Returns true if this Remark List has any shown ParticipantRemark in it.
        /// </summary>
        public bool HasAnyShownParticipantRemark {
            get {
                int count = 0;
                foreach (var re in this) {
                    if (re.Visibility == RemarkVisibility.SHOW) {
                        count++;
                    } else {
                        count--;
                    }
                }

                return count > 0;
            }
        }

        /// <summary>
        /// Returns an integer representing the number of times this RemarkList has a 
        /// Show visility RemarkAction with type remark.
        /// </summary>
        /// <param name="remark"></param>
        public int GetParticipantRemarkCount(ParticipantRemark remark) {
            int count = 0;
            foreach (var re in this)
                if (re.ParticipantRemark == remark && re.Visibility == RemarkVisibility.SHOW)
                    count++;

            return count;
        }

        /// <summary>
        /// Returns true if this RemarkList is shwoing one of the following remarks: DNS, DSQ, DNF, or ELIMINATED. 
        /// Returns flase if they do not (and would then mean that they are still competing).
        /// </summary>
        /// <returns></returns>
        public bool HasNonCompletionRemark {

            get {

                return (this.IsShowingParticipantRemark( ParticipantRemark.ELIMINATED )
                    || this.IsShowingParticipantRemark( ParticipantRemark.DNS )
                    || this.IsShowingParticipantRemark( ParticipantRemark.DNF )
                    || this.IsShowingParticipantRemark( ParticipantRemark.DSQ )
                    || this.IsShowingParticipantRemark( ParticipantRemark.THIRD )
                    || this.IsShowingParticipantRemark( ParticipantRemark.SECOND )
                    || this.IsShowingParticipantRemark( ParticipantRemark.FIRST ));
            }
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{this.Count} Actions: {GetSummary(false)}";
        }

        /// <summary>
        /// Returns the most important Remark. If the participant has the Remark more than once, it indicates it on the return string.
        /// </summary>
        /// <remarks>Same(ish) as python's RemarkHelperFunctions.GetMostImportantRemark()</remarks>
        public string GetSummary( bool useAbbreviations ) {
            if (this.IsShowingParticipantRemark( ParticipantRemark.DSQ ))
                return "DSQ";
            if (this.IsShowingParticipantRemark( ParticipantRemark.DNF ))
                return "DNF";
            if (this.IsShowingParticipantRemark( ParticipantRemark.DNS ))
                return "DNS";
            if (this.IsShowingParticipantRemark( ParticipantRemark.FIRST ))
                if (!useAbbreviations) {
                    return "FIRST";
                } else {
                    return "1ST";
                }
            if (this.IsShowingParticipantRemark( ParticipantRemark.SECOND ))
                if (!useAbbreviations) {
                    return "SECOND";
                } else {
                    return "2ND";
                }
            if (this.IsShowingParticipantRemark( ParticipantRemark.THIRD ))
                if (!useAbbreviations) {
                    return "THIRD";
                } else {
                    return "3RD";
                }
            if (this.IsShowingParticipantRemark( ParticipantRemark.QUALIFIED ))
                if (!useAbbreviations) {
                    return "QUALIFIED";
                } else {
                    return "QUAL";
                }
            if (this.IsShowingParticipantRemark( ParticipantRemark.ELIMINATED ))
                if (!useAbbreviations) {
                    return "ELIMINATED";
                } else {
                    return "ELIM";
                }
            if (this.IsShowingParticipantRemark( ParticipantRemark.BUBBLE ))
                if (!useAbbreviations) {
                    return "BUBBLE";
                } else {
                    return "BBL";
                }
            if (this.IsShowingParticipantRemark( ParticipantRemark.LEADER ))
                if (!useAbbreviations) {
                    return "LEADER";
                } else {
                    return "LDR";
                }

            int count = this.GetParticipantRemarkCount( ParticipantRemark.LEADER );
            if (count > 0)
                if (!useAbbreviations) {
                    return $"HELD LEAD x{count}";
                } else {
                    return $"LDRx{count}";
                }

            count = this.GetParticipantRemarkCount( ParticipantRemark.BUBBLE );
            if (count > 0)
                if (useAbbreviations) {
                    return $"SURVIVED x{count}";
                } else {
                    return $"SVDx{count}";
                }

            return "";
        }
    }
}
