using System.Collections.Generic;
using System.Linq;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Sinks.File;
using System;

namespace Serilog.Sinks.DateFormat
{
    public class PathToken 
    {

        public PathToken( PathTokenType type, string value )
        {
            Value = value;
            Type = type;
        }

        public string Value { get; set; }
        public PathTokenType Type { get; set; }


        public void Append( string value )
        {
            if( Value == null )
                Value = value;
            else
                Value += value;
        }

        static PathToken _delimiter = new PathToken( PathTokenType.Delimiter, null );
        public static PathToken Delimiter() => _delimiter;

        public static PathToken Part( string value ) => new PathToken( PathTokenType.Part, value );
        public static PathToken Expression( string value ) => new PathToken( PathTokenType.Expression, value );
    }
}
