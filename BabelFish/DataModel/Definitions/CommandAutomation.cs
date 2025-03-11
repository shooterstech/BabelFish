using Newtonsoft.Json.Linq;
using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Definitions
{
    public abstract class CommandAutomation
    {
        // maybe need to add action for applying penalties?
        // I am not sure why that would go here, and not immediately be applied by shot attributes.
        //      (which should in turn apply a remark.... maybe I see it...) - Liam

        public CommandAutomationSubject Subject { get; set; } = CommandAutomationSubject.NONE;

        /// <summary>
        /// ValueString object that is the items to apply an action of remark to.
        /// </summary>
        public string ParticipantRanks { get; set; } = string.Empty;

        /// <summary>
        /// Generates a list of Firing Lanes based on FiringLanes specified in property.
        /// </summary>
        /// <returns></returns>
        public List<int> GetParticipantRanksAsList()
        {
            List<int> list = new List<int>();

            ValueSeries vs = new ValueSeries(ParticipantRanks);

            for (int i = vs.StartValue; i <= vs.EndValue; i += vs.Step)
            {
                list.Add(i);
            }

            return list;
        }
    }
}
