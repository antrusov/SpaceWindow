using System;
using OpenCvSharp;

namespace SpaceWindow
{
    public class StereoSolver
    {
        public double _distance { get; set; }
        public double _height { get; set; }
        public double _hAngle { get; set; }
        public double _vAngle { get; set; }
        public int _imgWidth { get; set; }
        public int _imgHeight { get; set; }

        public StereoSolver(double distance, double height, double hAngle, double vAngle, int imgWidth, int imgHeight)
        {
            _distance = distance;
            _height = height;
            _hAngle = hAngle;
            _vAngle = vAngle;
            _imgWidth = imgWidth;
            _imgHeight = imgHeight;
        }

        public Point3d To3D(Point p1, Point p2)
        {
            var a1 = getAngleByPos(p1.X, _hAngle, _imgWidth);
            var a2 = getAngleByPos(p2.X, _hAngle, _imgWidth);
            var a3 = getAngleByPos(p1.Y, _vAngle, _imgHeight);

            //Console.WriteLine($" a1:{180.0 * (Math.PI - a1) / Math.PI} a2:{180.0 * a2 / Math.PI} a3:{180.0 * a3 / Math.PI}");

            var dist = getHeightBySideAndTwoAngles(_distance, Math.PI - a1, a2);
            var x = getSideByAngleAndOppositeSide(Math.PI - a1, dist);
            var y = getSideByAngleAndOppositeSide(a3, dist);

            //Console.WriteLine($" dist:{dist} x:{x} y:{y} ");

            return new Point3d(-x, _height - y, dist);
        }

        #region [ вспомогательные методы ]

        double getAngleByPos (int pos, double angle, int size)
        {
            var xCatet = size / 2.0;
            var b = (Math.PI * angle / 180.0) / 2.0;
            var a = Math.PI / 2.0 - b;
            var yCatet = xCatet * Math.Tan(a);

            var x = pos - size / 2.0;
            var res = Math.PI / 2.0 - Math.Atan(x / yCatet);

            return res;
        }

        double getHeightBySideAndTwoAngles (double side, double a1, double a2)
        {
            var height = side * Math.Sin(a1) * Math.Sin(a2) / Math.Sin(a1 + a2);

            return height;
        }

        double getSideByAngleAndOppositeSide(double a, double side)
        {
            return side / Math.Tan(a);
        }

        #endregion
    }
}