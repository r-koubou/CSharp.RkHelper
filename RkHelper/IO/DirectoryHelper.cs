using System.IO;

namespace RkHelper.IO
{
    public static class DirectoryHelper
    {
        public static DirectoryInfo Create( string directoryName )
        {
            return !Directory.Exists( directoryName )
                ? Directory.CreateDirectory( directoryName )
                : new DirectoryInfo( directoryName );
        }
    }
}