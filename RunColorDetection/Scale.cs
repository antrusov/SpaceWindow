using System;

namespace SpaceWindow
{
    public class Scale
    {
        double _from1;
        double _from2;
        double _to1;
        double _to2;

        public Scale (double from1, double from2, double to1, double to2)
        {
            _from1 = from1;
            _from2 = from2;
            _to1 = to1;
            _to2 = to2;
        }

        public double Get(double x) => (_to2 - _to1) * (x - _from1) / (_from2 - _from1) + _to1;

    }
}