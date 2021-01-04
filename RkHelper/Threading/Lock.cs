using System;
using System.Threading;

namespace RkHelper.Threading
{
    public class Lock
    {
        private SemaphoreSlim Semaphore { get; }

        public Lock()
        {
            Semaphore = new SemaphoreSlim( 0, 1 );
        }

        public Lock( SemaphoreSlim semaphore )
        {
            Semaphore = semaphore;
        }

        public Lock( int initialCount, int maxCount )
        {
            Semaphore = new SemaphoreSlim( initialCount, maxCount );
        }

        public Lock( int initialCount )
        {
            Semaphore = new SemaphoreSlim( initialCount );
        }

        public IDisposable GetLockObject()
        {
            Semaphore.Wait();
            return new LockObject( Semaphore );
        }

        private class LockObject : IDisposable
        {
            private SemaphoreSlim Semaphore { get; }

            public LockObject( SemaphoreSlim semaphore )
            {
                Semaphore = semaphore;
            }

            public void Dispose()
            {
                Semaphore.Release();
            }
        }
    }
}