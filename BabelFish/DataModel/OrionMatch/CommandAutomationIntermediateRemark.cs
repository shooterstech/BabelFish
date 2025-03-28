using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch
{
    public class CommandAutomationIntermediateRemark : CommandAutomationIntermediate
    {
        /// <summary>
        /// visibility of a remark to add to participants
        /// </summary>
        public RemarkVisibility visibility { get; set; }

        /// <summary>
        /// subject to add to participants, REMARK in this case
        /// </summary>
        public CommandAutomationSubject subject { get; set; } = CommandAutomationSubject.REMARK;

        /// <summary>
        /// specific remark to add to participants.
        /// </summary>
        public ParticipantRemark condition { get; set; }
    }
}
