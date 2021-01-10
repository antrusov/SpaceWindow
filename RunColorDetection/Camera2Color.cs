using System;
using OpenCvSharp;

namespace SpaceWindow
{
    public class Camera2Color
    {
        public enum ShowMode
        {
            None = 0,
            Camera = 1,
            Mask = 2,
            CameraAndMask = 3,
        }

        ShowMode _mode;
        Scalar _colorLow1;
        Scalar _colorUp1;
        Scalar _colorLow2;
        Scalar _colorUp2;
        double _minArea;

        VideoCapture _capture;
        Mat _src;
        Mat _hsv;
        Mat _mask1;
        Mat _mask2;
        Mat _mask;
        Mat _dst;
        Window _window;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public Camera2Color(
            int cameraIndex,
            int[] colorLow1,
            int[] colorUp1,
            int[] colorLow2,
            int[] colorUp2,
            double minArea = 1000,
            ShowMode mode = ShowMode.None
        )
        {
            _mode = mode;
            _colorLow1 = new Scalar(colorLow1[0], colorLow1[1], colorLow1[2]);
            _colorUp1 = new Scalar(colorUp1[0], colorUp1[1], colorUp1[2]);
            _colorLow2 = new Scalar(colorLow2[0], colorLow2[1], colorLow2[2]);
            _colorUp2 = new Scalar(colorUp2[0], colorUp2[1], colorUp2[2]);
            _minArea = minArea;

            _capture = new VideoCapture(cameraIndex);

            if (_mode != ShowMode.None)
                _window = new Window($"Camera: index = {cameraIndex}, mode = {mode}");

            _src = new Mat(_capture.FrameWidth, _capture.FrameHeight, MatType.CV_8UC4);
            _dst = new Mat(_capture.FrameWidth, _capture.FrameHeight, MatType.CV_8UC4);
            _hsv = new Mat();
            _mask1 = new Mat();
            _mask2 = new Mat();
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
            Cv2.InRange(_hsv,_colorLow1, _colorUp1, _mask1); //lower red
            Cv2.InRange(_hsv,_colorLow2, _colorUp2, _mask2); //upper red
            _mask = _mask1 + _mask2;

            switch(_mode)
            {
                case ShowMode.Camera:
                    _dst = _src;
                    break;
                case ShowMode.Mask:
                    _dst = _mask;
                    break;
                case ShowMode.CameraAndMask:
                    _dst.SetTo(new Scalar(0,0,0));
                    Cv2.BitwiseAnd(_src, _src, _dst, _mask);
                    break;
                default:
                    break;
            }

            var errodeElement = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(10,10));
            Cv2.Erode(_mask, _mask, errodeElement);
            //Cv2.Dilate(_mask, _mask, errodeElement);

            var moments = Cv2.Moments(_mask, binaryImage: true);
            var m01 = moments.M01;
            var m10 = moments.M10;
            var area = moments.M00;

            if (area > _minArea)
            {
                X = (int)(m10 / area);
                Y = (int)(m01 / area);
            }

            if (_window != null)
            {
                var center = new Point(X,Y);
                var axes = new Size { Width = 5, Height = 5 };
                Cv2.Ellipse(_dst, center, axes, 0, 0, 360, new Scalar(255, 0, 0), 3);

                _window.ShowImage(_dst);
            }
        }
    }
}