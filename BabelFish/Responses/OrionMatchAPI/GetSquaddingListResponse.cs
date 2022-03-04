using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BabelFish.DataModel.OrionMatch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BabelFish.Responses.OrionMatchAPI
{
    public class GetSquaddingListResponse : Response<Squadding>
    {
        /// <summary>
        /// Facade function that returns the same as this.Value
        /// </summary>
        public Squadding Squadding
        {
            get { return Value; }
        }

        protected override void ConvertBodyToValue()
        {
            try
            {
                // BUILDING FIRINGPOINT OBJECT
                // THINK THE ANSWER IS BUILDING THIS OBJECT BUT IN THE 2 FOR LOOPS BELOW
                //List<SquaddingAssignmentFiringPoint> newSA = new List<SquaddingAssignmentFiringPoint>();
                //var squaddingObject = Body.Values<Squadding>().FirstOrDefault();
                //foreach (SquaddingAssignment sa in squaddingObject.SquaddingList)
                //{
                //    switch (sa)
                //    {
                //        case SquaddingAssignment:
                //            newSA.Add((SquaddingAssignmentFiringPoint)sa);
                //            break;
                //            //Add the other types
                //    }
                //}


                // CONVERT EACH SQUADDINGASSIGNMENT INTO FIRINGPOINT
                //  SEE IF THIS CAN BE DONE ON JToken OR NEED TO CREATE NEW OBJECT? (ABOVE)
                //var newBody = Body.Children<JObject>();

                //foreach (Squadding loopBody in Body.Values<Squadding>())
                //{
                //    foreach (SquaddingAssignment sa in loopBody.SquaddingList)
                //    {
                //        switch (sa)
                //        {
                //            case SquaddingAssignment:
                //                Body[loopBody][sa].ToObject<SquaddingAssignmentFiringPoint>();
                //                break;
                //            //Add the other types
                //        }
                //    }
                //}


                // TEST: PLUGGING INTO NEWTONSOFT PROCESS
                //var checkValues = Body.Values<Squadding>();
                //var checkSquadding = checkValues.FirstOrDefault();
                //string jsonTypeNameAll = JsonConvert.SerializeObject(checkSquadding, Formatting.Indented, new JsonSerializerSettings
                //{ TypeNameHandling = TypeNameHandling.All });
                //string jsonTypeNameAuto = JsonConvert.SerializeObject(
                //    checkSquadding, Formatting.Indented, new JsonSerializerSettings
                //    { TypeNameHandling = TypeNameHandling.Auto });
                //Value = JsonConvert.DeserializeObject<Squadding>(jsonTypeNameAuto, new JsonSerializerSettings
                //{ TypeNameHandling = TypeNameHandling.Auto });


                //THIS IS FAILING ON ABSTRACT CLASS SquaddingAssignment
                // BUILD ABOVE AND ASSIGN Value
                //Value = Body.ToObject<Squadding>();

            }
            finally { }
        }
    }
}
