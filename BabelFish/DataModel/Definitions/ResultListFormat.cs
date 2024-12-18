using Newtonsoft.Json;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A ResultListFormat describes how to convert a ResultList into an intermediate format for
    /// displaying. The intermediat format is implementation independent, meaning it doesn't know if it
    /// will be dsiplayed on a computer monitor, mobile phone, or a drawing on the side of the wall. 
    /// 
    /// The ResultListFormat will describes in a 2D fashion the data from the ResultList to display.
    /// </summary>
    public class ResultListFormat : Definition, ICopy<ResultListFormat>
    {

        public ResultListFormat() : base() {
            Type = DefinitionType.RESULTLISTFORMAT;

            Fields = new List<ResultListField>();

            Format = new ResultListFormatDetail();
        }

        /// <inheritdoc/>
        public ResultListFormat Copy() {
            ResultListFormat rlf = new ResultListFormat();
            this.Copy( rlf );
            rlf.ScoreFormatCollectionDef = this.ScoreFormatCollectionDef;
            rlf.ScoreConfigDefault = this.ScoreConfigDefault;
            if (this.Fields != null) {
                foreach (var rlfield in this.Fields) {
                    rlf.Fields.Add( rlfield.Copy() );
                }
            }
            //unsure if this should be nullable....
            rlf.Format = this.Format.Copy();
            return rlf;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod( StreamingContext context ) {
            base.OnDeserializedMethod( context );

            if (Fields == null)
                Fields = new List<ResultListField>();

            if (Format == null)
                Format = new ResultListFormatDetail();
        }

        /// <summary>
        /// String, formated as a set name. The set name of the ScoreFormatCollection to use when formatting score results. 
        /// 
        /// the default value is v1.0:orion:Standard Score Formats.
        /// </summary>
        [JsonProperty( Order = 11 )]
        public string ScoreFormatCollectionDef { get; set; } = "v1.0:orion:Standard Score Formats";

        /// <summary>
        /// The name of the ScoreConfig to use if none other is specified.
        /// </summary>
        /// <remarks>The ScoreConfigName is usually specified by the Result List.</remarks>
        [JsonProperty( Order = 12 )]
        public string ScoreConfigDefault { get; set; } = string.Empty;

        /// <summary>
        /// A list of ResultListFields that define field values that will be used in the text output of the 
        /// Result List Intermediate Fromat cell values. 
        /// </summary>
        [JsonProperty( Order = 13 )]
        public List<ResultListField> Fields { get; set; }

        /// <summary>
        /// Describes the intermediate format for cells of data within a Result List. 
        /// </summary>
        [JsonProperty( Order = 14 )]
        public ResultListFormatDetail Format { get; set; }

		/// <inheritdoc />
		public override async Task<bool> GetMeetsSpecificationAsync() {
            var validation = new IsResultListFormatValid();

			var meetsSpecification = await validation.IsSatisfiedByAsync( this );
			SpecificationMessages = validation.Messages;

			return meetsSpecification;
		}
	}
}
