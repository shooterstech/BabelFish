using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.BabelFish.DataModel.OrionMatch;

namespace ShootersTech.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure in the data model needed for Deserialzing a Match object from json.
    /// </summary>
    public class ResultListWrapper
    {
        public ResultList ResultList = new ResultList();

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("ResultList for ");
            foo.Append(ResultList.ResultName);
            return foo.ToString();
        }
    }
}
