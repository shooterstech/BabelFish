using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Score
{
    public class AverageScoreField
    {
        //public Guid ScoreAverage { get; set; }
        public string UserId { get; set; }

        public override string ToString()
        {
            StringBuilder foo = new StringBuilder();
            foo.Append("Score for ");
            foo.Append(UserId);
            return foo.ToString();
        }
    }
}
