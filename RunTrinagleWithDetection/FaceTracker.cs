using System;
using System.Threading;
using OpenCvSharp;

namespace SpaceWindow
{
    class FaceTracker
    {
        Camera2Face camera1;
        Camera2Face camera2;
        FaceTrackerSettings _cfg;
        ThreadSafeValue<OpenCvSharp.Point3d> _pos;
        Thread t;
        bool run = false;

        public FaceTracker(FaceTrackerSettings cfg, ThreadSafeValue<OpenCvSharp.Point3d> pos)
        {
            _cfg = cfg;
            _pos = pos;
            camera1 = new Camera2Face(0, showWindow: true);
            camera2 = new Camera2Face(1, showWindow: true);
        }

        public void StartTracking ()
        {
            t = new Thread(TrackingProc);
            t.IsBackground = true; //основной процесс при своем закрытии не будет ждать завершения этого потока
            t.Start();
        }

        public void StopTracking ()
        {
            run = false; //инициировали закрытие потока
            t.Join(); //ждем, пока поток не завершится
        }

        void TrackingProc()
        {
            run = true;

            //init
            var solver = new StereoSolver(_cfg.DistanceBetweenCameras, _cfg.CameraHeight, _cfg.CameraHorizontalAngle, _cfg.CameraVerticalAngle, _cfg.ImageWidth, _cfg.ImageHeight);
            int sleepTime = (int)Math.Round(1000 / 30.0);

            while (run)
            {
                //update value
                camera1.Update();
                camera2.Update();
                var pos = solver.To3D(
                    new Point(camera1.X, camera1.Y),
                    new Point(camera2.X, camera2.Y)
                );

                _pos.Value = pos;

                Thread.Sleep(sleepTime);
            }

            //done
            //...
        }
    }
}