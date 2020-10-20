using System;
using OpenCvSharp;

namespace SpaceWindow
{
    class Program
    {
        static void Main(string[] args)
        {
            using var capture = new VideoCapture(0);

            int sleepTime = (int)Math.Round(1000 / capture.Fps);

            using (var window = new Window("capture"))
            {
                // Frame image buffer
                var image = new Mat();

                // When the movie playback reaches end, Mat.data becomes NULL.
                while (true)
                {
                    capture.Read(image); // same as cvQueryFrame
                    if(image.Empty())
                        break;

                    window.ShowImage(image);
                    
                    if (Cv2.WaitKey(sleepTime) != -1)
                        break;
                } 
            }
        }
    }
}
