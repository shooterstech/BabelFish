using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BabelFish.DataModel;

namespace BabelFish.Responses
{
    public class MessageResponse : IResponse
    {
        public string Title { get; set; } = string.Empty;
        public List<String> Message { get; set; } = new List<string>();
        public List<String> ResponseCodes { get; set; } = new List<string>();
    }
}
