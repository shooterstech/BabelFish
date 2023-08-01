using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.Athena.Shot
{
	[Serializable]
	public class Location
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float GetRadiusSquared() {
            return X * X + Y * Y;
        }

        public float GetRadius() {
            return (float) Math.Sqrt(GetRadiusSquared());
        }
    }

}