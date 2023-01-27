using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {

    /// <summary>
    /// Helper class that creates the added structure in the data model needed for Deserialzing a Match object from json.
    /// </summary>
    public class CourseOfFireWrapper : BaseClass {
        public ResultCOF ResultCOF = new ResultCOF();

        public override string ToString() {
            StringBuilder foo = new StringBuilder();
            foo.Append( "ResultCOF for " );
            foo.Append( ResultCOF.Participant.DisplayName );
            foo.Append( ": " );
            foo.Append( ResultCOF.MatchName );
            return foo.ToString();
        }
    }
}
