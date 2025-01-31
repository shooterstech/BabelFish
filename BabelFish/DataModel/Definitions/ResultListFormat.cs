using System.Text.Json;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// A ResultListFormat describes how to convert a ResultList into an intermediate format for
    /// displaying. The intermediat format is implementation independent, meaning it doesn't know if it
    /// will be dsiplayed on a computer monitor, mobile phone, or a drawing on the side of the wall. 
    /// 
    /// The ResultListFormat will describes in a 2D fashion the data from the ResultList to display.
    /// </summary>
    public class ResultListFormat : Definition {

        public ResultListFormat() : base() {
            Type = DefinitionType.RESULTLISTFORMAT;

            Fields = new List<ResultListField>();

            Format = new ResultListFormatDetail();
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
		[G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11 )]
        public string ScoreFormatCollectionDef { get; set; } = "v1.0:orion:Standard Score Formats";

        /// <summary>
        /// The name of the ScoreConfig to use if none other is specified.
        /// </summary>
        /// <remarks>The ScoreConfigName is usually specified by the Result List.</remarks>
		[G_STJ_SER.JsonPropertyOrder( 12 )]
        [G_NS.JsonProperty( Order = 12 )]
        public string ScoreConfigDefault { get; set; } = string.Empty;

        /// <summary>
        /// A list of ResultListFields that define field values that will be used in the text output of the 
        /// Result List Intermediate Fromat cell values. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 13 )]
        [G_NS.JsonProperty( Order = 13 )]
        public List<ResultListField> Fields { get; set; }

        /// <summary>
        /// Describes the intermediate format for cells of data within a Result List. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 14 )]
        [G_NS.JsonProperty( Order = 14 )]
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
