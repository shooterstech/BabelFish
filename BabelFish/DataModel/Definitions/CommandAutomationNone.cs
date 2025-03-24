using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions
{
    class CommandAutomationNone : CommandAutomation
    {
        public CommandAutomationNone()
        {
            this.Subject = CommandAutomationSubject.NONE;
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"{Subject}";
        }
    }
}
