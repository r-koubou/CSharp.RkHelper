using System.Collections;
using System.Threading.Tasks;

using NUnit.Framework;

using RkHelper.Threading;

namespace RkHelper.Testing.Threading
{
    [TestFixture]
    public class LockTest
    {
        private Lock LockObject { get; } = new Lock( 1 );

        [SetUp]
        public void SetUp()
        {}

        [Test]
        [TestCase(1000)]
        public void AsyncTest( int count )
        {
            var incCount = 0;
            var array = new ArrayList();

            var option = new ParallelOptions
            {
                MaxDegreeOfParallelism = 4
            };

            var parallel = Parallel.For( 0, count, option, _ =>
                {
                    using( LockObject.GetLockObject() )
                    {
                        array.Add( incCount );
                        incCount++;
                    }
                }
            );

            while( !parallel.IsCompleted )
            {
                Task.Delay( 60 );
            }

            for( var i = 0; i < count; i++ )
            {
                Assert.IsTrue( (int)array[ i ]! == i );
            }
        }
    }
}