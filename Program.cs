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
