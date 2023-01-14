using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class Logging
    {

        public Logging()
        {
            LogLevel = "INFO";
        }

        public string LogFile { get; set; }

        /// <summary>
        /// Must be one of DEBUG, INFO, WARNING, ERROR, CRITICAL
        /// </summary>
        public string LogLevel { get; set; }

        /// <summary>
        /// Returns a more human readable error level
        /// Will be Debug, Info, Warning, Default, or Minimal
        /// </summary>
        public string LogLevelDisplay
        {
            get
            {
                switch (LogLevel)
                {
                    case "ERROR":
                        return "Default";
                    case "CRITICAL":
                        return "Minimal";
                    default:
                        return $"{LogLevel[0]}{LogLevel.ToLower().Substring(1)}";
                }
            }
        }

        public bool Capability { get; set; }
    }
}