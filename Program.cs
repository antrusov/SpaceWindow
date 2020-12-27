using System;
using System.IO;
using OpenCvSharp;
using Microsoft.Extensions.Configuration;

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
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var cfg = configuration.GetSection("GloveSettings").Get<GloveSettings>();

            var camera1 = new Camera2Color
            (
                cfg.CameraIndex,
                cfg.ColorRanges.Lower1,
                cfg.ColorRanges.Upper1,
                cfg.ColorRanges.Lower2,
                cfg.ColorRanges.Upper2,
                cfg.MinArea,
                cfg.CameraMode
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
