using System.IO;

using NUnit.Framework;

using RkHelper.IO;

namespace RkHelper.Testing.IO
{
    [TestFixture]
    public class ScopedTemporaryFileTest
    {
        [Test]
        public void AliveFileScopedOnlyTest()
        {
            var tempPath = Path.Combine( Path.GetTempPath(), Path.GetRandomFileName() );
            var tempFile = new ScopedTemporaryFile( tempPath );

            Assert.IsFalse( File.Exists( tempFile.FilePath ) );

            tempFile.CreateEmptyFile();
            Assert.IsTrue( File.Exists( tempFile.FilePath ) );
            Assert.IsTrue( new FileInfo( tempFile.FilePath ).Length == 0 );

            tempFile.Dispose();
            Assert.IsFalse( File.Exists( tempFile.FilePath ) );
        }
    }
}