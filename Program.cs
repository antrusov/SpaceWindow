using System;
using OpenCvSharp;

namespace SpaceWindow
{
    class Program
    {
        static void RunCamera2Face()
        {
            var camera1 = new Camera2Face(0, showWindow: true);
            var camera2 = new Camera2Face(1, showWindow: true);
            int sleepTime = (int)Math.Round(1000 / 30.0);

            while (true)
            {
                camera1.Update();
                camera2.Update();

                //выход
                if (Cv2.WaitKey(sleepTime) != -1)
                    break;
            } 
        }

        static void RunColorDetection ()
        {
            var colorLow1 = new Scalar(0, 120, 70);
            var colorUp1 = new Scalar(10, 255, 255);

            var colorLow2 = new Scalar(170, 120, 70);
            var colorUp2 = new Scalar(180, 255, 255);

            var minArea = 1000;

            var camera1 = new Camera2Color
            (
                0,
                colorLow1,
                colorUp1,
                colorLow2,
                colorUp2,
                minArea,
                mode: Camera2Color.ShowMode.CameraAndMask
            );
            int sleepTime = (int)Math.Round(1000 / 30.0);

            while (true)
            {
                camera1.Update();

                //выход
                if (Cv2.WaitKey(sleepTime) != -1)
                    break;
            } 
        }

        static void Main(string[] args)
        {
            try
            {
                //RunCamera2Face();
                RunColorDetection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                Console.ReadLine();
            }
        }
    }
}
