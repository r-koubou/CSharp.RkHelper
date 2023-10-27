using System;

namespace RkHelper.Primitives
{
    public class EmptyTextException : Exception
    {
        public EmptyTextException()
        {}

        public EmptyTextException( string message ) : base( message )
        {}
    }
}
