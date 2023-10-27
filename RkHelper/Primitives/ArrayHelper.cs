using System;

namespace RkHelper.Primitives
{
    public static class ArrayHelper
    {
        public static void ValidateArrayRange<T>( T[] array, int offset, int length )
        {
            if( array == null )
            {
                throw new ArgumentNullException( nameof( array ) );
            }

            if( offset < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( offset ), "offset < 0" );
            }

            if( length < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( length ), "length < 0" );
            }

            if( offset >= array.Length )
            {
                throw new ArgumentException( $"offset >= array.Length ({nameof( offset )}={offset}, {nameof( array )}.{nameof( array.Length )}={array.Length})" );
            }

            if( offset + length > array.Length )
            {
                throw new ArgumentException( $"offset + length > array.Length ({nameof( offset )}={offset}, {nameof( length )}={length}, {nameof( array )}.{nameof( array.Length )}={array.Length})" );
            }
        }
    }
}
