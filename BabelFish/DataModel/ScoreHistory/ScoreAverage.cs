using System;
using System.Collections.Generic;
using System.Text;
using ShootersTech.DataModel.Definitions;

namespace ShootersTech.DataModel.ScoreHistory {

    /// <summary>
    /// Represents the data returned by a GetScoreAverage API call.
    /// NOTE: there is no difference in the format of the data if Format==DAY versus
    /// Format!=Day (i.e. WEKE, or MONTH).
    /// 
    /// Key is the User Id of the athlete.
    /// Value is a dictionary of the scores they shot on specific dates.
    /// </summary>
    public class ScoreAverage : Dictionary<string, ScoreAveragesOnDate> {

    }

    /// <summary>
    /// Key is a string representing a Date.
    /// </summary>
    public class ScoreAveragesOnDate : Dictionary<string, StageStyleAverage>  {

    }

    /// <summary>
    /// Key is a string representing a SetName of a Stage Style
    /// </summary>
    public class StageStyleAverage : Dictionary<string, AveragedScore> {

    }
}
