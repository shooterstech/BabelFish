using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Range {
    public class DisplayState {

        public DisplayState() {
            FiringPoints = new List<string>();
            RankedLists = new List<string>();
        }

        [DefaultValue(true)]
        public bool Enabled { get; set; }

        /// <summary>
        /// A list of firing point numbers (represented by strings) that this Display object
        /// should display information for.
        /// </summary>
        public List<string> FiringPoints { get; set; }

        /// <summary>
        /// The name of the Event Assignment json file currently in use. Or at least believed to be.
        /// </summary>
        [DefaultValue( "AthenaDefaultAssignments.json" )]
        public string CurrentEventAssignments { get; set; } = "AthenaDefaultAssignments.json";

        /// <summary>
        /// The name of the Event Assignment json file to use as the default mapping of Event Assignments 
        /// (Display Event name to View Configuration name) for a competition.
        /// </summary>
        [DefaultValue("AthenaDefaultAssignments.json")]
        public string QualificationEventAssignments { get; set; } = "AthenaDefaultAssignments.json";

        /// <summary>
        /// The name of the Event Assignment json file to use as the default mapping of Event Assignments 
        /// (Display Event name to View Configuration name) for a competition.
        /// </summary>
        [DefaultValue("AthenaDefaultAssignments.json")]
        public string FinalEventAssignments { get; set; } = "AthenaDefaultAssignments.json";

        /// <summary>
        /// The name of the Event Assignment json file to use as the default mapping of Event Assignments 
        /// (Display Event name to View Configuration name) during practice.
        /// </summary>
        [DefaultValue( "AthenaPracticeAssignments.json" )]
        public string PracticeEventAssignments { get; set; } = "AthenaPracticeAssignments.json";

        /// <summary>
        /// The name of the Image Key Mapping file currently in use. 
        /// </summary>
        [DefaultValue( "ClubDefault.json" )]
        public string ImageKeyMapping { get; set; } = "ClubDefault.json";

        [DefaultValue("")]
        public string RankedList { get; set; }

        public List<string> RankedLists { get; set; }

        /// <summary>
        /// The name of the Color Scheme currently in use.
        /// </summary>
        [DefaultValue("AthenaColorScheme.json")]
        public string ColorScheme { get; set; } = "AthenaColorScheme.json";

        
    }
}
