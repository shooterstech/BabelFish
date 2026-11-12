namespace Scopos.BabelFish.DataModel.OrionMatch {
    /// <summary>
    /// This class represents an Orion match attribute pre-Reconfigurable Rulebook. In time this class will be
    /// replaced with DataModel.Match.Attribute, which is the Reconfigurable Rulebook version.
    /// </summary>
    [Serializable]
    [Obsolete( "Use the Reconfigurable Rulebook ATTRIBUTE Definition instead." )]
    public class Attribute {
        public Attribute() {
            Values = new List<string>();
        }

        public string AttributeDef { get; set; }

        public List<string> Values { get; set; }
    }
}
