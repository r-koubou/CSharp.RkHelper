using System;

namespace RkHelper.Text
{
    [Obsolete( "Use RkHelper.Primitives.EmptyTextException instead" )]
    public class EmptyTextException : Exception
    {
        public EmptyTextException()
        {}

        public EmptyTextException( string message ) : base( message )
        {}
    }
}
