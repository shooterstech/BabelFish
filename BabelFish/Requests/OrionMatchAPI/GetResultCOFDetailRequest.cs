using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.Requests.OrionMatchAPI 
{
    public class GetResultCOFDetailRequest : Request
    {
        public GetResultCOFDetailRequest(string resultCOFId = "")
        {
            ResultCOFID = resultCOFId;
        }

        public string ResultCOFID { get; set; } = string.Empty;

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/resultcof/{ResultCOFID}"; }
        }
    }
}
