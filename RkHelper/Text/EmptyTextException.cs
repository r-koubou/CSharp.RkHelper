using System;

namespace RkHelper.Text
{
    public class EmptyTextException : Exception
    {
        public EmptyTextException()
        {}

        public EmptyTextException( string message ) : base( message )
        {}
    }
}