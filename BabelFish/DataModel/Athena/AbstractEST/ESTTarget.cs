using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Scopos.BabelFish.DataModel.Athena.AbstractEST
{
    public class ESTTarget : AbstractEST
    {

        public ESTTarget()
        {

            TargetLight = new TargetLight();
            RedLight = new RedLight();
            GreenLight = new GreenLight();
            TargetLift = new TargetLift();
            FrameHitDetector = new FrameHitDetector();
            TapeFeed = new TapeFeed();
            ValidationPhotos = new ValidationPhotos();
            Target = new Target();
            LiveView = new LiveView();
            CameraCalibration = new CameraCalibration();
        }

        [JsonProperty(Order = 1)]
        public Target Target { get; set; }

        [JsonProperty(Order = 8)]
        public ShotDetection ShotDetection { get; set; }

        [JsonProperty(Order = 3)]
        public TargetLight TargetLight { get; set; }


        [JsonProperty(Order = 4)]
        public RedLight RedLight { get; set; }


        [JsonProperty(Order = 5)]
        public GreenLight GreenLight { get; set; }


        [JsonProperty(Order = 6)]
        public TargetLift TargetLift { get; set; }


        [JsonProperty(Order = 7)]
        public FrameHitDetector FrameHitDetector { get; set; }

        [JsonProperty(Order = 8)]
        public TapeFeed TapeFeed { get; set; }


        [JsonProperty(Order = 9)]
        public ValidationPhotos ValidationPhotos { get; set; }

        [JsonProperty(Order = 10)]
        public LiveView LiveView { get; set; }


        [JsonProperty(Order = 11)]
        public CameraCalibration CameraCalibration { get; set; }

    }
}