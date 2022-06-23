using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.Helpers;
using BabelFish.DataModel.Definitions;

namespace BabelFish.Requests.ScoreAPI
{
    public class GetScoreRequest : Request
    {
        private Dictionary<string, List<string>> ParameterList = new Dictionary<string, List<string>>();

        public GetScoreRequest(ScoreStyle scoreStyle, Dictionary<string, List<string>> parameterList, bool withAuthentication)
        {
            ScoreStyle = scoreStyle;
            WithAuthentication = withAuthentication;
            ParameterList = parameterList;
        }
        public ScoreStyle ScoreStyle { get; set; }

        /// <inheritdoc />
        public override string RelativePath
        {
            get { return $"/athlete/score/{EnumHelper.Description(ScoreStyle)}"; }
        }

        public override Dictionary<string, List<string>> QueryParameters
        {
            get
            {
                return ParameterList;
            }
        }
    }
}
