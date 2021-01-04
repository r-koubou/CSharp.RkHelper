using System;
using System.IO;

namespace RkHelper.IO
{
    public static class PathHelper
    {
        public static string IncrementPathNameWhenExist( string baseDirectory, string fileName )
        {
            var sourcePath = Path.Combine( baseDirectory, fileName );

            if( !File.Exists( sourcePath ) )
            {
                return sourcePath;
            }

            var pathExtension = Path.GetExtension( sourcePath );
            var pathWithoutExtension = Path.GetFileNameWithoutExtension( sourcePath );
            var duplicate = 0;

            var path = sourcePath;

            while( File.Exists( path ) )
            {
                if( duplicate == int.MaxValue )
                {
                    throw new InvalidOperationException( "too many duplicate files!");
                }
                duplicate++;
                path = Path.Combine( baseDirectory, $"{pathWithoutExtension}({duplicate}){pathExtension}" );
            }

            return path;

        }

        public static string IncrementPathNameWhenExist( string sourcePath )
        {
            return IncrementPathNameWhenExist(
                Path.GetDirectoryName( sourcePath ) ?? string.Empty,
                Path.GetFileName( sourcePath )
            );
        }
    }
}