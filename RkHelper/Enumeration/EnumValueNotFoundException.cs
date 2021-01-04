using System;

namespace RkHelper.Enumeration
{
    public class EnumValueNotFoundException : Exception
    {
        public EnumValueNotFoundException()
        {}

        public EnumValueNotFoundException( string message ) : base( message )
        {}

        public EnumValueNotFoundException( string name, Type enumType ) :
            this( $"Enum value `{name}` does not exist in type({enumType})" )
        {}
    }
}