using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace BabelFish.DataModel.Score
{
    public class AverageScoreField
    {
        /// <summary>
        /// Decimal Score
        /// </summary>
        public float D { get; set; } = 0;

        /// <summary>
        /// Integer Score
        /// </summary>
        public float I { get; set; } = 0;

        /// <summary>
        /// Number of inner tens
        /// </summary>
        public float X { get; set; } = 0;
    }
}
