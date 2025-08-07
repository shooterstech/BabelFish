﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Range {
    public class TargetLine {

        public TargetLine() {

        }

        /// <summary>
        /// A human readable name given to this Target line.
        /// </summary>
        public string TargetLineName { get; set; }

        /// <summary>
        /// A unique, within the orion license account, id for this Target line. Usually a short semi-readable name.
        /// </summary>
        public string TargetLineLabel { get; set; }

        /// <summary>
        /// A human readable description of the Target LIne.
        /// </summary>
        public string Description { get; set; }

        public static TargetLine DefaultTargetLine {
            get {
                var tl = new TargetLine() {
                    TargetLineLabel = "10m",
                    TargetLineName = "10m Air Gun",
                    Description = "10m air gun target line."
                };

                return tl;
            }
        }
    }
}
