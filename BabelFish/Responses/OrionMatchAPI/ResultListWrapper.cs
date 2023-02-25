using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure in the data model needed for Deserialzing a ResultList object from json.
    /// </summary>
    public class ResultListWrapper : BaseClass {
        public ResultList ResultList = new ResultList();

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( "ResultList for " );
            foo.Append( ResultList.ResultName );
            return foo.ToString();
        }
    }
}
