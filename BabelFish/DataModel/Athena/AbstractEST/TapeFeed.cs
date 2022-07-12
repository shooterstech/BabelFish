using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShootersTech.BabelFish.DataModel.Athena.AbstractEST
{
    public class TapeFeed
    {

        public TapeFeed()
        {

        }

        public bool Capability { get; set; }

        public int Advance { get; set; }

        public int DefaultAdvance { get; set; }

        public bool InMotion { get; set; }

        public int LastAdvance { get; set; }

        public string Event { get; set; }

        /// <summary>
        /// JSON formatted string representing the hardware settings for this tape feed.
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