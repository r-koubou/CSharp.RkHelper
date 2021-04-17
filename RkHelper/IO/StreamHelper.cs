using System.IO;
using System.Reflection;

namespace RkHelper.IO
{
    public static class StreamHelper
    {
        private const int WorkSize = 8192;
        private static readonly byte[] WorkBuffer = new byte[ WorkSize ];

        #region Read, Write

        public static void ReadAllAndWrite( Stream source, Stream dest )
        {
            while( true )
            {
                var readByte = source.Read( WorkBuffer, 0, WorkBuffer.Length );

                if( readByte == 0 )
                {
                    break;
                }

                dest.Write( WorkBuffer, 0, readByte );
            }
        }

        public static void ReadBytes( Stream stream, byte[] buffer, int offset, int length )
        {
            var rest = length;
            var ofs = offset;

            while( rest > 0 )
            {
                var readByte = stream.Read( buffer, ofs, length );

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

        public static byte[] GetAssemblyResourceBytes<TClass>( string resourceName )
            where TClass : class
        {
            using var stream = GetAssemblyResourceStream<TClass>( resourceName );

            if( stream == null )
            {
                return new byte[ 0 ];
            }

            using var memoryStream = new MemoryStream();
            ReadAllAndWrite( stream, memoryStream );

            return memoryStream.ToArray();
        }

        #endregion

    }
}