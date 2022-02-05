using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BabelFish.DataModel.OrionMatch
{
    public abstract class ResponseTemplate
    {
        public string Title { get; set; } = String.Empty;

        public List<String> Message { get; set; } = new List<String>();

        public List<String> ResponseCodes { get; set; } = new List<String>();

    }
}
