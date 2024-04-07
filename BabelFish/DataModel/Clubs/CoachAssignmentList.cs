using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Clubs
{

    /// <summary>
    /// Represents the data returned by a CoachAssignmentCRUD API call.
    /// </summary>
    public class CoachAssignmentList 
    {

        public CoachAssignmentList() { }

        public int LicenseNumber { get; set; }

        public List<string> Items { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"CoachAssignmentList with {Items.Count} items";
        }

    }
}
