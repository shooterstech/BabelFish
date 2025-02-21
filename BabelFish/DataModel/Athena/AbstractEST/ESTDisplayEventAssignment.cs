﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    /// <summary>
    /// This file is not used and may be deleted.
    /// </summary>
    public class ESTDisplayEventAssignment
    {

        public ESTDisplayEventAssignment()
        {
            EventAssignments = new Dictionary<string, string>();
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {
            //Providate default values if they were not read during deserialization

            if (EventAssignments == null)
            {
                EventAssignments = new Dictionary<string, string>();
            }
        }


        /// <summary>
        /// Mapping between the DisplayEvent (key) which is defined on the course of fire definition for Commands
        /// to the ViewConfig (value) to use.
        /// </summary>
        public Dictionary<string, string> EventAssignments { get; set; }
    }
}