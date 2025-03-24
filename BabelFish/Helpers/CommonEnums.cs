using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Scopos.BabelFish.Helpers {

        /*
         * EKA NOTE Jan 2025: Not sure if Scopos.BabelFish.Helpers is the right namespace for the SortBy enum, but I don't have anywhere better
         */

        /// <summary>
        /// Common enum for classes that implement the IComparer interface.
        /// Also used with the TieBreakingRule class.
        /// </summary>
        public enum SortBy {
            /// <summary>
            /// Sort by ascending order
            /// </summary>
            [Description( "Ascending" )]
            [EnumMember( Value = "Ascending" )]
            ASCENDING,

            /// <summary>
            /// Sort by descending order
            /// </summary>
            [Description( "Descending" )]
            [EnumMember( Value = "Descending" )]
            DESCENDING
        }
    }

