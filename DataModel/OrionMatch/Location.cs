using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class Location {

        //TODO: rm for not matching up to Postman API - City,State,Country not included - 20220202(AKS) 
        //public string City { get; set; }
        //public string State { get; set; }
        //public string Country { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }
    }
}
