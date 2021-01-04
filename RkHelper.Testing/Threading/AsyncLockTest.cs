using System.Collections;
using System.Threading.Tasks;

using NUnit.Framework;

using RkHelper.Threading;

namespace RkHelper.Testing.Threading
{
    [TestFixture]
    public class AsyncLockTest
    {
        private AsyncLock LockObject { get; } = new( 1 );

        [Test]
        [TestCase(1000)]
        public void AsyncTest( int count )
        {
            var array = new ArrayList();
            var incCount = 0;

            var tasks = new Task[ count ];

            for( var i = 0; i < count; i++ )
            {
                tasks[ i ] = Task.Run( async () => {
                    using( await LockObject.GetLockObject() )
                    {
                        array.Add( incCount );
                        incCount++;
                    }
                });
            }

            Task.WaitAll( tasks );

            for( var i = 0; i < count; i++ )
            {
                Assert.IsTrue( (int)array[ i ]! == i );
            }
        }
    }
}