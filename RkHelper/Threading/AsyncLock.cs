using System;
using System.Threading;
using System.Threading.Tasks;

namespace RkHelper.Threading
{
    public class AsyncLock
    {
        private SemaphoreSlim Semaphore { get; }

        public AsyncLock()
        {
            Semaphore = new SemaphoreSlim( 0, 1 );
        }

        public AsyncLock( SemaphoreSlim semaphore )
        {
            Semaphore = semaphore;
        }

        public AsyncLock( int initialCount, int maxCount )
        {
            Semaphore = new SemaphoreSlim( initialCount, maxCount );
        }

        public AsyncLock( int initialCount )
        {
            Semaphore = new SemaphoreSlim( initialCount );
        }

        public async Task<IDisposable> GetLockObject()
        {
            await Semaphore.WaitAsync();
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