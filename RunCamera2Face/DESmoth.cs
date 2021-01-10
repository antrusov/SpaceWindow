namespace SpaceWindow
{
    //Double exponential smoothing
    public class DESmoth
    {
        double _alpha;
        double _beta;
        double _s;
        double _b;

        public double Value
        {
            get
            {
                return _s;
            }
        }

        public DESmoth (double alpha = 0.35, double beta = 0.35)
        {
            _alpha = alpha;
            _beta = beta;
            _s = 0;
            _b = 0;
        }

        public void Reset(double startValue)
        {
            _s = startValue;
            _b = 0;
        }

        public double AddNewValue(double newRawValue)
        {
            var st = _alpha*newRawValue + (1-_alpha)*(_s + _b);
            var bt = _beta*(st - _s) + (1 - _beta)*_b;

            _s = st;
            _b = bt;
            
            return _s;
        }
    }
}