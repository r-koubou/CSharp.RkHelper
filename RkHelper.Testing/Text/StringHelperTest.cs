using System;

using NUnit.Framework;

using RkHelper.Text;

namespace RkHelper.Testing.Text
{
    [TestFixture]
    public class StringHelperTest
    {
        [Test]
        [TestCase( "" )]
        [TestCase( "    " )]
        [TestCase( null )]
        public void EmptyTest( string text )
        {
            Assert.IsTrue( StringHelper.IsEmpty( text ) );
            Assert.Catch<EmptyTextException>( () => StringHelper.ValidateEmpty( text ) );
        }

        [Test]
        public void NullTest()
        {
            Assert.IsTrue( StringHelper.IsNull( null! ) );
            Assert.IsTrue( StringHelper.IsNull( default! ) );
            Assert.Catch<ArgumentNullException>( () => StringHelper.ValidateNull( null! ) );
            Assert.Catch<ArgumentNullException>( () => StringHelper.ValidateNull( default! ) );
        }

        [Test]
        [TestCase( "C", "A", "B", "C" )]
        [TestCase( "A", "C", "B", "A" )]
        public void ContainsTest( string search, params string[] text )
        {
            Assert.IsTrue( StringHelper.Contains( search, text ) );
        }
        [Test]
        [TestCase( "123", "A", "B", "C" )]
        [TestCase( "456", "C", "B", "A" )]
        [TestCase( "", "C", "B", "A" )]
        public void NotContainsTest( string search, params string[] text )
        {
            Assert.IsFalse( StringHelper.Contains( search, text ) );
        }

    }
}