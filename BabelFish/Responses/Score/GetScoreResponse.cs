using Newtonsoft.Json.Linq;

namespace BabelFish.Responses.ScoreAPI
{
    public class GetScoreResponse<T> : Response<T>
    {
        public GetScoreResponse(Helpers.ScoreStyle scoreStyle)
        {
            this.ScoreStyle = scoreStyle;
        }

        public Helpers.ScoreStyle ScoreStyle { get; private set; }

        public T Score
        {
            get { return Value; }
        }
    }
}
