using System;
using System.Threading;

namespace SpaceWindow
{
    public class TriangleWithDetection
    {
        public TriangleWithDetection ()
        {
            //...
        }

        public void Run ()
        {
            var pos = new ThreadSafeValue<int>();
            var render = new Render(pos);
            var tracker = new Tracker(pos);

            tracker.StartTracking();
            render.Run();
            tracker.StopTracking();
        }
    }

    class ThreadSafeValue<T> where T : struct 
    {
        T _value;
        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        public T Value
        {
            get
            {
                cacheLock.EnterReadLock();
                try
                {
                    return _value;
                }
                finally
                {
                    cacheLock.ExitReadLock();
                }
            }
            set
            {
                cacheLock.EnterWriteLock();
                try
                {
                    _value = value;
                }
                finally
                {
                    cacheLock.ExitWriteLock();
                }
            }
        }
        ~ThreadSafeValue()
        {
            if (cacheLock != null) cacheLock.Dispose();
        }
    }

    class Render
    {
        ThreadSafeValue<int> _pos;

        public Render(ThreadSafeValue<int> pos)
        {
            _pos = pos;
            //...
        }

        public void Run ()
        {
            using var win = new OpenCvSharp.Window("test");
            while (true)
            {
                //...
                Console.WriteLine(_pos.Value);
                if (OpenCvSharp.Cv2.WaitKey(100) != -1)
                    break;
            }
            //OpenCvSharp.Cv2.WaitKey();
        }
    }

    class Tracker
    {
        Random rnd = new Random();
        ThreadSafeValue<int> _pos;

        Thread t;

        bool run = false;

        public Tracker(ThreadSafeValue<int> pos)
        {
            _pos = pos;
        }

        public void StartTracking ()
        {
            t = new Thread(TrackingProc);
            t.IsBackground = true; //основной процесс при своем закрытии не будет ждать завершения этого потока
            t.Start();
        }

        public void StopTracking ()
        {
            run = false; //инициировали закрытие потока
            t.Join(); //ждем, пока поток не завершится
        }

        void TrackingProc()
        {
            run = true;

            Console.WriteLine("Init Track");
            while (run)
            {
                _pos.Value = rnd.Next(10, 100);
                Console.WriteLine("Loop Track");
                Thread.Sleep(250);
            }
            Console.WriteLine("End Track");
        }
    }
}