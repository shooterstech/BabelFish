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
            throw new NotImplementedException();
        }

        public void HideRemarkOnParticipants(ResultList resultList)
        {
            throw new NotImplementedException();
        }
    }
}
