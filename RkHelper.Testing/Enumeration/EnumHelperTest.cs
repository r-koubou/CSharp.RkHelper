using NUnit.Framework;

using RkHelper.Enumeration;

namespace RkHelper.Testing.Enumeration
{
    [TestFixture]
    public class EnumHelperTest
    {
        private enum TestEnum
        {
            Hoge,
            Fuga,
            Baba
        }

        [Test]
        [TestCase( "Hoge" )]
        [TestCase( "Fuga" )]
        [TestCase( "Baba" )]
        public void ParseTest( string enumName )
        {
            var value = EnumHelper.Parse<TestEnum>( enumName );
            Assert.AreEqual( value.ToString(), enumName );

            Assert.IsTrue( EnumHelper.TryParse( enumName, out value ) );
            Assert.AreEqual( value.ToString(), enumName );
        }

        [Test]
        [TestCase( "Sasa" )]
        [TestCase( "Kusa" )]
        [TestCase( "Popo" )]
        public void ParseWithDefaultTest( string enumName )
        {
            var value = EnumHelper.Parse( enumName, TestEnum.Baba );
            Assert.AreEqual( value.ToString(), TestEnum.Baba.ToString() );
        }

        [Test]
        [TestCase( "aaaa" )]
        [TestCase( "bbbb" )]
        [TestCase( "cccc" )]
        public void ParseFailTest( string enumName )
        {
            Assert.Catch<EnumValueNotFoundException>(  () => EnumHelper.Parse<TestEnum>( enumName ) );

            Assert.IsFalse( EnumHelper.TryParse<TestEnum>( enumName, out var value ) );
            Assert.AreNotEqual( value.ToString(), enumName );
        }
    }
}