using System;
using OpenCvSharp;

namespace SpaceWindow
{
    public class Camera2ColorFilter
    {
        int _cameraIndex;
        int _minH;
        int _maxH;
        int _minS;
        int _maxS;
        int _minV;
        int _maxV;

        int _srcWidth;
        int _srcHeight;

        VideoCapture _capture; //обьект для захвата видеопотока из камеры
        Window _rawWindow; //окно с исходным изображением
        Window _dstWindow; //окно с результатом

        Window _hTrackWindow;
        Window _sTrackWindow;
        Window _vTrackWindow;

        Mat _src; //очередное изображение с камеры
        Mat _dst; //обрезанное изображение
        Mat _hTrimmed;
        Mat _sTrimmed;
        Mat _vTrimmed;
        Mat _hsv; //изображение, преобразованное в HSV
        Mat _h; //h канал
        Mat _s; //s канал
        Mat _v; //v канал
        Mat _hRange; //h канал (обрезанный)
        Mat _sRange; //s канал (обрезанный)
        Mat _vRange; //v канал (обрезанный)
        Mat _hsvRange; //результирующее hsv изображение

        public Camera2ColorFilter (
            int cameraIndex,
            int minH = 0, int maxH = 255,
            int minS = 0, int maxS = 255,
            int minV = 0, int maxV = 255
        )
        {
            _cameraIndex = cameraIndex;
            _minH = minH;
            _maxH = maxH;
            _minS = minS;
            _maxS = maxS;
            _minV = minV;
            _maxV = maxV;

            //исходное окно
            _capture = new VideoCapture(cameraIndex);
            _srcWidth = _capture.FrameWidth;
            _srcHeight = _capture.FrameHeight;

            //окна
            _rawWindow = new Window($"Raw window. Camera: index = {cameraIndex}");
            _dstWindow = new Window($"Result");
            _hTrackWindow = new Window($"H Track");
            _sTrackWindow = new Window($"S Track");
            _vTrackWindow = new Window($"V Track");

            //изображения
            _src = new Mat(_srcWidth, _srcHeight, MatType.CV_8UC4);
            _dst = new Mat(_srcWidth, _srcHeight, MatType.CV_8UC4);
            _hsv = new Mat(_srcWidth, _srcHeight, MatType.CV_8UC1);
            _h = new Mat(_srcWidth, _srcHeight, MatType.CV_8UC1);
            _s = new Mat(_srcWidth, _srcHeight, MatType.CV_8UC1);
            _v = new Mat(_srcWidth, _srcHeight, MatType.CV_8UC1);
            _hRange = new Mat(_srcWidth, _srcHeight, MatType.CV_8UC1);
            _sRange = new Mat(_srcWidth, _srcHeight, MatType.CV_8UC1);
            _vRange = new Mat(_srcWidth, _srcHeight, MatType.CV_8UC1);
            _hsvRange = new Mat(_srcWidth, _srcHeight, MatType.CV_8UC1);
            _hTrimmed = new Mat(_srcWidth, _srcHeight, MatType.CV_8UC4);
            _sTrimmed = new Mat(_srcWidth, _srcHeight, MatType.CV_8UC4);
            _vTrimmed = new Mat(_srcWidth, _srcHeight, MatType.CV_8UC4);

            //ползунки
            Cv2.CreateTrackbar("minH", "H Track", ref _minH, 255);
            Cv2.CreateTrackbar("maxH", "H Track", ref _maxH, 255);
            Cv2.CreateTrackbar("minS", "S Track", ref _minS, 255);
            Cv2.CreateTrackbar("maxS", "S Track", ref _maxS, 255);
            Cv2.CreateTrackbar("minV", "V Track", ref _minV, 255);
            Cv2.CreateTrackbar("maxV", "V Track", ref _maxV, 255);

            //расположить окна
            int dx = 10;
            int dy = 32;
            _rawWindow.Move(0,0);
            _dstWindow.Move(_srcWidth + dx, 0);

            _hTrackWindow.Move((_srcWidth + dx) * 0, _srcHeight + dy);
            _sTrackWindow.Move((_srcWidth + dx) * 1, _srcHeight + dy);
            _vTrackWindow.Move((_srcWidth + dx) * 2, _srcHeight + dy);
        }

        public void Update()
        {
            _capture.Read(_src);

            Cv2.CvtColor(_src, _hsv, ColorConversionCodes.BGR2HSV);
            Cv2.ExtractChannel(_hsv, _h, 0);
            Cv2.ExtractChannel(_hsv, _s, 1);
            Cv2.ExtractChannel(_hsv, _v, 2);

            Cv2.InRange(_h, _minH, _maxH, _hRange);
            Cv2.InRange(_s, _minS, _maxS, _sRange);
            Cv2.InRange(_v, _minV, _maxV, _vRange);

            _dst.SetTo(new Scalar(0,0,0));
            _hTrimmed.SetTo(new Scalar(0,0,0));
            _sTrimmed.SetTo(new Scalar(0,0,0));
            _vTrimmed.SetTo(new Scalar(0,0,0));
            _hsvRange.SetTo(new Scalar(0,0,0));

            Cv2.BitwiseAnd(_hRange, _sRange, _hsvRange);
            Cv2.BitwiseAnd(_hsvRange, _vRange, _hsvRange);

            //вычисление центра
            var errodeElement = Cv2.GetStructuringElement(MorphShapes.Ellipse, new Size(5,5));
            Cv2.Erode(_hsvRange, _hsvRange, errodeElement);
            var moments = Cv2.Moments(_hsvRange, binaryImage: true);
            var m01 = moments.M01;
            var m10 = moments.M10;
            var area = moments.M00;
            var x = (int)(m10 / area);
            var y = (int)(m01 / area);

            //рисование результата
            var center = new Point(x,y);
            var axes = new Size { Width = 5, Height = 5 };
            Cv2.BitwiseAnd(_src, _src, _dst, _hsvRange);        
            Cv2.Ellipse(_dst, center, axes, 0, 0, 360, new Scalar(255, 0, 0), 3);

            //рисование каналов
            Cv2.BitwiseAnd(_h, _h, _hTrimmed, _hRange);
            Cv2.BitwiseAnd(_s, _s, _sTrimmed, _sRange);
            Cv2.BitwiseAnd(_v, _v, _vTrimmed, _vRange);

            //Cv2.MinMaxLoc(_h, out _minH, out _maxH)
            //Cv2.MinMaxLoc(_s, out _minS, out _maxS)
            //Cv2.MinMaxLoc(_v, out _minV, out _maxV)

            _rawWindow.ShowImage(_src);
            _dstWindow.ShowImage(_dst);
            _hTrackWindow.ShowImage(_hTrimmed);
            _sTrackWindow.ShowImage(_sTrimmed);
            _vTrackWindow.ShowImage(_vTrimmed);
        }
    }
}