using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataModel.OrionMatch
{
    /// <summary>
    /// Object that holds the RemarkName and Reason for a remark, if needed.
    /// This is mostly notation on the participants status within a match.
    /// </summary>
    [Serializable]
    public class Remark
    {
        /// <summary>
        /// this would be the name of the remark being given, DNS, DSQ, Eliminated, etc.
        /// </summary>
        public string RemarkName {  get; set; }

        /// <summary>
        /// A reason for a remark is not always given, but should be when the remark is assigned by the RSO
        /// </summary>
        public string Reason { get; set; } = string.Empty;
    }
}