using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Serilog.Sinks.DateFormat
{
    public static class TimeFormatAnalyzer
    {
        public enum FormatComponent
        {
            Year = 6,
            Month = 5,
            Day = 4,
            Hour = 3,
            Minute = 2,
            Second = 1,
        }


        public static FormatComponent GetMinInterval( IEnumerable<string> formats )
        {
            var lesser = FormatComponent.Year;

            foreach( var format in formats )
            {
                var c = GetMinInterval( format );
                if( c < lesser )
                    lesser = c;
            }

            return lesser;
        }


        public static FormatComponent GetMinInterval(string format)
        {
            FormatComponent c = FormatComponent.Year;

            if( format.Contains("y") )
                c = FormatComponent.Year;

            if( format.Contains( "M" ) )
                c = FormatComponent.Month;
            
            if( format.Contains( "d" ) )
                c = FormatComponent.Day;

            if( format.Contains( "h" ) || format.Contains( "H" ) )
                c = FormatComponent.Hour;

            if( format.Contains( "m" ) )
                c = FormatComponent.Minute;

            if( format.Contains( "s" ) )
                c = FormatComponent.Second;

            return c;
        }
    }
}
