using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch
{
    public class CommandAutomationIntermediateRemark : CommandAutomationIntermediate
    {
        public RemarkVisibility visibility { get; set; }

        public CommandAutomationSubject subject { get; set; } = CommandAutomationSubject.REMARK;

        public ParticipantRemark condition { get; set; }
    }
}
