using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class IncidentReportRuleDefinition {

        public IncidentReportRuleDefinition() { }

        public IncidentReportRuleDefinition(string name, string ruleReference) {
            this.Name = name;
            this.RuleReference = ruleReference;
            this.SuggestedResolution = "";
        }

        public IncidentReportRuleDefinition(string name, string ruleReference, string suggestedResolution) {
            this.Name = name;
            this.RuleReference = ruleReference;
            this.SuggestedResolution = suggestedResolution;
        }

    /// <summary>
    /// Human readable description of the rule violation
    /// </summary>
    public string Name { get; set; }

        /// <summary>
        /// A rule reference for the Incident Report
        /// </summary>
        public string RuleReference { get; set; }

        /// <summary>
        /// The resolution applied to the competitor for this Incident
        /// </summary>
        public string SuggestedResolution { get; set; }

        public override string ToString() {
            return Name;
        }
    }
}
