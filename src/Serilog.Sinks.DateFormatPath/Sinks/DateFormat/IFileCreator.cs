using System;
using System.Linq;

namespace Serilog.Sinks.DateFormat
{
    public interface IFileCreator
    {
        void CreateFile( string path );
    }
}
