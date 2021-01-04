using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RkHelper.IO.Extension
{
    public static class StreamExtension
    {
        private const int WorkAreaSize = 2048;

        #region Read
        public static int ReadAll( this Stream stream, byte[] buffer, int offset, int count )
        {
            var x = offset;

            while( x < count )
            {
                var readBytes = stream.Read( buffer, x, count );

                if( readBytes == 0 )
                {
                    break;
                }

                x += readBytes;
            }

            return x;
        }

        public static Task<int> ReadAllAsync( this Stream stream, byte[] buffer, int offset, int count )
        {
            return ReadAllAsync( stream, buffer, offset, count, CancellationToken.None );
        }

        public static Task<int> ReadAllAsync( this Stream stream, byte[] buffer, int offset, int count, CancellationToken cancellationToken )
        {
            return Task.Run( async () =>
            {
                var x = offset;

                while( x < count )
                {
                    var readBytes = await stream.ReadAsync( buffer, x, count, cancellationToken );

                    if( readBytes == 0 )
                    {
                        break;
                    }

                    x += readBytes;
                }
                return x;
            }, cancellationToken );
        }
        #endregion

        #region Write
        public static void Write( this Stream stream, Stream source )
        {
            byte[] workArea = new byte[ WorkAreaSize ];

            while( true )
            {
                var readByte = source.Read( workArea, 0, workArea.Length );

                if( readByte == 0 )
                {
                    break;
                }

                stream.Write( workArea, 0, readByte );
            }
        }

        public static Task WriteAsync( this Stream stream, Stream source )
        {
            return WriteAsync( stream, source, CancellationToken.None );
        }

        public static Task WriteAsync( this Stream stream, Stream source, CancellationToken cancellationToken )
        {
            return Task.Run(  () => stream.Write( source ), cancellationToken );
        }
        #endregion

    }
}