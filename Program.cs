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
                using var capture = new VideoCapture(0);
                using var src = new Mat(capture.FrameWidth, capture.FrameHeight, MatType.CV_8UC4);
                using var dst = new Mat(capture.FrameWidth, capture.FrameHeight, MatType.CV_8UC4);
                using var gray = new Mat();
                using var classifier = new CascadeClassifier("haarcascade_frontalface_alt.xml");

                int sleepTime = (int)Math.Round(1000 / capture.Fps);
                Rect[] faces;

                using (var window = new Window("capture"))
                {
                    while (true)
                    {
                        capture.Read(src);

                        //поиск лиц
                        Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);
                        faces = classifier.DetectMultiScale(gray, 1.08, 2, HaarDetectionType.ScaleImage, new Size(30, 30));

                        //вывод результатов
                        src.CopyTo(dst);
                        foreach (Rect face in faces)
                        {
                            var center = new Point
                            {
                                X = (int)(face.X + face.Width * 0.5),
                                Y = (int)(face.Y + face.Height * 0.5)
                            };
                            var axes = new Size
                            {
                                Width = (int)(face.Width * 0.5),
                                Height = (int)(face.Height * 0.5)
                            };
                            Cv2.Ellipse(dst, center, axes, 0, 0, 360, new Scalar(255, 0, 255), 4);
                        }
                        window.ShowImage(dst);

                        //выход
                        if (Cv2.WaitKey(sleepTime) != -1)
                            break;
                    } 
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
