using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace RkHelper.IO
{
    public static class StreamHelper
    {
        private const int WorkSize = 8192;
        private static readonly byte[] WorkBuffer = new byte[ WorkSize ];

        public static T? DisposeWithFlush<T>( T stream, bool returnNull = true ) where T : Stream
        {
            try { stream.Flush(); } catch { /* ignored */}
            try { stream.Close(); } catch { /* ignored */}
            try { stream.Dispose(); } catch { /* ignored */}

            return returnNull ? null : stream;
        }

        public static T? DisposeWithClose<T>( T stream, bool returnNull = true ) where T : Stream
        {
            try { stream.Close(); } catch { /* ignored */}
            try { stream.Dispose(); } catch { /* ignored */}

            return returnNull ? null : stream;
        }

        #region Read, Write

        public static void ReadAllAndWrite( Stream source, Stream dest )
            => ReadAllAndWriteAsync( source, dest ).GetAwaiter().GetResult();

        public async static Task ReadAllAndWriteAsync( Stream source, Stream dest )
        {
            while( true )
            {
                var readByte = await source.ReadAsync( WorkBuffer, 0, WorkBuffer.Length );

                if( readByte == 0 )
                {
                    break;
                }

                await dest.WriteAsync( WorkBuffer, 0, readByte );
            }
        }

        public static void ReadBytes( Stream stream, byte[] buffer, int offset, int length )
            => ReadBytesAsync( stream, buffer, offset, length ).GetAwaiter().GetResult();

        public async static Task ReadBytesAsync( Stream stream, byte[] buffer, int offset, int length )
        {
            var rest = length;
            var ofs = offset;

            while( rest > 0 )
            {
                var readByte = await stream.ReadAsync( buffer, ofs, length );

                if( readByte == 0 )
                {
                    break;
                }

                ofs  += readByte;
                rest -= readByte;
            }
        }

        #endregion

        #region Manifest Resource

        /// <summary>
        /// Alias of Assembly.GetManifestResourceStream(string)
        /// Try to get stream with `TClass's assembly root namespace`.`resourceName`
        /// </summary>
        /// <typeparam name="TClass">Using for TClass's assembly root namespace</typeparam>
        /// <returns></returns>
        public static Stream? GetAssemblyResourceStream<TClass>( string resourceName )
            where TClass : class
        {
            var type = typeof( TClass );
            var asm = Assembly.GetAssembly( type );
            return asm?.GetManifestResourceStream( asm.GetName().Name + "." + resourceName );
        }

        public static byte[] GetAssemblyResourceBytes<TClass>( string resourceName ) where TClass : class
            => GetAssemblyResourceBytesAsync<TClass>( resourceName ).GetAwaiter().GetResult();

        public static async Task<byte[]> GetAssemblyResourceBytesAsync<TClass>( string resourceName )
            where TClass : class
        {
            await using var stream = GetAssemblyResourceStream<TClass>( resourceName );

            if( stream == null )
            {
                return Array.Empty<byte>();
            }

            using var memoryStream = new MemoryStream();
            await ReadAllAndWriteAsync( stream, memoryStream );

            return memoryStream.ToArray();
        }

        #endregion

    }
}
