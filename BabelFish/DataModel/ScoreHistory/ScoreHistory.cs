using System;
using System.Collections.Generic;
using System.Text;

namespace ShootersTech.DataModel.ScoreHistory {

    /// <summary>
    /// Represents the data returned by a Get score History API Call
    /// Key is the athlete's User ID, which is UUID formatted.
    /// Value is a ScoreHistoryOnDate object.
    /// 
    /// T is going to be a concrete implementation of SchreHistoryBase. 
    /// </summary>
    public class ScoreHistory<T> : Dictionary<string, ScoreHistoryOnDate<T>> {

    }

    /// <summary>
    /// Key is a string representing a Date, e.g. "2022-06-30".
    /// </summary>
    public class ScoreHistoryOnDate<T> : Dictionary<string, List<T>> {

    }
}
