using System;
using OpenCvSharp;

namespace SpaceWindow
{
    public class Camera2Color
    {        
        Scalar _colorLow;
        Scalar _colorUp;
        VideoCapture _capture;
        Mat _src;
        Mat _hsv;
        Mat _mask;
        Mat _dst;
        Window _window;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public Camera2Color(int cameraIndex, Scalar colorLow, Scalar colorUp, bool showWindow = false)
        {
            _colorLow = colorLow;
            _colorUp = colorUp;

            _capture = new VideoCapture(cameraIndex);

            if (showWindow)
                _window = new Window($"Camera: index = {cameraIndex}, colorLow = {colorLow}, colorUp = {colorUp}");

            _src = new Mat(_capture.FrameWidth, _capture.FrameHeight, MatType.CV_8UC4);
            _dst = new Mat(_capture.FrameWidth, _capture.FrameHeight, MatType.CV_8UC4);
            _hsv = new Mat();
            _mask = new Mat();

            Width = _capture.FrameWidth;
            Height = _capture.FrameHeight;
            X = -1;
            Y = -1;
        }

        public void Update()
        {
            _capture.Read(_src);
            Cv2.CvtColor(_src, _hsv, ColorConversionCodes.BGR2HSV);
            Cv2.InRange(_hsv,_colorLow, _colorUp, _mask);
            //Cv2.BitwiseAnd(_src, _src, _dst, _mask);

            var errodeElement = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(5,5));
            Cv2.Erode(_mask, _mask, errodeElement);
            Cv2.Dilate(_mask, _mask, errodeElement);
            Cv2.Dilate(_mask, _mask, errodeElement);
            Cv2.Erode(_mask, _mask, errodeElement);

            var moments = Cv2.Moments(_mask, binaryImage: true);
            var m01 = moments.M01;
            var m10 = moments.M10;
            var area = moments.M00;

            if (area > 1000)
            {
                X = (int)(m10 / area);
                Y = (int)(m01 / area);
            }

            if (_window != null)
            {
                var center = new Point(X,Y);
                var axes = new Size { Width = 5, Height = 5 };
                Cv2.Ellipse(_src, center, axes, 0, 0, 360, new Scalar(255, 0, 0), 1);

                _window.ShowImage(_src);
                //_window.ShowImage(_dst);
                //_window.ShowImage(_mask);
            }
        }
    }
}