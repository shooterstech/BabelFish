using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Scopos.BabelFish.Converters;
namespace Scopos.BabelFish.DataModel.SocialNetwork {
    public class SocialRelationship : BaseClass{
        
        public SocialRelationship(){}
        public string RelationshipName { get; set; } = "";
        public string ActiveId { get; set; } = "";
        public string PassiveId { get; set; } = "";
        public bool ActiveApproved {get;set;} = false;
        public bool PassiveApproved { get; set; } = false;
        public string DateCreated { get; set; } = "";

    }
}