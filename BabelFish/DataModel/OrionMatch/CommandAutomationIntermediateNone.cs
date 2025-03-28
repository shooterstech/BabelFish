using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch
{
    public class CommandAutomationIntermediateNone : CommandAutomationIntermediate
    {

        public CommandAutomationSubject subject { get; set; } = CommandAutomationSubject.NONE;

    }
}
