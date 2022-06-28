using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShootersTech.Responses
{
    interface IResponse
    {
        string Title { get; set; }
        List<String> Message { get; set; }
        List<String> ResponseCodes { get; set; }
    }
}
