using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.Converters;

namespace Scopos.BabelFish.DataModel.OrionMatch {
	/// <summary>
	/// A Participant is anyone who has a role in a Match. This includes athletes, teams, match officials, and coaches.
	/// 
	/// IMPORTANT: When adding Participant to a class (such as Result COF or ResultEvent (under Result List), need to make 
	/// sure to deserialize the Participant's attribute values. To do so, as an example, see GetResultCOFResponse's 
	/// PostResponseProcessingAsync()
	/// </summary>
	[Serializable]
    [G_NS.JsonConverter( typeof( G_BF_NS_CONV.ParticipantConverter ) )]
    public abstract class Participant : IDeserializableAbstractClass {

        /*
         * A description of how to describe Inherited / Abstract classes in OpenAPI 3.0 is at https://swagger.io/docs/specification/data-models/inheritance-and-polymorphism/
         */

        public Participant() {
            Coaches = new List<Individual>();
        }

        /// <summary>
        /// A unique, human readable, value assigned to all Participants in a match.
        /// 
        /// In most cases the CompetitorNumber will be numeric, but it can also be alphabetical.
        /// </summary>
        public string CompetitorNumber { get; set; } = string.Empty;


        /*
         * TODO: In some re-rentry matches a Particpant will have different AttributeValues for different re-entry stages. The CMPs 
         * garand / springfield / vintage military rifle competition is one eacmple. On the first re-entry they may shoot a garand 
         * rifle, the seocnd a sprinfield, and so on. 
         * 
         * To represent this, need a way to override AttributeValues based on the reentry tag.
         * 
         * The Medea.Participant had a .ReentryTag property. Currently choosing not to implement it.
         */

        /// <summary>
        /// A list of AttributeValues assigned to this Participant.
        /// </summary>
        public List<AttributeValueDataPacketMatch> AttributeValues { get; set; } = new List<AttributeValueDataPacketMatch>();

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize AttributeValues when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeAttributeValues() {
            return (AttributeValues != null && AttributeValues.Count > 0);
        }

        /*
         * TODO: In some re-rentry matches a Particpant will have different AttributeValues for different re-entry stages. The CMPs 
         * garand / springfield / vintage military rifle competition is one eacmple. On the first re-entry they may shoot a garand 
         * rifle, the seocnd a sprinfield, and so on. 
         * 
         * To represent this, need a way to override AttributeValues based on the reentry tag.
         */


        /// <summary>
        /// A list of Remark objects, each containing a RemarkName, sometimes a reason, and a status (show or don't)
        /// </summary>
        public RemarkList RemarkList { get; set; } = new RemarkList();

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize AttributeValues when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeRemarkList() {
            return (RemarkList != null && RemarkList.Count() > 0);
        }

        /// <summary>
        /// A list of this Participant's coaches.
        /// </summary>
        public List<Individual> Coaches { get; set; }

        /// <summary>
        /// A Newtonsoft Conditional Property to only serialize Coaches when the list has something in it.
        /// https://www.newtonsoft.com/json/help/html/ConditionalProperties.htm
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeCoaches() {
            return (Coaches != null && Coaches.Count > 0);
        }


        /// <summary>
        /// When a competitor's name is displayed, this is the default display value.
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// The three letter country code the participant is from.
        /// </summary>
        public string Country { get; set; } = string.Empty;

        /// <summary>
        /// The Hometown the participant is from.
        /// </summary>
        public string HomeTown { get; set; } = string.Empty;

        /// <summary>
        /// When a competitor's name is displayed, and there is limited number of characters, use this value. 
        /// 
        /// There is no rule as to how long the Short value could be, but by convention 12 characters or less.
        /// </summary>
        public string DisplayNameShort { get; set; } = string.Empty;

        //TODO: Club, ReentryTag not in API return data
        /// <summary>
        /// The Hometown Club the Participant represents. Note, this is NOT the same as any team the Participant is shooting with. 
        /// </summary>
        public string Club { get; set; } = string.Empty;

        public override string ToString() {
            return this.DisplayName;
        }

        /// <summary>
        /// Implementation of the IDeserializableAbstractClass interface.
        /// To have added control over the Deserialization of abstract classes, in to
        /// Concrete classes, the JSON should include a ConcreteClassId that specifies
        /// the Concrete class.
        /// </summary>
        public int ConcreteClassId { get; set; }

        public abstract int UniqueMergeId { get; }
    }
}
