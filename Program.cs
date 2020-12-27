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
            var colorLow = new Scalar(169,100,100);
            var colorUp = new Scalar(189,255,255);

            //var colorLow = new Scalar(0,50,120);
            //var colorUp = new Scalar(10,255,255);

            var camera1 = new Camera2Color(0, colorLow, colorUp, showWindow: true);
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
