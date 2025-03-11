using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions
{
    public class CommandAutomationRemark : CommandAutomation
    {
        /// <summary>
        /// Condition to add to the participants on the ranks given.
        /// </summary>
        public ParticipantRemark Condition { get; set; } = ParticipantRemark.BUBBLE;


        /// <summary>
        /// Action describes what should be done to the remark specified, on the participants specified.
        /// </summary>
        public RemarkVisibility Action { get; set; } = RemarkVisibility.HIDDEN;

        public CommandAutomationRemark()
        {
            this.Subject = CommandAutomationSubject.REMARK;
            this.Action = RemarkVisibility.HIDDEN;
            this.ParticipantRanks = string.Empty;
            this.Condition = ParticipantRemark.BUBBLE;
        }

        public void ShowRemarkOnParticipants(ResultList resultList)
        {
            Remark remark = new Remark() {
                ParticipantRemark = Condition,
                Reason = "Automatically given by Range Script",
                Visibility = Action
            };
            var ranks = GetParticipantRanksAsList();
            foreach (var item in resultList.Items)
            {
                if ( ranks.Contains(item.Rank) )
                {
                    item.Participant.RemarkList.Add(remark);
                }
            }
        }

        public void HideRemarkOnParticipants(ResultList resultList)
        {
            Remark remark = new Remark()
            {
                ParticipantRemark = Condition,
                Reason = "Automatically given by Range Script",
                Visibility = RemarkVisibility.SHOW
            };
            var ranks = GetParticipantRanksAsList();
            foreach (var item in resultList.Items)
            {
                //I am not sure this will work. does contains look for the same exact object? or one that has the same form?
                if (ranks.Contains(item.Rank))
                {
                    int? spot = item.Participant.RemarkList.Find(remark);
                    if(spot != null)
                    { 
                        item.Participant.RemarkList[(int)spot].Visibility = RemarkVisibility.HIDDEN;
                    }
                }
            }
        }
    }
}
