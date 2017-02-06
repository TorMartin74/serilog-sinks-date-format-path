using System;
using System.Linq;

namespace Serilog.Sinks.DateFormat
{
    public static class FileCreator
    {
        static IFileCreator _directoryCreator = new FileCreatorImpl();

        public static void CreateFile( string path )
        {
            _directoryCreator.CreateFile( path );
        }


        public static void SetFileCreator( IFileCreator directoryCreator )
        {
            _directoryCreator = directoryCreator;
        }
    }
}
