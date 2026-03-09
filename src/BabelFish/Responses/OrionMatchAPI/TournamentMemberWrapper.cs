using Scopos.BabelFish.DataModel;
using Scopos.BabelFish.DataModel.OrionMatch;

namespace Scopos.BabelFish.Responses.OrionMatchAPI {
    public class TournamentMemberWrapper : BaseClass {

        public TournamentMember TournamentMember { get; set; } = new TournamentMember();
    }
}
