using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
            MenuInterface = new MenuInterface();
        }

        [OnDeserialized]
        internal void OnDeserialized(StreamingContext context)
        {

            //Providate default values if they were not read during deserialization

            if (ExternalErrors == null)
                ExternalErrors = new List<string>();
        }

        public Target Target { get; set; }

        public ShotDetection ShotDetection { get; set; }

        public TargetLight TargetLight { get; set; }

        public RedLight RedLight { get; set; }

        public GreenLight GreenLight { get; set; }

        public TargetLift TargetLift { get; set; }

        public FrameHitDetector FrameHitDetector { get; set; }

        public TapeFeed TapeFeed { get; set; }

        public ValidationPhotos ValidationPhotos { get; set; }

        public LiveView LiveView { get; set; }

        public CameraCalibration CameraCalibration { get; set; }

        public MenuInterface MenuInterface { get; set; }

    }
}