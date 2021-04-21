using System;
using System.IO;

namespace RkHelper.IO
{
    /// <summary>
    /// A file which given file path will be deleted in disposing
    /// </summary>
    public class ScopedTemporaryFile : IDisposable
    {
        public string FilePath { get; }

        public ScopedTemporaryFile( string filePath )
        {
            FilePath = filePath;
        }

        public FileStream Open( FileMode mode ) => File.Open( FilePath, mode );
        public FileStream OpenRead() => File.OpenRead( FilePath );
        public FileStream OpenWrite() => File.OpenWrite( FilePath );

        public void Dispose()
        {
            if( !File.Exists( FilePath ) )
            {
                return;
            }

            try
            {
                File.Delete( FilePath );
            }
            catch
            {
                // ignored
            }
        }

        public void CreateEmptyFile()
        {
            try
            {
                File.WriteAllBytes( FilePath, Array.Empty<byte>() );
            }
            catch
            {
                // ignored
            }
        }
    }
}