using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShootersTech.DataModel.OrionMatch {
    /// <summary>
    /// An OrionTarget is a brief definition of a type of target. It contains enough information for a handheld 
    /// device to take an image of the target. For a complete definition use the TargetScheme with the 
    /// GetTargetDefinition() call from IDefinitionAPI.
    /// </summary>
    [Serializable]
    public class OrionTarget {

        public OrionTarget() { }

        public OrionTarget(int targetScheme, string name, int widthBulls, int heightBulls, float relativeDiameter) {
            this.TargetScheme = targetScheme;
            this.Name = name;
            this.NumBullsWidth = widthBulls;
            this.NumBullsHeight = heightBulls;
            this.AimingBullRelativeDiameter = relativeDiameter;
        }

        /// <summary>
        /// The unique integer value describing the target. 
        /// </summary>
        public int TargetScheme { get; set; }

        /// <summary>
        /// Human readable name for the target. For example, "10m Air Rifle"
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// When photograghing the target, the number of columns of competition Aiming Bulls to draw on screen
        /// </summary>
        public int NumBullsWidth { get; set; }

        /// <summary>
        /// When photograghing the target, the number of rows of competition Aiming Bulls to draw on screen
        /// </summary>
        public int NumBullsHeight { get; set; }

        /// <summary>
        /// When photograghing the target, the relative size of the aiming bull to draw on screen.
        /// The value is a percentage of the height of the view screen.
        /// </summary>
        public float AimingBullRelativeDiameter { get; set; }

        public static OrionTarget TargetFactory(int targetScheme) {
            switch (targetScheme) {
                case 0:
                    return new OrionTarget(0, "Generic Target", 1, 1, .18f);
                case 56:
                    return new OrionTarget(56, "50m Rifle 6 Bull", 2, 3, .18f);
                case 57:
                    return new OrionTarget(57, "50yd. Rifle 6 Bull", 2, 3, .18f);
                case 58:
                    return new OrionTarget(58, "50yd. Conventional Rifle 6 Bull", 2, 3, .15f);
                case 59:
                    return new OrionTarget(59, "50m Conventional Rifle 6 Bull", 2, 3, .15f);
                case 60:
                    return new OrionTarget(60, "50m Conventional Rifle reduced for 50yd. 6 Bull", 2, 3, .15f);
                case 61:
                    return new OrionTarget(61, "300m Rifle reduced for 100yd. 3 Bull", 1, 2, .26f);
                case 62:
                    return new OrionTarget(62, "100yd. Conventional Rifle 3 Bull", 1, 2, .26f);
                case 75:
                    return new OrionTarget(75, "25yd. and 50yd. Rimfire Sporter", 1, 1, .25f);
                case 77:
                    return new OrionTarget(77, "5m BB Gun", 4, 3, .1f);
                default:
                    throw new NotSupportedException("Unknown target scheme " + targetScheme);
            }
        }
    }
}
