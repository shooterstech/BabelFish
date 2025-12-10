using System.Runtime.Serialization;
using Scopos.BabelFish.DataActors.Specification.Definitions;

namespace Scopos.BabelFish.DataModel.Definitions {

    /// <summary>
    /// <para>A TARGET COLLECTION defines a set of TARGET definitions that are used together for multiple COURSE OF FIRE scripts.  with each other during competitions or practice. 
    /// It may only list one TARGET definition, or it may list multiple TARGET definitions. Each index is then used as part of a COURSE OF FIRE script.</para>
    /// 
    /// <para>For example, in high power, one TARGET COLLECTION could include the 200yds. SR target, the 300yds SR-3, and the 600yds MR-1 target for the National Match Course. 
    /// Another TARGET SET, for the same style of shooting could define the 200yds. SR target, the 300yds reduced for 200yds SR-42, and the 600yds reduced for 200yds MR-2. </para>
    /// 
    /// <para>In another example, for smallbore three position, one TARGET COLLECTION could include the 50m Rifle target. Another the 50m Rifle reduced for 50yds. 
    /// And yet another the 50m Rifle reduced for 50ft. </para>
    /// </summary>
    public class TargetCollection : Definition {
        
        /// <summary>
        /// Public constructor
        /// </summary>
        public TargetCollection() : base() {
            Type = DefinitionType.TARGETCOLLECTION;
        }

        [OnDeserialized]
        internal new void OnDeserializedMethod(StreamingContext context) {
            base.OnDeserializedMethod(context);

            if (TargetCollections == null)
                TargetCollections = new List<TargetCollectionModal>();
        }

        /// <summary>
        /// The list of TargetCollections for use as options. Each TargetCollections must have a its list of TargetDefs be the same length. 
        /// </summary>
		[G_STJ_SER.JsonPropertyOrder( 11 )]
        [G_NS.JsonProperty( Order = 11 )]
        public List<TargetCollectionModal> TargetCollections { get; set; } = new List<TargetCollectionModal>();

        /// <summary>
        /// Helper method to return a list of TargetCollectionNames.
        /// </summary>
        /// <returns></returns>
        public List<string> GetTargetCollectionNames() {
            List<string> list = new List<string>();
            foreach (var item in TargetCollections) {
                list.Add( item.TargetCollectionName );
            }
            return list;
        }

        /// <summary>
        /// Helper method to return the TargetCollectionModal with th e passed in target collection name.
        /// </summary>
        /// <param name="targetCollectionName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if the passed in targetCollectionName is not found.</exception>
        public TargetCollectionModal GetTargetCollection( string targetCollectionName ) {
            foreach (var item in TargetCollections) {
                if (item.TargetCollectionName == targetCollectionName) {
                    return item;
                }
            }

            var listOfNames = string.Join( ", ", this.GetTargetCollectionNames() );
            throw new InvalidOperationException( $"A TargetCollectionModal with name {targetCollectionName} was not found in {this.SetName}. The list of known TargetCollectionModals are " );
        }

        /// <summary>
        /// Returns the default target collection name.
        /// </summary>
        /// <returns></returns>
        public string GetDefaultTargetCollectionName() {
            if (TargetCollections.Count > 0)
                return TargetCollections[0].TargetCollectionName;

            return "";
        }

        /// <summary>
        /// Helper method, returns a booleann indicating if the passed in target collection name is part of this TARGET COLLECTION
        /// </summary>
        /// <param name="targetCollectionName"></param>
        /// <returns></returns>
        public bool IsValidTargetCollectionName( string targetCollectionName ) {
            foreach (var tc in TargetCollections) {
                if (tc.TargetCollectionName == targetCollectionName)
                    return true;
            }

            return false;
		}

		/// <inheritdoc />
		public override async Task<bool> GetMeetsSpecificationAsync() {
            var validation = new IsTargetCollectionValid();

			var meetsSpecification = await validation.IsSatisfiedByAsync( this );
			SpecificationMessages = validation.Messages;

			return meetsSpecification;
		}

        /// <inheritdoc />
        public override bool SetDefaultValues() {
            this.TargetCollections = new List<TargetCollectionModal>();
            this.Discipline = DisciplineType.RIFLE;
            this.Subdiscipline = "Subdiscipline";

            var defaultTcm = new TargetCollectionModal();
            defaultTcm.RangeDistance = "10m";
            defaultTcm.TargetCollectionName = "Default Collection";
            defaultTcm.TargetDefs.Add( Definitions.SetName.Parse( "v1.0:issf:10m Air Rifle" ) );
            this.TargetCollections.Add( defaultTcm );

            return true;
        }
    }
}
