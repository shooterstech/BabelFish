using System;
using System.Collections.Generic;
using System.Text;
using Scopos.BabelFish.DataModel.OrionMatch;
using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.SocialNetwork;

namespace Scopos.BabelFish.Responses.SocialNetworkAPI
{

    /// <summary>
    /// Helper class that creates the added structure in the data model needed for Deserialzing a Match object from json.
    /// </summary>
    public class RelationshipRoleWrapper : BaseClass
    {

        public SocialRelationship SocialRelationship { get; set; } = new SocialRelationship();

    }
}
