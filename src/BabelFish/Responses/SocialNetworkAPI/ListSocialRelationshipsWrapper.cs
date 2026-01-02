using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.SocialNetwork;

namespace Scopos.BabelFish.Responses.SocialNetworkAPI
{
    public class ListSocialRelationshipsWrapper: BaseClass
    {
        public SocialRelationshipList SocialRelationshipList { get; set; } = new SocialRelationshipList();
    }
}