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
        //отслеживание координат лица
        static void RunCamera2Face(IConfiguration configuration)
        {
            var cfg = configuration.GetSection("FaceTrackerSettings").Get<FaceTrackerSettings>();
            
            var camera1 = new Camera2Face(0, showWindow: true);
            var camera2 = new Camera2Face(1, showWindow: true);
            var solver = new StereoSolver(cfg.DistanceBetweenCameras, cfg.CameraHeight, cfg.CameraHorizontalAngle, cfg.CameraVerticalAngle, cfg.ImageWidth, cfg.ImageHeight);
            int sleepTime = (int)Math.Round(1000 / 30.0);

            while (true)
            {
                camera1.Update();
                camera2.Update();
                var pos = solver.To3D(
                    new Point(camera1.X, camera1.Y),
                    new Point(camera2.X, camera2.Y)
                );

                Console.WriteLine($"{pos.X:0.###}\t{pos.Y:0.###}\t{pos.Z:0.###}");

                //выход
                if (Cv2.WaitKey(sleepTime) != -1)
                    break;
            } 
        }

        //отслеживание координат цветового пятна
        static void RunColorDetection (IConfiguration configuration)
        {
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

        //тестовый прогон фильтрации цветового пятна по картинке
        static void RunColorConfig (IConfiguration configuration)
        {
            var cfg = configuration.GetSection("GloveSettings").Get<GloveSettings>();

            var src = new Mat("RunColorConfig/img1.png");
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

        //интерактивная фильтрация цветового пятна (с ползунками)
        static void RunColorFilter (IConfiguration configuration)
        {
            var cfg = configuration.GetSection("FaceTrackerSettings").Get<FaceTrackerSettings>();

            //исходное окно
            //окна: каналами + ползунки + обрезанный канал
            //результирующее окно

            //int h = 0;
            //Cv2.CreateTrackbar("H", "src image", ref h, 255);
        }

        //отображение кубической карты (с учетом положения камеры)
        static void RunCubemap(IConfiguration configuration)
        {
            var cfg = configuration.GetSection("CubemapSettings").Get<CubemapSettings>();

            var box = new Skybox();
        }

        //эксперименты с OpenGL (отображение треугольника)
        static void RunTriangle(IConfiguration configuration)
        {
            var cfg = configuration.GetSection("TriangleSettings").Get<TriangleSettings>();

            using var window = new TrinangleWindow(cfg.Width, cfg.Height, cfg.Title);
            window.Run();
        }

        static void Main(string[] args)
        {
            try
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();

                //RunCamera2Face(configuration);
                //RunColorDetection(configuration);
                //RunColorConfig(configuration);
                RunColorFilter(configuration);
                //RunCubemap(configuration);
                //RunTriangle(configuration);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex.StackTrace);
                Console.ReadLine();
            }
        }
    }
}
