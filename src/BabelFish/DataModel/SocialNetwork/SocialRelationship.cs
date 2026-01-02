using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Scopos.BabelFish.Converters;
namespace Scopos.BabelFish.DataModel.SocialNetwork {
    public class SocialRelationship : BaseClass{
        
        public SocialRelationship(){}
        public SocialRelationshipName RelationshipName { get; set; }
        public string ActiveId { get; set; } = "";
        public string PassiveId { get; set; } = "";
        public bool ActiveApproved {get;set;} = false;
        public bool PassiveApproved { get; set; } = false;
        public string DateCreated { get; set; } = "";

        public bool Equals(SocialRelationship other)
        {
            if (other == null) return false;
            return RelationshipName == other.RelationshipName
                && ActiveId == other.ActiveId
                && PassiveId == other.PassiveId
                //&& DateCreated == other.DateCreated
                && ActiveApproved == other.ActiveApproved
                && PassiveId == other.PassiveId;
        }

    }
}