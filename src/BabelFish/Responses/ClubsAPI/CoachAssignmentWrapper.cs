using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.Clubs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.Responses.ClubsAPI
{
    public class CoachAssignmentWrapper : BaseClass
    {
        public CoachAssignmentList CoachAssignmentList { get; set; } = new CoachAssignmentList();
    }
}
