using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serilog.Sinks.DateFormat
{
    internal static class Extensions
    {
        public static StringBuilder ConcatToStringBuilder( this Stack<string> stack )
        {
            var sb = stack.Aggregate( new StringBuilder(), ( a, b ) => a.Append( b ) );
            stack.Clear();

            return sb;
        }
    }
}
