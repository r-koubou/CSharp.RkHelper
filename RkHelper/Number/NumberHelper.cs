using System;

namespace RkHelper.Number
{
    public static class NumberHelper
    {
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
    }
}