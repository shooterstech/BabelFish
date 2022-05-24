using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShootersTech.DataModel.Athena.AbstractEST
{
    public class TargetLift
    {

        public TargetLift()
        {

        }

        public bool Capability { get; set; }

        public int DefaultHeight { get; set; }

        public bool InMotion { get; set; }

        public int Height { get; set; }

        public int MaximumHeight { get; set; }

        public int MinimumHeight { get; set; }

        public string Error { get; set; }

        public string Event { get; set; }

        /// <summary>
        /// JSON formatted string representing the hardware settings for this target lift.
        /// Setting values are hardware specific, and could differ from model to model. Purposefully leaving as a 
        /// json formatted string to allow manufacturers to set their own values.
        /// </summary>
        public string Settings { get; set; }

        /// <summary>
        /// Returns the Settings as a JObject.
        /// </summary>
        /// <returns></returns>
        public JObject GetSettings()
        {
            return JObject.Parse(Settings);
        }

        /// <summary>
        /// Sets Settings (a JSON formatted string) to the passed in JObject
        /// </summary>
        /// <param name="settings"></param>
        public void SetSettings(JObject settings)
        {
            Settings = settings.ToString();
        }
    }
}