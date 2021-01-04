using System;
using System.Security.Cryptography;

// based: https://qiita.com/IL_k/items/06f8579c97d0397e6284

namespace RkHelper.Hash
{
    public sealed class CrcSlicingBy8 : HashAlgorithm
    {
        private const uint DefaultCrcValue = 0xFFFFFFFF;
        private const int BufferSize       = 1024 * 16;

        private static readonly uint[] U32Table0 = new uint[ 256 ];
        private static readonly uint[] U32Table1 = new uint[ 256 ];
        private static readonly uint[] U32Table2 = new uint[ 256 ];
        private static readonly uint[] U32Table3 = new uint[ 256 ];
        private static readonly uint[] U32Table4 = new uint[ 256 ];
        private static readonly uint[] U32Table5 = new uint[ 256 ];
        private static readonly uint[] U32Table6 = new uint[ 256 ];
        private static readonly uint[] U32Table7 = new uint[ 256 ];

        private readonly byte[] _u8Buf = new byte[ BufferSize ];

        private uint _crc32 = DefaultCrcValue;

        public uint Crc32 => _crc32 ^ 0xFFFFFFFF;

        static CrcSlicingBy8()
        {
            unchecked
            {
                const uint polynomial = 0xEDB88320;
                uint i;
                uint crc;

                for( i = 0; i < 256; i++ )
                {
                    crc = i;

                    for( uint j = 8; j > 0; j-- )
                    {
                        if( ( crc & 1 ) == 1 )
                        {
                            crc = ( crc >> 1 ) ^ polynomial;
                        }
                        else
                        {
                            crc >>= 1;
                        }
                    }
                    U32Table0[ i ] = crc;
                }
                //Slicing by 8 Table
                for( i = 0; i < 256; i++ )
                {
                    crc            = U32Table0[ i ];
                    crc            = U32Table1[ i ] = U32Table0[ crc & 0xff ] ^ ( crc >> 8 );
                    crc            = U32Table2[ i ] = U32Table0[ crc & 0xff ] ^ ( crc >> 8 );
                    crc            = U32Table3[ i ] = U32Table0[ crc & 0xff ] ^ ( crc >> 8 );
                    crc            = U32Table4[ i ] = U32Table0[ crc & 0xff ] ^ ( crc >> 8 );
                    crc            = U32Table5[ i ] = U32Table0[ crc & 0xff ] ^ ( crc >> 8 );
                    crc            = U32Table6[ i ] = U32Table0[ crc & 0xff ] ^ ( crc >> 8 );
                    U32Table7[ i ] = U32Table0[ crc & 0xff ] ^ ( crc >> 8 );
                }
            }
        }

        #region HashAlgorithm
        protected override void HashCore( byte[] array, int ibStart, int cbSize )
        {
            #region Argument check
            if( ibStart < 0 )
            {
                throw new ArgumentException( $"ibStart < 0 (={ibStart})" );
            }
            if( ibStart >= array.Length  )
            {
                throw new ArgumentException( $"ibStart >= array.Length (={ibStart})" );
            }
            if( cbSize < 0 )
            {
                throw new ArgumentException( $"cbSize < 0 (={cbSize})" );
            }
            if( cbSize > array.Length  )
            {
                throw new ArgumentException( $"cbSize > array.Length (={cbSize})" );
            }
            #endregion

            var rest = cbSize;
            var offset = ibStart;

            while( rest > 0 )
            {
                var copySize = Math.Min( rest, BufferSize );
                HashCoreImpl( array, offset, copySize );

                rest   -= copySize;
                offset += copySize;
            }
        }

        private void HashCoreImpl( byte[] source, int offset, int count )
        {
            var blocks = ( count / 8 ) * 8;

            Buffer.BlockCopy( source, offset, _u8Buf, 0, count );

            for( var i = 0; i < blocks; i += 8 )
            {
                _crc32 ^= (uint)( _u8Buf[ i ] | _u8Buf[ i + 1 ] << 8 | _u8Buf[ i + 2 ] << 16 | _u8Buf[ i + 3 ] << 24 );
                _crc32 = U32Table7[ _crc32 & 0xFF ] ^ U32Table6[ ( _crc32 >> 8 ) & 0xFF ] ^
                        U32Table5[ ( _crc32 >> 16 ) & 0xFF ] ^ U32Table4[ ( _crc32 >> 24 ) & 0xFF ] ^
                        U32Table3[ _u8Buf[ i + 4 ] ] ^ U32Table2[ _u8Buf[ i + 5 ] ] ^
                        U32Table1[ _u8Buf[ i + 6 ] ] ^ U32Table0[ _u8Buf[ i + 7 ] ];
            }

            var rem = count % 8;
            offset = count - rem;

            for( var i = offset; i < rem; i++ )
            {
                _crc32 = ( _crc32 >> 8 ) ^ U32Table0[ ( _crc32 ^ _u8Buf[ i ] ) & 0xFF ];
            }

            #region unsafe version
            // unsafe
            // {
            //     Buffer.BlockCopy( source, offset, _u8Buf, 0, count );
            //
            //     fixed( byte* u8pc = _u8Buf )
            //     {
            //         int i;
            //         var u8p = u8pc;
            //         var first = ( (int)u8p ) % 4;
            //
            //         if( first > count )
            //         {
            //             first = count;
            //         }
            //
            //         var blocks = ( ( count - first ) / 8 );
            //         var last = count - ( blocks * 8 ) - first;
            //
            //         for( i = 0; i < first; i++ )
            //         {
            //             _crc32 = ( _crc32 >> 8 ) ^ U32Table0[ ( _crc32 ^ *u8p ) & 0xFF ];
            //             u8p++;
            //         }
            //
            //         for( i = 0; i < blocks; i++ )
            //         {
            //             _crc32 ^= *(uint*)u8p;
            //             u8p    += 4;
            //
            //             _crc32 = U32Table7[ _crc32 & 0xFF ] ^ U32Table6[ ( _crc32 >> 8 ) & 0xFF ] ^
            //                      U32Table5[ ( _crc32 >> 16 ) & 0xFF ] ^ U32Table4[ ( _crc32 >> 24 ) & 0xFF ] ^
            //                      U32Table3[ *(uint*)u8p & 0xFF ] ^ U32Table2[ ( *(uint*)u8p >> 8 ) & 0xFF ] ^
            //                      U32Table1[ ( *(uint*)u8p >> 16 ) & 0xFF ] ^ U32Table0[ ( *(uint*)u8p >> 24 ) & 0xFF ];
            //             u8p += 4;
            //         }
            //
            //         for( i = 0; i < last; i++ )
            //         {
            //             _crc32 = ( _crc32 >> 8 ) ^ U32Table0[ ( _crc32 ^ *u8p ) & 0xFF ];
            //             u8p++;
            //         }
            //     }
            // }
            #endregion
        }

        protected override byte[] HashFinal()
        {
            var crc = Crc32;

            return new[]
            {
                (byte)( ( crc >> 24 ) & 0xFF ),
                (byte)( ( crc >> 16 ) & 0xFF ),
                (byte)( ( crc >>  8 ) & 0xFF ),
                (byte)( crc & 0xFF ),
            };
        }

        public override void Initialize()
        {
            _crc32 = DefaultCrcValue;
        }
        #endregion
    }
}