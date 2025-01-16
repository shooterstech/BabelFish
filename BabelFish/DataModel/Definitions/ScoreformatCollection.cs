using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Scopos.BabelFish.DataActors.Specification.Definitions;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {

	/// <summary>
	/// <para>A SCORE FORMAT COLLECTION defines a set of Score Config. Each Score Config consists of one or more Score Format key value pairs where the 
	/// key is the name of the Score Format and the value is a Score Format string. Each SCORE FORMAT COLLECTION may be used for any number of COURSE OF FIRE 
	/// scripts during competitions or practice. Each Score Format name included in the SCORE FORMAT COLLECTION may be used in the COURSE OF FIRE script 
	/// Event or Singular Definition Objects (under ScoreFormat).</para>
	/// 
	/// <para>A SCORE FORMAT COLLECTION may contain any number of ScoreConfigs, but each ScoreConfig must contain the same number of Score Format key value pairs.
    /// The Score Format keys must be the same across all ScoreConfigs.</para>
	/// </summary>
	[Serializable]
    public class ScoreFormatCollection : Definition
    {
        
        public ScoreFormatCollection() : base() {
            Type = DefinitionType.SCOREFORMATCOLLECTION;
        }


        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);

            if (ScoreConfigs == null)
                ScoreConfigs = new List<ScoreConfig>();
        }

        [JsonPropertyOrder ( 11  )]
        public List<string> ScoreFormats { get; set; } = new List<string>();

		[JsonPropertyOrder ( 12 )]
		public List<ScoreConfig> ScoreConfigs { get; set; } = new List<ScoreConfig>();

        public string GetDefaultScoreConfigName() {
            if (ScoreConfigs.Count > 0)
                return ScoreConfigs[0].ScoreConfigName;

            //NOTE: This really is an error condition, as all ScoreFormatCollection should have at least 1 ScoreConfig
            return "";
        }

        /// <summary>
        /// Helper function that returns a list of the names of the ScoreConfigs
        /// </summary>
        /// <returns></returns>
        public List<string> GetScoreConfigNames() {
            List<string> names = new List<string>();
            foreach( var sc in ScoreConfigs) {
                names.Add(sc.ScoreConfigName);
            }

            return names;
        }

        public bool IsValidScoreConfigName( string scoreConfigName ) {
            foreach (var sc in ScoreConfigs) {
                if (sc.ScoreConfigName == scoreConfigName)
                    return true;
            }

            return false;
        }

        /// <inheritdoc/>
        [JsonPropertyOrder( 99 )]
        [DefaultValue("")]
        public string Comment { get; set; } = string.Empty;

		/// <inheritdoc />
		public override async Task<bool> GetMeetsSpecificationAsync() {
            var validation = new IsScoreFormatCollectionValid();

			var meetsSpecification = await validation.IsSatisfiedByAsync( this );
			SpecificationMessages = validation.Messages;

			return meetsSpecification;
		}
	}
}
