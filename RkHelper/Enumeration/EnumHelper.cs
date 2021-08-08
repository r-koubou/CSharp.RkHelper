using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace RkHelper.Enumeration
{
    public static class EnumHelper
    {
        #region Parser
        // see also
        // https://qiita.com/masaru/items/a44dc30bfc18aac95015
        public static TEnum Parse<TEnum>( string text, TEnum defaultValue ) where TEnum : struct
        {
            if( string.IsNullOrEmpty( text ) )
            {
                return defaultValue;
            }

            if( !TryParse( text, out TEnum target ) )
            {
                return defaultValue;
            }

            return target;
        }

        public static bool TryParse<TEnum>( string text, out TEnum target ) where TEnum : struct
        {
            return Enum.TryParse( text, out target ) && Enum.IsDefined( typeof( TEnum ), target );
        }

        public static TEnum Parse<TEnum>( string text ) where TEnum : struct
        {
            if( !TryParse( text, out TEnum target ) )
            {
                throw new EnumValueNotFoundException( text, typeof( TEnum ) );
            }

            return target;
        }
        #endregion Parser

        #region Converter
        public static bool TryFromInt<T>( int v, [MaybeNull] out T result ) where T : struct
        {
            result = default;
            try
            {
                result = (T)Enum.ToObject( typeof( T ), v );
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T FromInt<T>( int v ) where T : struct
        {
            return (T)Enum.ToObject( typeof( T ), v );
        }

        public static IReadOnlyList<T> GetValues<T>() where T : struct
        {
            return Enum.GetValues( typeof( T ) ).Cast<T>().ToList();
        }

        #endregion
    }
}