using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scopos.BabelFish.DataModel.OrionMatch {
    [Serializable]
    public class Ammunition {

        public Ammunition() { }

        public Ammunition(float bulletDiameter) {
            this.ScoringDiameter = bulletDiameter;
            this.BulletDiameter = bulletDiameter;
        }

        public Ammunition(float bulletDiameter, float scoringDiameter) {
            this.ScoringDiameter = scoringDiameter;
            this.BulletDiameter = bulletDiameter;
        }

        public float ScoringDiameter { get; set; }

        public float BulletDiameter { get; set; }
    }
}
