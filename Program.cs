using System;
using OpenCvSharp;

namespace SpaceWindow
{
    class Program
    {
        static void Main(string[] args)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                Console.ReadLine();
            }
        }
    }
}
