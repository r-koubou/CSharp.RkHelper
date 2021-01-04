using NUnit.Framework;

using RkHelper.Hash;

namespace RkHelper.Testing.Hash
{
    [TestFixture]
    public class CrcSlicingBy8Test
    {
        [Test]
        public void Crc32Test()
        {
            var crc = new CrcSlicingBy8();
            var expect = new byte[] { 0xb5, 0x1d, 0x57, 0x1d };
            var crc32 = crc.ComputeHash( new byte[] { 0xca, 0xfe, 0xba, 0xbe } );

            Assert.AreEqual( expect, crc32 );
        }
    }
}