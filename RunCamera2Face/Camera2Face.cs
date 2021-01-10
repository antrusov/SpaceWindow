using System;
using OpenCvSharp;

namespace SpaceWindow
{
    public class Camera2Face
    {
        bool _init;
        KSmoth _smothX;
        KSmoth _smothY;
        KSmoth _smothWidth;
        KSmoth _smothHeight;

        VideoCapture _capture;
        CascadeClassifier _classifier;
        Mat _src;
        Mat _dst;
        Mat _gray;
        Rect[] _faces;

        Window _window;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public Camera2Face(int cameraIndex, double a = 0.1, string haarcascadeFileName = "XML/haarcascade_frontalface_alt.xml", bool showWindow = false)
        {
            _init = false;
            _smothX = new KSmoth();
            _smothY = new KSmoth();
            _smothWidth = new KSmoth();
            _smothHeight = new KSmoth();

            _capture = new VideoCapture(cameraIndex);
            _classifier = new CascadeClassifier(haarcascadeFileName);
            _src = new Mat(_capture.FrameWidth, _capture.FrameHeight, MatType.CV_8UC4);
            _dst = new Mat(_capture.FrameWidth, _capture.FrameHeight, MatType.CV_8UC4);
            _gray = new Mat();
            if (showWindow)
                _window = new Window($"Camera: index = {cameraIndex}, a = {a}, haarFileName = {haarcascadeFileName}");

            Width = _capture.FrameWidth;
            Height = _capture.FrameHeight;
            X = -1;
            Y = -1;
        }

        public void Update()
        {
            _capture.Read(_src);

            //поиск лиц
            Cv2.CvtColor(_src, _gray, ColorConversionCodes.BGR2GRAY);
            _faces = _classifier.DetectMultiScale(_gray, 1.08, 2, HaarDetectionType.ScaleImage, new Size(30, 30));

            //инициализация
            if (!_init && _faces.Length > 0)
            {
                _init = true;
                _smothX.Reset(_faces[0].X);
                _smothY.Reset(_faces[0].Y);
                _smothWidth.Reset(_faces[0].Width);
                _smothHeight.Reset(_faces[0].Height);
            }

            if (_faces.Length > 0)
            {
                _smothX.AddNewValue(_faces[0].X);
                _smothY.AddNewValue(_faces[0].Y);
                _smothWidth.AddNewValue(_faces[0].Width);
                _smothHeight.AddNewValue(_faces[0].Height);

                var x = _smothX.Value;
                var y = _smothY.Value;
                var wd = _smothWidth.Value;
                var hg = _smothHeight.Value;

                X = (int)(x + wd * 0.5);
                Y = (int)(y + hg * 0.5);
            }

            if (_window != null)
            {
                _src.CopyTo(_dst);

                //сырые результаты
                foreach (Rect face in _faces)
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
                    Cv2.Ellipse(_dst, center, axes, 0, 0, 360, new Scalar(255, 0, 0), 1);
                }

                //найденные координаты
                {
                    var x = _smothX.Value;
                    var y = _smothY.Value;
                    var wd = _smothWidth.Value;
                    var hg = _smothHeight.Value;

                    var center = new Point
                    {
                        X = (int)(x + wd * 0.5),
                        Y = (int)(y + hg * 0.5)
                    };
                    var axes = new Size
                    {
                        Width = (int)(wd * 0.5),
                        Height = (int)(hg * 0.5)
                    };
                    //Cv2.Ellipse(dst, center, axes, 0, 0, 360, new Scalar(255, 0, 255), 4);
                    Cv2.Ellipse(_dst, center, new Size { Width = 5, Height = 5 }, 0, 0, 360, new Scalar(0, 0, 255), 3);
                }

                _window.ShowImage(_dst);
            }
       }
    }
}