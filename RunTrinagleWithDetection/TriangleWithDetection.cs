using System;
using System.Threading;
using OpenCvSharp;

namespace SpaceWindow
{
    public class TriangleWithDetection
    {
        TriangleWithDetectionSettings _cfg;

        public TriangleWithDetection (TriangleWithDetectionSettings cfg)
        {            
            _cfg = cfg;
        }

        public void Run ()
        {
            var pos = new ThreadSafeValue<Point3d>();
            var render = new TriangleRender(pos);
            var tracker = new FaceTracker(_cfg.FaceTrackerSettings, pos);

            tracker.StartTracking();
            render.Run();
            tracker.StopTracking();
        }
    }
}