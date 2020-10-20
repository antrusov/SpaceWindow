using System;

namespace SpaceWindow {
    public class Smoth    
    {
        readonly double _a;
        double _oldPrognose;
        double _oldValue;

        public double Value { get { return _oldPrognose; } }

        public Smoth(double a = 0.1) {
            _a = a;
        }

        public void Reset(double startValue) {
            _oldValue = startValue;
            _oldPrognose = startValue;
        }

        public double AddNewValue(double newValue)
        {
            //https://towardsdatascience.com/simple-exponential-smoothing-749fc5631bed
            var value = _a * _oldPrognose + (1 + _a) * _oldValue;

            _oldPrognose = value;
            _oldValue = newValue;

            return value;
        }
    }

}