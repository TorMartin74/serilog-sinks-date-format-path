using System;
using System.IO;
using System.Linq;

namespace Serilog.Sinks.DateFormat
{
    internal class FileCreatorImpl : IFileCreator
    {
        public void CreateFile( string path )
        {
            Directory.CreateDirectory( path );
        }
    }
}
