using System.Text.Json;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Scopos.BabelFish.DataModel.Definitions {


    /// <summary>
    /// An abstract class that helps define both an aiming mark (what an athlete aims at) or a scoring ring
    /// </summary>
    [Serializable]
    public abstract class ScoringShapeDimension: IReconfigurableRulebookObject {

        /// <summary>
        /// Public constructor
        /// </summary>
        public ScoringShapeDimension() { }

        /// <summary>
        /// THe shape of this ScoringShape.
        /// </summary>
        
        [DefaultValue( ScoringShape.CIRCLE )]
        public ScoringShape Shape { get; set; } = ScoringShape.CIRCLE;

        /// <summary>
        /// The printed size of this scoring shape.
        /// <para>Printed size means how big this scoirng shape is when printed on paper. It is not the size used for scoirng (as that must include the size of the bullet).</para>
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>If the Shape is a Circle -> Dimension is diameter, measured in mm.</item>
        /// <item>If the Shape is a Square -> Dimension is length of a side, measured in mm.</item>
        /// </list>
        /// </remarks>
        public float Dimension { get; set; } = 0;

        /// <summary>
        /// Returns the radius, that that the center of a bullet must land within
        /// to hit this scoring ring. Measured in mm.
        /// </summary>
        /// <param name="bulletScoringDiameter">The diameber of the bullet to use for scoirng. Measured in mm.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Thrown if .Shape is not a CIRCLE.</exception>
        public float GetScoringRadius( float bulletScoringDiameter ) {
            if (Shape != ScoringShape.CIRCLE)
                throw new NotImplementedException( $"GetScoringRadius not yet implemented for shape {Shape}." );

            return (Dimension + bulletScoringDiameter) / 2.0f;
        }
        /// <summary>
        /// Returns a boolean indicating if a bullet centered at (x, y), with scoring diameter, if that shot
        /// touches this ScoringShape.
        /// </summary>
        /// <param name="x">X coordinate of the center of the shot, measured in mm.</param>
        /// <param name="y">Y coordinate of the center of the shot, measured in mm.</param>
        /// <param name="scoringDiameter">Scoring diameter of the shot, measuredi in mm.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Thrown if .Shape is not a CIRCLE.</exception>
        public bool HitsScoringShape( float x, float y, float scoringDiameter ) {
            if (Shape != ScoringShape.CIRCLE)
                throw new NotImplementedException( $"HitScoringShape not yet implemented for shape {Shape}.");

            float r = (float) Math.Sqrt( x * x + y * y );
            
            return ( r <= this.GetScoringRadius( scoringDiameter ) );
        }


        /// <inheritdoc/>
        [JsonPropertyOrder ( 99 )]
        [DefaultValue( "" )]
        public string Comment { get; set; } = string.Empty;
    }
}
