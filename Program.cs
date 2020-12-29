using System;
using System.IO;
using OpenCvSharp;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

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
            var notifier = new Notifier(cfg.BaseAddress, cfg.Query);
            var scaleX = new Scale(0, camera1.Width, cfg.Area.X1, cfg.Area.X2);
            var scaleY = new Scale(0, camera1.Height, cfg.Area.Y1, cfg.Area.Y2);

            var oldx = 0.0;
            var oldy = 0.0;

            while (true)
            {
                //получить координаты красного объекта
                camera1.Update();

                //подготовить данные для отправки
                var x = scaleX.Get(camera1.X);
                var y = scaleY.Get(camera1.Y);
                var vx = (x - oldx) * cfg.VelocityCoefficient;
                var vy = (y - oldy) * cfg.VelocityCoefficient;
                oldx = x;
                oldy = y;

                //отправить информацию на сервер
                notifier.Update(x, y, vx, vy).GetAwaiter().GetResult();

                //интервал между отправкой данных + выход если нажата клавиша
                if (Cv2.WaitKey(cfg.Period) != -1)
                    break;
            } 
        }

        static void RunColorConfig ()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var cfg = configuration.GetSection("GloveSettings").Get<GloveSettings>();

            var src = new Mat("img1.png");
            var dst = new Mat();
            var hsv = new Mat();
            var mask = new Mat();
            var mask1 = new Mat();
            var mask2 = new Mat();

            var colorLow1 = new Scalar(cfg.ColorRanges.Lower1[0], cfg.ColorRanges.Lower1[1], cfg.ColorRanges.Lower1[2]);
            var colorUp1 = new Scalar(cfg.ColorRanges.Upper1[0], cfg.ColorRanges.Upper1[1], cfg.ColorRanges.Upper1[2]);
            var colorLow2 = new Scalar(cfg.ColorRanges.Lower2[0], cfg.ColorRanges.Lower2[1], cfg.ColorRanges.Lower2[2]);
            var colorUp2 = new Scalar(cfg.ColorRanges.Upper2[0], cfg.ColorRanges.Upper2[1], cfg.ColorRanges.Upper2[2]);

            Cv2.CvtColor(src, hsv, ColorConversionCodes.BGR2HSV);
            Cv2.InRange(hsv, colorLow1, colorUp1, mask1); //lower red
            Cv2.InRange(hsv,colorLow2, colorUp2, mask2); //upper red
            mask = mask1 + mask2;

            dst.SetTo(new Scalar(0,0,0));
            Cv2.BitwiseAnd(src, src, dst, mask);

            using (new Window("src image", src)) 
            using (new Window("dst image", dst)) 
            {
                Cv2.WaitKey();
            }
        }

        static void Main(string[] args)
        {
            try
            {
                //RunCamera2Face();
                //RunColorDetection();
                RunColorConfig();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                Console.ReadLine();
            }
        }
    }
}
