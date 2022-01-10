using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BabelFish.DataModel.Definitions {
    public class ShowInSegment  {

        [JsonConverter(typeof(StringEnumConverter))]
        public enum ShotType { BOTH, SIGHTER, COMPETITION }

        private List<string> validationErrorList = new List<string>();

        public ShowInSegment() {

        }

        [DefaultValue(null)]
        public List<string> StageLabel { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ShotType Competition { get; set; }

        /// <summary>
        /// Must be one of the following values
        /// ALL
        /// STRING (default)
        /// Past(n), where n is an integer
        /// </summary>
        [DefaultValue("STRING")]
        public string ShotPresentation { get; set; }

    }
}
