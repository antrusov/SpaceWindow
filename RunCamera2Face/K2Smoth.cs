namespace SpaceWindow
{
    //реализация фильтра Калмана вот отсюда:
    //https://habr.com/ru/post/140274/
    class K2Smoth
    {
        public double X0 {get; private set;} // predicted state
        public double P0 { get; private set; } // predicted covariance

        public double F { get; private set; } // factor of real value to previous real value
        public double Q { get; private set; } // measurement noise
        public double H { get; private set; } // factor of measured value to real value
        public double R { get; private set; } // environment noise

        public double Value { get; private set; }
        public double Covariance { get; private set; }

        public K2Smoth(double q = 2, double r = 15, double f = 1, double h = 1)
        {
            Q = q;
            R = r;
            F = f;
            H = h;
        }

        public void Reset(double state, double covariance = 0.1)
        {
            Value = state;
            Covariance = covariance;
        }

        public double AddNewValue(double data)
        {
            //time update - prediction
            X0 = F*Value;
            P0 = F*Covariance*F + Q;

            //measurement update - correction
            var K = H*P0/(H*P0*H + R);
            Value = X0 + K*(data - H*X0);
            Covariance = (1 - K*H)*P0;   

            return Value;         
        }
    }
}