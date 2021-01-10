namespace SpaceWindow
{
    //фильтрация по Калману
    public class KSmoth
    {
        double _k;
        double _s;

        public KSmoth(double k = 0.5)
        {
            _k = k;
        }

        public double Value { get { return _s; } }

        public void Reset(double startValue)
        {
            _s = startValue;
        }

        public double AddNewValue(double newRawValue)
        {
            _s = _k * newRawValue + (1 - _k) * _s;
            return _s;
        }
    }
}