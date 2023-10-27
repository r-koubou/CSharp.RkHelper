using System;
using System.Globalization;

using RkHelper.Text;

namespace RkHelper.Primitives
{
    public static class NumberHelper
    {
        #region Validation
        public static void ValidateRange<T>( T value, T min, T max ) where T : IComparable<T>
        {
            if( min.CompareTo( max ) > 0 )
            {
                throw new ArgumentOutOfRangeException( $"{nameof(min)} > {nameof(max)}. ({min} > {max})");
            }

            if( value.CompareTo( min ) < 0 )
            {
                throw new ArgumentOutOfRangeException( $"{nameof(value)}({value}) < {nameof(min)}({min})" );
            }

            if( value.CompareTo( max ) > 0 )
            {
                throw new ArgumentOutOfRangeException( $"{nameof(value)}({value}) > {nameof(max)}({max})" );
            }
        }

        public static bool InRange<T>(T value, T min, T max) where T : IComparable<T>
        {
            if( min.CompareTo( max ) > 0 ||
                value.CompareTo( min ) < 0 ||
                value.CompareTo( max ) > 0 )
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Parsing
        public static bool TryParse( string numberText, out int result, int fromBase = 10 )
        {
            result = 0;
            if( StringHelper.IsEmpty( numberText ) )
            {
                return false;
            }

            return fromBase switch
            {
                2  => TryParseBit( numberText, out result ),
                10 => TryParseDecimal( numberText, out result ),
                16 => TryParseHex( numberText, out result ),
                _  => throw new ArgumentException( $"fromBase is {fromBase}" )
            };
        }

        private static bool TryParseDecimal( string numberText, out int result )
        {
            if( int.TryParse( numberText, out result ) )
            {
                return true;
            }

            return false;
        }

        private static bool TryParseHex( string numberText, out int result )
        {
            if( numberText.Length >= 3 && numberText.ToLower().StartsWith( "0x" ) )
            {
                var hexText = numberText[ 2.. ];
                return int.TryParse( hexText, NumberStyles.AllowHexSpecifier, null, out result );
            }

            result = 0;
            return false;
        }

        private static bool TryParseBit( string numberText, out int result )
        {
            result = 0;

            if( numberText.Length >= 3 && numberText.ToLower().StartsWith( "0b" ) )
            {
                var bitText = numberText[ 2.. ];
                try
                {
                    result = Convert.ToInt32( bitText, 2 );
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;

        }
        #endregion

    }
}
