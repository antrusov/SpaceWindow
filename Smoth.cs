using System;

namespace SpaceWindow {
    public class Smoth    
    {
        readonly double _a;
        double _smothValue;
        double _rawValue;

        public double Value { get { return _smothValue; } }

        public Smoth(double a = 0.1) {
            _a = a;
        }

        public void Reset(double startValue) {
            _rawValue = startValue;
            _smothValue = startValue;
        }

        public double AddNewValue(double newRawValue)
        {
            //https://towardsdatascience.com/simple-exponential-smoothing-749fc5631bed
            var value = _a * _smothValue + (1 - _a) * _rawValue;

            _smothValue = value;
            _rawValue = newRawValue;

            return value;
        }
    }

}