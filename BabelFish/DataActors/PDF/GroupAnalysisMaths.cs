using Scopos.BabelFish.DataModel.Athena.Shot;
using Scopos.BabelFish.DataModel.Definitions;
using Scopos.BabelFish.DataModel.OrionMatch;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scopos.BabelFish.DataActors.PDF
{
    public class GroupAnalysisMaths
    {
        private List<Shot> shots = new List<Shot>();
        private List<Shot> shotsToAnalyize = new List<Shot>();

        private double xCenter = 0;
        private double yCenter = 0;
        private double rCenter = 0;

        private double majorAxis = 0;
        private double minorAxis = 0;

        private double angle = 0;

        private double distanceBetweenWidestShots = 0;

        public GroupAnalysisMaths(List<Shot> shots)
        {
            this.shots = shots;
            Calculate();
        }

        private void Calculate()
        {
            foreach (var shot in this.shots)
            {
                if (!shot.Attributes.Contains(Shot.SHOT_ATTRIBUTE_MISSED_SHOT)
                    && !shot.Attributes.Contains(Shot.SHOT_ATTRIBUTE_UNKNOWN_COORDINATES)
                    && !shot.Attributes.Contains(Shot.SHOT_ATTRIBUTE_EMPTY)
                    && !shot.Attributes.Contains(Shot.SHOT_ATTRIBUTE_FRAME_HIT))
                    shotsToAnalyize.Add(shot);
            }

            if (shotsToAnalyize.Count == 0)
                return;

            //Calculate the average X and Y location
            int count = 0;
            double averageR = 0;
            foreach (Shot s in shotsToAnalyize)
            {
                count++;
                xCenter += s.Location.X;
                yCenter += s.Location.Y;
                averageR += s.Location.GetRadius();
            }
            xCenter /= count;
            yCenter /= count;
            averageR /= count;
            rCenter = Math.Sqrt(xCenter * xCenter + yCenter * yCenter);

            //Calculate the variance of the radius
            double varX = 0, varY = 0, coVar = 0;
            foreach (Shot s in shotsToAnalyize)
            {
                varX += (s.Location.X - xCenter) * (s.Location.X - xCenter);
                varY += (s.Location.Y - yCenter) * (s.Location.Y - yCenter);
                //varR += (s.GetRadius() - averageR) * (s.GetRadius() - averageR);
                coVar += (s.Location.X - xCenter) * (s.Location.Y - yCenter);
            }
            varX /= count;
            varY /= count;
            //varR /= count;
            coVar /= count;

            //Calculate the angle of the group
            angle = Math.Atan(coVar / varX);
            var angleDegress = angle * 180.0 / Math.PI;

            //Transfrom the center of the group to the origin, and rotate the major axis of the group to the X axis
            double shotAngle, newX, newY, newR;
            varX = 0;
            varY = 0;
            foreach (Shot s in shotsToAnalyize)
            {
                shotAngle = Math.Atan((s.Location.Y - yCenter) / (s.Location.X - xCenter)) + (((s.Location.X - xCenter) < 0) ? Math.PI : 0);
                newR = Math.Sqrt((s.Location.Y - yCenter) * (s.Location.Y - yCenter) + (s.Location.X - xCenter) * (s.Location.X - xCenter));
                newX = Math.Cos(shotAngle - angle) * newR;
                newY = Math.Sin(shotAngle - angle) * newR;
                varX += newX * newX;
                varY += newY * newY;
            }
            varX /= count;
            varY /= count;

            var foo = shotsToAnalyize[0].EventName;

            //1.645 is the norminv(.95)
            majorAxis = Math.Sqrt(varX) * 1.645;
            minorAxis = Math.Sqrt(varY) * 1.645;

            widestGroup();

        }

        private void widestGroup()
        {
            //get the widest shot
            Scopos.BabelFish.DataModel.Athena.Shot.Shot widestShot = null;
            double maxDistance = 0;
            foreach (var shot1 in shotsToAnalyize)
            {
                if (shot1.Location.GetRadiusSquared() > maxDistance)
                {
                    maxDistance = shot1.Location.GetRadiusSquared();
                    widestShot = shot1;
                }
            }
            //Now find furthest shot from the widest one.
            maxDistance = 0;
            foreach (var shot1 in shotsToAnalyize)
            {
                var x1 = widestShot.Location.X;
                var y1 = widestShot.Location.Y;
                var x2 = shot1.Location.X;
                var y2 = shot1.Location.Y;
                var dist = Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1)));
                if(dist > maxDistance)
                {
                    maxDistance = dist;
                }
            }
            distanceBetweenWidestShots = maxDistance;
        }

        /// <summary>
        /// x coord of the group center, center target is (0,0)
        /// </summary>
        /// <returns></returns>
        public double GetCenterX()
        {
            return xCenter;
        }

        /// <summary>
        /// y coord of the group center, center target is (0,0)
        /// </summary>
        /// <returns></returns>
        public double GetCenterY()
        {
            return yCenter;
        }

        /// <summary>
        /// Radial distance of group center from center target (0,0)
        /// </summary>
        /// <returns></returns>
        public double GetRadialDistance()
        {
            return rCenter;
        }

        /// <summary>
        /// Major (X) axis radial distance from center group
        /// </summary>
        /// <returns></returns>
        public double GetMajorAxis()
        {
            return majorAxis;
        }

        /// <summary>
        /// Minor (Y) axis radial distance from center group
        /// </summary>
        /// <returns></returns>
        public double GetMinorAxis()
        {
            return minorAxis;
        }

        /// <summary>
        /// Angle of group from zero CCW
        /// </summary>
        /// <returns></returns>
        public double GetAngle()
        {
            return angle;
        }

        /// <summary>
        /// Area of group, PI*major*minor
        /// </summary>
        /// <returns></returns>
        public double GetArea()
        {
            return Math.PI * majorAxis * minorAxis;
        }

        /// <summary>
        /// major / minor, clore to 1 is better
        /// </summary>
        /// <returns></returns>
        public double GetRoundness()
        {
            if (minorAxis == 0D)
                return 1D;
            else
                return majorAxis / minorAxis;
        }

        public double GetDistanceBetweenWidestShots()
        {
            return distanceBetweenWidestShots;
        }
    }
}
