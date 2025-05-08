using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST {
    public class ESTDisplayEventAssignment {

        public ESTDisplayEventAssignment() {

        }


        /// <summary>
        /// Mapping between the DisplayEvent (key) which is defined on the course of fire definition for Commands
        /// to the ViewConfig (value) to use.
        /// </summary>
        public EventAssignments EventAssignments { get; set; } = new EventAssignments();
    }
}
