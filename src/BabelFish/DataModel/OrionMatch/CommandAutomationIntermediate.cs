using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch
{
    /// <summary>
    /// objects holding a participant, typically a remark and it's visibility.
    /// </summary>
    public abstract class CommandAutomationIntermediate
    {
        public Participant participant { get; set; }

    }
}
