using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BabelFish.DataModel;

namespace BabelFish.Responses
{
    ////public abstract class ResponseTemplate : IResponse
    ////{
    ////    public string Title { get; set; } = String.Empty;

    ////    public List<String> Message { get; set; } = new List<String>();

    ////    public List<String> ResponseCodes { get; set; } = new List<String>();

    ////    public MessageResponse MessageResponse { get; set; } = new MessageResponse();
    ////}

    public class MessageResponse : IResponse
    {
        public string Title { get; set; } = string.Empty;
        public List<String> Message { get; set; } = new List<string>();
        public List<String> ResponseCodes { get; set; } = new List<string>();
    }
}
