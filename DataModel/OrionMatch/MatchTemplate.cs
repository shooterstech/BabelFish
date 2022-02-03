using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BabelFish.DataModel.OrionMatch
{
    public abstract class MatchTemplate
    {
        public string Title { get; set; } = "";

        public string[] Message { get; set; } = null;

        public string[] ResponseCodes { get; set; } = null;
    }
}
