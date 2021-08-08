using System;

using NUnit.Framework;

using RkHelper.Number;

namespace RkHelper.Testing.Number
{
    [TestFixture]
    public class NumberHelperTest
    {
        [Test]
        [TestCase( 10,  0,    100 )]
        [TestCase( -10, -100, 100 )]
        [TestCase( 100, 0,   100 )]
        [TestCase( 0, 0,   100 )]
        public void ValidTest( int value, int min, int max )
        {
            NumberHelper.ValidateRange( value, min, max );
        }

        [Test]
        [TestCase( 101,  0,    100 )]
        [TestCase( -10, -5, 100 )]
        [TestCase( 101, 100,    100 )]
        public void ValidOutOfRangeTest( int value, int min, int max )
        {
            Assert.Catch<ArgumentOutOfRangeException>( () =>
            {
                NumberHelper.ValidateRange( value, min, max );
            });
        }

        [Test]
        [TestCase( 100, 0, 100 )]
        public void ValidMinMaxCheckingTest( int value, int min, int max )
        {
            NumberHelper.ValidateRange( value, min, max );
        }

        [Test]
        [TestCase( 100, 101, 100 )]
        public void InvalidMinMaxCheckingTest( int value, int min, int max )
        {
            Assert.Catch<ArgumentException>( () =>
            {
                NumberHelper.ValidateRange( value, min, max );
            });
        }

        [TestCase( "12",         12,         10 )]
        [TestCase( "0xAB",       0xab,       16 )]
        [TestCase( "0b00001111", 0b00001111, 2 )]
        public void IntTryParseTest( string hexString, int hex, int d )
        {
            var r = NumberHelper.TryParse( hexString, out var result, d );
            Assert.IsTrue( r, "TryParse" );
            Assert.AreEqual( hex, result, "Value" );
        }

    }
}