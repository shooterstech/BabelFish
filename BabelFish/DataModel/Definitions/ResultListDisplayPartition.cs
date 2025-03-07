using BabelFish.DataModel.Definitions;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// Specifies how a row, in a ResultListIntermediatFormat should be decorated for styling.
    /// </summary>
    public class ResultListDisplayPartition : IReconfigurableRulebookObject {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ResultListDisplayPartition() {
            //RowClass = new List<string>();
        }

        public ResultListDisplayPartition( string rowClassDefault )
        {
            if (!string.IsNullOrEmpty(rowClassDefault))
                ClassSet.Add(new ClassSet(rowClassDefault, ShowWhenVariable.ALWAYS_SHOW.Clone() ));
            /*
            if (!string.IsNullOrEmpty( rowClassDefault ))
                RowClass.Add( rowClassDefault );

            if (!string.IsNullOrEmpty( rowClassDefault ))
                ClassList.Add( rowClassDefault );
            */
        }

        [OnDeserialized]
        internal void OnDeserializedMethod( StreamingContext context ) {

            if (ClassList == null)
                ClassList = new List<string>();

            if (ClassSet == null)
                ClassSet = new List<ClassSet>();
        }

        /// <summary>
        /// The list of css classes to assign to the rows within this Partition.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <listheader>While any css calss values may be used, the common values are:</listheader>
        /// <item>rlf-row-header</item>
        /// <item>rlf-row-athlete</item>
        /// <item>rlf-row-team</item>
        /// <item>rlf-row-child</item>
        /// <item>rlf-row-footer</item>
        /// </list>"
        /// </remarks>
        [Obsolete("Use .ClassSet instead.")]
        public List<string> ClassList { get; set; } = new List<string>();

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize ClassSEt when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeClassList() {
            return (ClassList != null && ClassList.Count > 0);
        }

        /// <summary>
        /// The list of ClassSet objects, each containing a CSS class (string) and ShowWhen object, to determine if the class should be used when displaying the row.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <listheader>While any css calss values may be used, the common values are:</listheader>
        /// <item>rlf-row-header</item>
        /// <item>rlf-row-athlete</item>
        /// <item>rlf-row-team</item>
        /// <item>rlf-row-child</item>
        /// <item>rlf-row-footer</item>
        /// </list>"
        /// </remarks>
        public List<ClassSet> ClassSet { get; set; } = new List<ClassSet>();

        public void CombineClassListSet()
        {
            if (ClassList is null || ClassList.Count == 0) return;

            if (ClassSet is null || ClassSet.Count == 0)
            {
                //true is classSet list and Convert to class set
                foreach (var cl in ClassList)
                {
                    var cs = new ClassSet();
                    cs.Name = cl;
                    cs.ShowWhen = ShowWhenVariable.ALWAYS_SHOW.Clone();
                    ClassSet.Add(cs);
                }
            }
        }

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize ClassSEt when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeClassSet() {
            return (ClassSet != null && ClassSet.Count > 0);
        }

        /// <inheritdoc/>
        [JsonPropertyOrder( 99 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
    }
}
