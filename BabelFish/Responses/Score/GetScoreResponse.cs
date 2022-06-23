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

        /// <inheritdoc/>
        /// trying to no manually parse but haven't found a way yet
        ////protected override void ConvertBodyToValue()
        ////{
        ////    NOPE CAN'T USE, MAYBE CHECK INTO Value?? var responseArgument = BabelFish.Helpers.EnumHelper.Description(ScoreStyle);
        ////    GetScoreResponse<T> newResponse = new GetScoreResponse<T>(this.ScoreStyle);

        ////    try
        ////    {
        ////        JObject o = JObject.Parse(Body.ToString());
        ////        string otype = o.Type.ToString();
        ////        foreach (JProperty property in o.Properties())
        ////        {
        ////            if (property.Name == "ScoreHistory")
        ////            {
        ////                bool foundHistory = true;
        ////            }
        ////            if (property.Name == "ScoreAverage")
        ////            {
        ////                bool foundAverage = true;
        ////            }
        ////            if (property.Name == "Arguments")
        ////            {
        ////                //declear T argument, loop and populate
        ////                //= Body["Arguments"].ToObject<T>();
        ////                //Value = Body["Arguments"].ToObject<T>();
        ////                //Body.ToObject<responseArgument>();
        ////                bool parseArgumentObject = true;

        ////                var argument = newResponse.Score;
        ////                JObject o1 = JObject.Parse(property.Value.ToString());
        ////                foreach (JProperty property1 in o1.Properties())
        ////                {
        ////                    //assign property Property1.Name->property1.Value
        ////                }
        ////            }
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        string err = ex.Message;
        ////    }

        ////    //            Value = Body[SetName.ToString()].ToObject<T>();
        ////}
    }
}
