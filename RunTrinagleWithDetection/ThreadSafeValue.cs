using System;
using System.Threading;

namespace SpaceWindow
{
    public class ThreadSafeValue<T> where T : struct 
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
}